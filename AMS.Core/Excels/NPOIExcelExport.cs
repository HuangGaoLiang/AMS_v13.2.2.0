using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AMS.Core
{
    /// <summary>
    /// 使用NPOI导出到EXCEL
    /// caiyakang 2018-11-19
    /// </summary>
    public class NPOIExcelExport
    {
        private bool _isTitleRender { get; set; }  //是否已经渲染标题
        private IWorkbook _workBook;               //表示一个工作蒲
        public string Title { get; set; }  //默认标题


        /// <summary>
        /// 实例化一个EXCEL导出对象
        /// </summary>
        public NPOIExcelExport() : this("")
        { }

        /// <summary>
        /// 暂不支持模板,以后才支持
        /// </summary>
        /// <param name="templatePath"></param>
        protected NPOIExcelExport(string templatePath)
        {
            this.CreateBook();
        }


        /// <summary>
        /// 创建工作浦
        /// </summary>
        private void CreateBook()
        {
            if (_workBook == null)
            {
                _workBook = new HSSFWorkbook();
            }
        }

        /// <summary>
        /// 导出EXCEL并下载
        /// </summary>
        /// <param name="controller">控制器实例</param>
        /// <param name="fileName">文件名</param>
        public FileContentResult DownloadToFile(ControllerBase controller, string fileName)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                this._workBook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                data = ms.GetBuffer();
            }
            return controller.File(data, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 添加导出数据行
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">要导出的数据集合</param>
        /// <param name="header">列头</param>
        /// <param name="sheetName">EXCEL的页签名称</param>
        /// <returns></returns>
        public int Add<T>(List<T> list, string[] header, string sheetName = "")
        {
            return this.Append<T>(list, header, 0, sheetName);
        }


        /// <summary>
        /// 追加导出数据行
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">要导出的数据集合</param>
        /// <param name="header">列头</param>
        /// <param name="rowIndex">列头</param>
        /// <param name="sheetName">EXCEL的页签名称</param>
        /// <returns></returns>
        private int Append<T>(List<T> data, string[] header, int rowIndex, string sheetName = "")
        {
            int position = rowIndex;
            ISheet worksheet = this.GetSheet(sheetName);
            int columnCount = header == null ? data.Count : header.Count();
            this.AppendBegin(worksheet, header, columnCount, ref position);
            this.AppendBody(worksheet, data, ref position);
            AppendEnd(worksheet);
            return position;
        }


        /// <summary>
        /// 追加数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="worksheet"></param>
        /// <param name="data">要导出的数据集合</param>
        /// <param name="rowIndex">行索引,从哪一行开始</param>
        private void AppendBody<T>(ISheet worksheet, List<T> data, ref int rowIndex)
        {
            IRow rows = null;
            Type entityType = data[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            for (int i = 0; i < data.Count; i++)
            {
                rows = worksheet.CreateRow(rowIndex);
                object entity = data[i];
                for (int j = 0; j < entityProperties.Length; j++)
                {
                    object[] entityValues = new object[entityProperties.Length];
                    entityValues[j] = entityProperties[j].GetValue(entity);
                    rows.CreateCell(j).SetCellValue(entityValues[j].ToString());
                }
                rowIndex++;
            }
        }


        /// <summary>
        /// 追加之前
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="header">列头</param>
        /// <param name="columnCount"></param>
        /// <param name="rowIndex">行索引,从哪一行开始</param>
        private void AppendBegin(ISheet worksheet, string[] header, int columnCount, ref int rowIndex)
        {
            this.CreateTitle(worksheet, columnCount, ref rowIndex);
            this.CreateHeader(worksheet, header, ref rowIndex);
        }


        /// <summary>
        /// 追加结束之后
        /// </summary>
        /// <param name="worksheet"></param>
        private void AppendEnd(ISheet worksheet)
        {
            throw new Exception("未实现");
        }


        /// <summary>
        /// 创建表列头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="header"></param>
        /// <param name="rowIndex"></param>
        private void CreateHeader(ISheet sheet, string[] header, ref int rowIndex)
        {
            if (header != null && header.Count() > 0)
            {
                IRow rowTitle = sheet.CreateRow(rowIndex);
                for (int i = 0; i < header.Length; i++)
                {
                    rowTitle.CreateCell(i).SetCellValue(header[i]);
                }
                rowIndex++;
            }
        }


        /// <summary>
        /// 创建标题
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnCount"></param>
        /// <param name="rowIndex"></param>
        private void CreateTitle(ISheet sheet, int columnCount, ref int rowIndex)
        {
            //创建标题,标题默认会居中
            if (!_isTitleRender)
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    sheet.CreateRow(rowIndex).CreateCell(0);
                    CellRangeAddress cellRangeAddress = new CellRangeAddress(rowIndex, rowIndex, rowIndex, columnCount);
                    sheet.AddMergedRegion(cellRangeAddress);
                    ICell cell = sheet.GetRow(cellRangeAddress.FirstRow).GetCell(cellRangeAddress.FirstColumn);
                    ICellStyle style = this._workBook.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.Center;//设置单元格的样式：水平对齐居中
                    cell.CellStyle = style;
                    cell.SetCellValue(Title);
                    rowIndex++;
                }
                _isTitleRender = true;
            }
        }

        private ISheet GetSheet(string name)
        {
            return GetSheet(this._workBook, name);
        }
        private ISheet GetSheet(IWorkbook book, string name)
        {
            //创建或取得SHEET
            ISheet sheet = null;
            if (book.NumberOfSheets > 0)
            {
                sheet = book.GetSheetAt(0);
            }
            else
            {
                if (!string.IsNullOrEmpty(name))
                {
                    sheet = book.GetSheet(name);
                    if (sheet == null)
                    {
                        sheet = book.CreateSheet(name);
                    }
                }
                else
                {
                    sheet = book.CreateSheet("sheet1");
                }
            }
            return sheet;
        }


        ~NPOIExcelExport()
        {
            if (this._workBook != null)
            {
                this._workBook.Dispose();
            }
        }
    }
}
