﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HHLWedding.DataAssmblly
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HHL_WeddingEntities : DbContext
    {
        public HHL_WeddingEntities()
            : base("name=HHL_WeddingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Sys_UserJurisdiction> Sys_UserJurisdiction { get; set; }
        public virtual DbSet<Sys_Channel> Sys_Channel { get; set; }
        public virtual DbSet<Sys_Department> Sys_Department { get; set; }
        public virtual DbSet<Sys_EmployeeJob> Sys_EmployeeJob { get; set; }
        public virtual DbSet<Sys_EmployeeType> Sys_EmployeeType { get; set; }
        public virtual DbSet<Sys_Employee> Sys_Employee { get; set; }
        public virtual DbSet<FD_HotelLabel> FD_HotelLabel { get; set; }
        public virtual DbSet<CommonImages> CommonImages { get; set; }
        public virtual DbSet<Sys_EmployeePower> Sys_EmployeePower { get; set; }
        public virtual DbSet<MyMusic> MyMusic { get; set; }
        public virtual DbSet<sm_Message> sm_Message { get; set; }
        public virtual DbSet<sys_LoginLog> sys_LoginLog { get; set; }
        public virtual DbSet<FL_Customer> FL_Customer { get; set; }
        public virtual DbSet<FL_Invite> FL_Invite { get; set; }
        public virtual DbSet<FD_SaleSource> FD_SaleSource { get; set; }
        public virtual DbSet<FD_SaleType> FD_SaleType { get; set; }
        public virtual DbSet<FL_InviteDetails> FL_InviteDetails { get; set; }
        public virtual DbSet<FD_Hotel> FD_Hotel { get; set; }
        public virtual DbSet<SS_Report> SS_Report { get; set; }
        public virtual DbSet<FL_Order> FL_Order { get; set; }
        public virtual DbSet<FL_OrderDetails> FL_OrderDetails { get; set; }
    }
}
