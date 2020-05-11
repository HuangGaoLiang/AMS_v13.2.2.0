using System;
using System.Linq;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 打印次数调用服务
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    internal static class PrintCounterService
    {
        /*
         * 打印序号规则：校区简称+年+5位数字，例如“深圳东海201800001”
         * 每打印一次，就计数一次，连续，不能断号。
         * 分文件计数，目前系统中的打印文件有报班报名表、、报班收据、定金收据、游学营报名表、游学营收据、零售票据。
         * 计数规则如下：校区、年度、单据类型 从00001开始。
         * 校区名称+年度+序号，例如：东海校区+2018+00001
         */

        /// <summary>
        /// 打印并计数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="printBillType">打印类型</param>
        /// <returns>打印序号</returns>
        /// <exception cref="ArgumentNullException">
        /// 校区Id为空;校区未找到；
        /// </exception>
        internal static string Print(string schoolId, PrintBillType printBillType)
        {
            if (string.IsNullOrWhiteSpace(schoolId))
            {
                throw new ArgumentNullException(nameof(schoolId));
            }

            OrgService orgService = new OrgService();

            var allSchoolInfos = orgService.GetAllSchoolList();

            var schoolInfo = allSchoolInfos.FirstOrDefault(m => m.SchoolId == schoolId);

            if (schoolInfo == null)
            {
                throw new ArgumentNullException(nameof(schoolInfo));
            }

            int year = DateTime.Now.Year;
            byte type = (byte)printBillType;

            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_PRINT, schoolId, type.ToString()))
            {
                TblDatPrintCounterRepository printCounterRepository = new TblDatPrintCounterRepository();
                TblDatPrintCounter counter = printCounterRepository.Get(schoolId, year, type);

                if (counter == null)
                {
                    counter = new TblDatPrintCounter
                    {
                        PrintCounterId = IdGenerator.NextId(),
                        PrintBillType = type,
                        SchoolId = schoolId,
                        Counts = 1,
                        Year = year,
                        CreateTime = DateTime.Now
                    };
                    printCounterRepository.Add(counter);
                }
                else
                {
                    counter.Counts++;
                    printCounterRepository.Update(counter);
                }

                return $"{schoolInfo.SchoolName}{year}{counter.Counts.ToString().PadLeft(5, '0')}";
            }
        }
    }
}
