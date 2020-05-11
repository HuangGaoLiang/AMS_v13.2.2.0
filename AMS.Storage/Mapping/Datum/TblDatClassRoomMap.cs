/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 教室
     /// </summary>
   class TblDatClassRoomMap : IEntityTypeConfiguration<TblDatClassRoom>
   {
        void IEntityTypeConfiguration<TblDatClassRoom>.Configure(EntityTypeBuilder<TblDatClassRoom> entity)
        {
           // Table
           entity.ToTable("TblDatClassRoom");

           // Primary Key
           entity.HasKey(e => e.ClassRoomId);

           // Properties
           entity.Property(e => e.ClassRoomId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.RoomNo)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.ClassRoomId).HasColumnName("ClassRoomId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.RoomNo).HasColumnName("RoomNo");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
