using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SlicingPieAPI.Models
{
    public partial class SWD_SlicingPieContext : DbContext
    {
        public SWD_SlicingPieContext()
        {
        }

        public SWD_SlicingPieContext(DbContextOptions<SWD_SlicingPieContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }
        public virtual DbSet<SliceAsset> SliceAssets { get; set; }
        public virtual DbSet<StakeHolder> StakeHolders { get; set; }
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

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.NameAccount)
                    .HasName("UQ__StackHol__55E5D2B87CBA9E52")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .HasColumnName("AccountID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.EmailAccount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameAccount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneAccount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.HasIndex(e => e.CompanyName)
                    .HasName("UQ__Company__9BCE05DC38A2856E")
                    .IsUnique();

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CashMultiplier).HasDefaultValueSql("((4))");

                entity.Property(e => e.ComapnyIcon).IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NonCashMultiplier).HasDefaultValueSql("((2))");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
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
                    .HasConstraintName("FK__Project__Company__5CD6CB2B");
            });

            modelBuilder.Entity<ProjectDetail>(entity =>
            {
                entity.HasKey(e => new { e.TermId, e.ProjectId })
                    .HasName("PK__ProjectD__566B85A857C3176A");

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
                    .HasConstraintName("FK__ProjectDe__Proje__5DCAEF64");

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.ProjectDetails)
                    .HasForeignKey(d => d.TermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectDe__TermI__5EBF139D");
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

                entity.Property(e => e.AccountId)
                    .HasColumnName("AccountID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AssetStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TermId).HasColumnName("TermID");

                entity.Property(e => e.TimeAsset).HasColumnType("datetime");

                entity.Property(e => e.TypeAssetId).HasColumnName("TypeAssetID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Asset__CFID__534D60F1");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_SliceAsset_Company");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Asset__ProjectID__5441852A");

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.TermId)
                    .HasConstraintName("FK__Asset__TermID__5629CD9C");

                entity.HasOne(d => d.TypeAsset)
                    .WithMany(p => p.SliceAssets)
                    .HasForeignKey(d => d.TypeAssetId)
                    .HasConstraintName("FK__Asset__TypeAsset__5535A963");
            });

            modelBuilder.Entity<StakeHolder>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.CompanyId })
                    .HasName("PK__StackHol__42B0C85BF69BEC18");

                entity.ToTable("StakeHolder");

                entity.Property(e => e.AccountId)
                    .HasColumnName("AccountID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
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

                entity.Property(e => e.Shrole).HasColumnName("SHRole");

                entity.Property(e => e.Shsalary).HasColumnName("SHSalary");

                entity.Property(e => e.Shstatus)
                    .HasColumnName("SHStatus")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.StakeHolders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StackHole__Stack__66603565");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.StakeHolders)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StackHole__Compa__656C112C");
            });

            modelBuilder.Entity<TermSlouse>(entity =>
            {
                entity.HasKey(e => e.TermId)
                    .HasName("PK__TermSlic__410A2E45C7B9E7BC");

                entity.Property(e => e.TermId).HasColumnName("TermID");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TermName).HasMaxLength(50);

                entity.Property(e => e.TermTimeFrom).HasColumnType("datetime");

                entity.Property(e => e.TermTimeTo).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TermSlice)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__TermSlice__Compa__693CA210");
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
