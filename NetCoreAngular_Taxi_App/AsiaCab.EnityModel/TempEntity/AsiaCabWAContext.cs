using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class AsiaCabWAContext : DbContext
    {
        public AsiaCabWAContext()
        {
        }

        public AsiaCabWAContext(DbContextOptions<AsiaCabWAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agent> Agent { get; set; }
        public virtual DbSet<AgentGrade> AgentGrade { get; set; }
        public virtual DbSet<AgentStatus> AgentStatus { get; set; }
        public virtual DbSet<AgentType> AgentType { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<BillingDate> BillingDate { get; set; }
        public virtual DbSet<BillingDay> BillingDay { get; set; }
        public virtual DbSet<BillingType> BillingType { get; set; }
        public virtual DbSet<CreditTerm> CreditTerm { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerStatus> CustomerStatus { get; set; }
        public virtual DbSet<CustomerType> CustomerType { get; set; }
        public virtual DbSet<DocType> DocType { get; set; }
        public virtual DbSet<EmployeeCp> EmployeeCp { get; set; }
        public virtual DbSet<EmployeePrefix> EmployeePrefix { get; set; }
        public virtual DbSet<Flavor> Flavor { get; set; }
        public virtual DbSet<MemberInfo> MemberInfo { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Packaging> Packaging { get; set; }
        public virtual DbSet<PaymentType> PaymentType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ResidenceType> ResidenceType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Segment> Segment { get; set; }
        public virtual DbSet<Size> Size { get; set; }
        public virtual DbSet<User> User { get; set; }

        // Unable to generate entity type for table 'mas.ThaiDistrict'. Please see the warning messages.
        // Unable to generate entity type for table 'mas.ThaiGeography'. Please see the warning messages.
        // Unable to generate entity type for table 'mas.ThaiProvince'. Please see the warning messages.
        // Unable to generate entity type for table 'log.EventLog'. Please see the warning messages.
        // Unable to generate entity type for table 'mas.EmployeeAgent'. Please see the warning messages.
        // Unable to generate entity type for table 'mas.ThaiAmphur'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=asiacab.database.windows.net;Initial Catalog=AsiaCabWA;Persist Security Info=False;User ID=adminasiacab;Password=@dmin@si@C@b;Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>(entity =>
            {
                entity.ToTable("Agent", "mas");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.AgentGradeId).HasColumnName("AgentGradeID");

                entity.Property(e => e.AgentStatusId).HasColumnName("AgentStatusID");

                entity.Property(e => e.AgentTypeId).HasColumnName("AgentTypeID");

                entity.Property(e => e.Alley).HasMaxLength(50);

                entity.Property(e => e.AlleyInvoice).HasMaxLength(50);

                entity.Property(e => e.Avpname)
                    .HasColumnName("AVPName")
                    .HasMaxLength(100);

                entity.Property(e => e.BankGuarantee).HasMaxLength(150);

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.CashGuarantee).HasMaxLength(150);

                entity.Property(e => e.ConcessionPlace).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreditLimit).HasMaxLength(200);

                entity.Property(e => e.Cvcode).HasColumnName("CVCode");

                entity.Property(e => e.Details).HasMaxLength(150);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.DistrictInvoice).HasMaxLength(50);

                entity.Property(e => e.Dmname)
                    .HasColumnName("DMName")
                    .HasMaxLength(100);

                entity.Property(e => e.DocTypeId).HasColumnName("DocTypeID");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.EmployeePrefixId).HasColumnName("EmployeePrefixID");

                entity.Property(e => e.FaxNo).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.FirstNameContact).HasMaxLength(100);

                entity.Property(e => e.FirstNameOwner).HasMaxLength(100);

                entity.Property(e => e.Gmname)
                    .HasColumnName("GMName")
                    .HasMaxLength(100);

                entity.Property(e => e.HomePhoneNo).HasMaxLength(20);

                entity.Property(e => e.HouseNo).HasMaxLength(50);

                entity.Property(e => e.HouseNoInvoice).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.LastNameContact).HasMaxLength(100);

                entity.Property(e => e.LastNameOwner).HasMaxLength(100);

                entity.Property(e => e.MobilePhoneNo).HasMaxLength(150);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNoContact).HasMaxLength(150);

                entity.Property(e => e.PhoneNoOwner).HasMaxLength(50);

                entity.Property(e => e.PostalId).HasColumnName("PostalID");

                entity.Property(e => e.PostalIdinvoice).HasColumnName("PostalIDInvoice");

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.ProvinceInvoice).HasMaxLength(50);

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.RegionInvoice).HasMaxLength(50);

                entity.Property(e => e.ResignmentDate).HasColumnType("datetime");

                entity.Property(e => e.Road).HasMaxLength(50);

                entity.Property(e => e.RoadInvoice).HasMaxLength(50);

                entity.Property(e => e.RoomSize).HasMaxLength(50);

                entity.Property(e => e.Sdname)
                    .HasColumnName("SDName")
                    .HasMaxLength(100);

                entity.Property(e => e.SegmentId).HasColumnName("SegmentID");

                entity.Property(e => e.Smname)
                    .HasColumnName("SMName")
                    .HasMaxLength(100);

                entity.Property(e => e.StartEffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.SubDistrict).HasMaxLength(50);

                entity.Property(e => e.SubDistrictInvoice).HasMaxLength(50);

                entity.Property(e => e.TaxId).HasColumnName("TaxID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Village).HasMaxLength(50);

                entity.Property(e => e.VillageInvoice).HasMaxLength(50);

                entity.Property(e => e.VillageNo).HasMaxLength(50);

                entity.Property(e => e.VillageNoInvoice).HasMaxLength(50);
            });

            modelBuilder.Entity<AgentGrade>(entity =>
            {
                entity.ToTable("AgentGrade", "mas");

                entity.Property(e => e.AgentGradeId).HasColumnName("AgentGradeID");

                entity.Property(e => e.AgentGradeName).HasMaxLength(150);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AgentStatus>(entity =>
            {
                entity.ToTable("AgentStatus", "mas");

                entity.Property(e => e.AgentStatusId).HasColumnName("AgentStatusID");

                entity.Property(e => e.AgentStatusName).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AgentType>(entity =>
            {
                entity.ToTable("AgentType", "mas");

                entity.Property(e => e.AgentTypeId).HasColumnName("AgentTypeID");

                entity.Property(e => e.AgentTypeName).HasMaxLength(150);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area", "mas");

                entity.Property(e => e.CareArea)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Bank", "mas");

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.BankName).HasMaxLength(150);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.ToTable("Banner", "mas");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageData).IsRequired();

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ImageUrl).HasMaxLength(10);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BillingDate>(entity =>
            {
                entity.ToTable("BillingDate", "mas");

                entity.Property(e => e.BillingDateId).HasColumnName("BillingDateID");

                entity.Property(e => e.BillingDateName).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BillingDay>(entity =>
            {
                entity.ToTable("BillingDay", "mas");

                entity.Property(e => e.BillingDayId).HasColumnName("BillingDayID");

                entity.Property(e => e.BillingDayName).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BillingType>(entity =>
            {
                entity.ToTable("BillingType", "mas");

                entity.Property(e => e.BillingTypeId).HasColumnName("BillingTypeID");

                entity.Property(e => e.BillingTypeName).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CreditTerm>(entity =>
            {
                entity.ToTable("CreditTerm", "mas");

                entity.Property(e => e.CreditTermId).HasColumnName("CreditTermID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreditTermName).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "mas");

                entity.Property(e => e.AddressHome).HasColumnName("Address_Home");

                entity.Property(e => e.AddressShipment).HasColumnName("Address_Shipment");

                entity.Property(e => e.AlleyHome)
                    .HasColumnName("Alley_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.AlleyShipment)
                    .HasColumnName("Alley_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.BillingDateId).HasColumnName("BillingDateID");

                entity.Property(e => e.BillingDayId).HasColumnName("BillingDayID");

                entity.Property(e => e.BillingTypeId).HasColumnName("BillingTypeID");

                entity.Property(e => e.ContactName).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerStatusId).HasColumnName("CustomerStatusID");

                entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");

                entity.Property(e => e.Cvcode)
                    .HasColumnName("CVCode")
                    .HasMaxLength(50);

                entity.Property(e => e.DateOfBirth).HasMaxLength(50);

                entity.Property(e => e.DistrictHome)
                    .HasColumnName("District_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictShipment)
                    .HasColumnName("District_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.HomePhone).HasMaxLength(100);

                entity.Property(e => e.HouseNumberHome)
                    .HasColumnName("HouseNumber_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.HouseNumberShipment)
                    .HasColumnName("HouseNumber_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MobilePhone).HasMaxLength(100);

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.PostalIdHome)
                    .HasColumnName("PostalID_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalIdShipment)
                    .HasColumnName("PostalID_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.ProvinceHome)
                    .HasColumnName("Province_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.ProvinceShipment)
                    .HasColumnName("Province_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.ResidenceTypeId).HasColumnName("ResidenceTypeID");

                entity.Property(e => e.RoadHome)
                    .HasColumnName("Road_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.RoadShipment)
                    .HasColumnName("Road_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.RouteName).HasMaxLength(50);

                entity.Property(e => e.Spname)
                    .HasColumnName("SPName")
                    .HasMaxLength(200);

                entity.Property(e => e.SubDistrictHome)
                    .HasColumnName("SubDistrict_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.SubDistrictShipment)
                    .HasColumnName("SubDistrict_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VillageHome)
                    .HasColumnName("Village_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.VillageNumberHome)
                    .HasColumnName("VillageNumber_Home")
                    .HasMaxLength(50);

                entity.Property(e => e.VillageNumberShipment)
                    .HasColumnName("VillageNumber_Shipment")
                    .HasMaxLength(50);

                entity.Property(e => e.VillageShipment)
                    .HasColumnName("Village_Shipment")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerStatus>(entity =>
            {
                entity.ToTable("CustomerStatus", "mas");

                entity.Property(e => e.CustomerStatusId).HasColumnName("CustomerStatusID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerStatusName).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.ToTable("CustomerType", "mas");

                entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerTypeName).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DocType>(entity =>
            {
                entity.ToTable("DocType", "mas");

                entity.Property(e => e.DocTypeId).HasColumnName("DocTypeID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DocTypeName).HasMaxLength(150);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeeCp>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.ToTable("EmployeeCP", "mas");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.SalaryId).HasColumnName("SalaryID");

                entity.Property(e => e.StartWorkDate).HasColumnType("datetime");

                entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<EmployeePrefix>(entity =>
            {
                entity.ToTable("EmployeePrefix", "mas");

                entity.Property(e => e.EmployeePrefixId).HasColumnName("EmployeePrefixID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeePrefixName).HasMaxLength(150);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Flavor>(entity =>
            {
                entity.ToTable("Flavor", "mas");

                entity.Property(e => e.FlavorId).HasColumnName("FlavorID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FlavorName).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MemberInfo>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("MemberInfo", "mas");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("EMail")
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Idcard).HasColumnName("IDCard");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.PathImage).HasMaxLength(100);

                entity.Property(e => e.PrefixId).HasColumnName("PrefixID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.ToTable("News", "mas");

                entity.Property(e => e.NewsId).HasColumnName("News_Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NewsDescription).HasColumnName("News_Description");

                entity.Property(e => e.NewsImage)
                    .HasColumnName("News_Image")
                    .IsUnicode(false);

                entity.Property(e => e.NewsTopics).HasColumnName("News_Topics");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order", "mas");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.AgentName).HasMaxLength(150);

                entity.Property(e => e.Amount).HasMaxLength(200);

                entity.Property(e => e.CpreceiveDate)
                    .HasColumnName("CPReceiveDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Cvcode)
                    .HasColumnName("CVCode")
                    .HasMaxLength(150);

                entity.Property(e => e.FavoriteCustomer).HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasMaxLength(50);

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.Sppickup)
                    .HasColumnName("SPPickup")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusPo)
                    .HasColumnName("StatusPO")
                    .HasMaxLength(150);

                entity.Property(e => e.StockOnHold).HasMaxLength(150);

                entity.Property(e => e.Unit).HasMaxLength(50);
            });

            modelBuilder.Entity<Packaging>(entity =>
            {
                entity.ToTable("Packaging", "mas");

                entity.Property(e => e.PackagingId).HasColumnName("PackagingID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.PackagingName).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.ToTable("PaymentType", "mas");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentTypeName).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "mas");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FlavorId).HasColumnName("FlavorID");

                entity.Property(e => e.ProductName).HasMaxLength(150);

                entity.Property(e => e.SapproductCode)
                    .HasColumnName("SAPProductCode")
                    .HasMaxLength(50);

                entity.Property(e => e.SegmentId).HasColumnName("SegmentID");

                entity.Property(e => e.SizeId).HasColumnName("SizeID");

                entity.Property(e => e.Spprice).HasColumnName("SPPrice");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ResidenceType>(entity =>
            {
                entity.ToTable("ResidenceType", "mas");

                entity.Property(e => e.ResidenceTypeId).HasColumnName("ResidenceTypeID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ResidenceTypeName).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "mas");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateBy).HasMaxLength(10);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.Property(e => e.UpdateBy).HasMaxLength(10);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserType).HasMaxLength(10);
            });

            modelBuilder.Entity<Segment>(entity =>
            {
                entity.ToTable("Segment", "mas");

                entity.Property(e => e.SegmentId).HasColumnName("SegmentID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.SegmentName).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.ToTable("Size", "mas");

                entity.Property(e => e.SizeId).HasColumnName("SizeID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.SizeName).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "sy");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AuthProvder).HasMaxLength(50);

                entity.Property(e => e.Compare).IsRequired();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.PasswordUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SaltAes)
                    .IsRequired()
                    .HasColumnName("SaltAES");

                entity.Property(e => e.SaltHash)
                    .IsRequired()
                    .HasColumnName("SaltHASH");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserType).HasMaxLength(10);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
