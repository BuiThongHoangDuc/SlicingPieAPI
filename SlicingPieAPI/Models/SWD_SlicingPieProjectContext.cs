using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SlicingPieAPI.Models
{
    public partial class SWD_SlicingPieProjectContext : DbContext
    {
        public SWD_SlicingPieProjectContext()
        {
        }

        public SWD_SlicingPieProjectContext(DbContextOptions<SWD_SlicingPieProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SliceAsset> SliceAssets { get; set; }
        public virtual DbSet<StackHolder> StackHolders { get; set; }
        public virtual DbSet<StackHolerDetail> StackHolerDetails { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<TermSlouse> TermSlice { get; set; }
        public virtual DbSet<TypeAsset> TypeAssets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=SWD_SlicingPieDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.HasIndex(e => e.CompanyName)
                    .HasName("UQ__Company__9BCE05DC797E2AAA")
                    .IsUnique();

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ComapnyIcon).IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__Project__Company__3A81B327");
            });

            modelBuilder.Entity<ProjectDetail>(entity =>
            {
                entity.HasKey(e => new { e.TermId, e.ProjectId })
                    .HasName("PK__ProjectD__566B85A81F019CFE");

                entity.ToTable("ProjectDetail");

                entity.Property(e => e.TermId).HasColumnName("TermID");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectDetails)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectDe__Proje__59FA5E80");

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.ProjectDetails)
                    .HasForeignKey(d => d.TermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectDe__TermI__59063A47");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.NameRole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SliceAsset>(entity =>
            {
                entity.HasKey(e => e.AssetId)
                    .HasName("PK__Asset__4349237288ED6B0F");

                entity.ToTable("SliceAsset");

                entity.Property(e => e.AssetId)
                    .HasColumnName("AssetID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AssetStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StackHolerId)
                    .HasColumnName("StackHolerID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TermId).HasColumnName("TermID");

                entity.Property(e => e.TimeAsset).HasColumnType("datetime");

                entity.Property(e => e.TypeAssetId).HasColumnName("TypeAssetID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Asset__ProjectID__5441852A");

                entity.HasOne(d => d.StackHoler)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.StackHolerId)
                    .HasConstraintName("FK__Asset__CFID__534D60F1");

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.TermId)
                    .HasConstraintName("FK__Asset__TermID__5629CD9C");

                entity.HasOne(d => d.TypeAsset)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.TypeAssetId)
                    .HasConstraintName("FK__Asset__TypeAsset__5535A963");
            });

            modelBuilder.Entity<StackHolder>(entity =>
            {
                entity.HasKey(e => e.StackHolerId)
                    .HasName("PK__Co_Found__F54E0D5C57ADFFE7");

                entity.ToTable("StackHolder");

                entity.HasIndex(e => e.Shaccount)
                    .HasName("UQ__Co_Found__4C1236C94D8A4E4B")
                    .IsUnique();

                entity.Property(e => e.StackHolerId)
                    .HasColumnName("StackHolerID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Shaccount)
                    .HasColumnName("SHAccount")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Shemail)
                    .HasColumnName("SHEmail")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShphoneNo)
                    .HasColumnName("SHPhoneNo")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.StackHolders)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Co_Founde__RoleI__44FF419A");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.StackHolders)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__Co_Founde__Statu__440B1D61");
            });

            modelBuilder.Entity<StackHolerDetail>(entity =>
            {
                entity.HasKey(e => new { e.StackHolerId, e.CompanyId })
                    .HasName("PK__Co_Found__27977C98767D55BC");

                entity.ToTable("StackHolerDetail");

                entity.Property(e => e.StackHolerId)
                    .HasColumnName("StackHolerID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Shdtstatus)
                    .HasColumnName("SHDTStatus")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Shimage)
                    .HasColumnName("SHImage")
                    .IsUnicode(false);

                entity.Property(e => e.Shjob)
                    .HasColumnName("SHJob")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShmarketSalary).HasColumnName("SHMarketSalary");

                entity.Property(e => e.ShnameForCompany)
                    .HasColumnName("SHNameForCompany")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Shsalary).HasColumnName("SHSalary");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.StackHolerDetails)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Co_Founde__Compa__4CA06362");

                entity.HasOne(d => d.ShdtstatusNavigation)
                    .WithMany(p => p.StackHolerDetails)
                    .HasForeignKey(d => d.Shdtstatus)
                    .HasConstraintName("FK_StackHolerDetail_Status");

                entity.HasOne(d => d.StackHoler)
                    .WithMany(p => p.StackHolerDetails)
                    .HasForeignKey(d => d.StackHolerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Co_Founder__CFID__4BAC3F29");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TermSlouse>(entity =>
            {
                entity.HasKey(e => e.TermId)
                    .HasName("PK__TermOfCo__410A2E4581CF14F5");

                entity.Property(e => e.TermId).HasColumnName("TermID");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TermTimeFrom).HasColumnType("datetime");

                entity.Property(e => e.TermTimeTo).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TermSlice)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__TermOfCom__Compa__4F7CD00D");
            });

            modelBuilder.Entity<TypeAsset>(entity =>
            {
                entity.ToTable("TypeAsset");

                entity.Property(e => e.TypeAssetId)
                    .HasColumnName("TypeAssetID")
                    .ValueGeneratedNever();

                entity.Property(e => e.MultiplierType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NameAsset)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
