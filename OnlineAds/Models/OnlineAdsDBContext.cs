using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OnlineAds.Models
{
    public partial class OnlineAdsDBContext : DbContext
    {
        public OnlineAdsDBContext()
        {
        }

        public OnlineAdsDBContext(DbContextOptions<OnlineAdsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAdmin> TblAdmins { get; set; }
        public virtual DbSet<TblCategory> TblCategories { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblUser> TblUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source = VICTOR\\SQLEXPRESS01; Initial Catalog=OnlineAdsDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAdmin>(entity =>
            {
                entity.HasKey(e => e.AdId)
                    .HasName("PK__tbl_admi__CAA4A62777FBC0C2");

                entity.ToTable("tbl_admin");

                entity.HasIndex(e => e.AdUsername, "UQ__tbl_admi__9CC20817182CD587")
                    .IsUnique();

                entity.Property(e => e.AdId).HasColumnName("ad_id");

                entity.Property(e => e.AdPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ad_password");

                entity.Property(e => e.AdUsername)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ad_username");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.HasKey(e => e.CatId)
                    .HasName("PK__tbl_cate__DD5DDDBDD63083E6");

                entity.ToTable("tbl_category");

                entity.HasIndex(e => e.CatName, "UQ__tbl_cate__FA8C1790C3D46713")
                    .IsUnique();

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.CatFkAd).HasColumnName("cat_fk_ad");

                entity.Property(e => e.CatImage)
                    .IsRequired()
                    .HasColumnName("cat_image");

                entity.Property(e => e.CatName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cat_name");

                entity.Property(e => e.CatStatus).HasColumnName("cat_status");

                entity.HasOne(d => d.CatFkAdNavigation)
                    .WithMany(p => p.TblCategories)
                    .HasForeignKey(d => d.CatFkAd)
                    .HasConstraintName("FK__tbl_categ__cat_f__3A81B327");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.ProId)
                    .HasName("PK__tbl_prod__335E4CA6D3DB64DD");

                entity.ToTable("tbl_product");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.Property(e => e.ProDes)
                    .IsRequired()
                    .HasColumnName("pro_des");

                entity.Property(e => e.ProFkCat).HasColumnName("pro_fk_cat");

                entity.Property(e => e.ProFkUser).HasColumnName("pro_fk_user");

                entity.Property(e => e.ProImage)
                    .IsRequired()
                    .HasColumnName("pro_image");

                entity.Property(e => e.ProName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("pro_name");

                entity.Property(e => e.ProPrice).HasColumnName("pro_price");

                entity.HasOne(d => d.ProFkCatNavigation)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.ProFkCat)
                    .HasConstraintName("FK__tbl_produ__pro_f__412EB0B6");

                entity.HasOne(d => d.ProFkUserNavigation)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.ProFkUser)
                    .HasConstraintName("FK__tbl_produ__pro_f__4222D4EF");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PK__tbl_user__B51D3DEAB082FBE5");

                entity.ToTable("tbl_user");

                entity.HasIndex(e => e.UEmail, "UQ__tbl_user__3DF9EF22893B61F9")
                    .IsUnique();

                entity.HasIndex(e => e.UContact, "UQ__tbl_user__F8193DB75D663E92")
                    .IsUnique();

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.UCity)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("u_city");

                entity.Property(e => e.UContact)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("u_contact");

                entity.Property(e => e.UDob)
                    .HasColumnType("date")
                    .HasColumnName("u_dob");

                entity.Property(e => e.UEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("u_email");

                entity.Property(e => e.UGender)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("u_gender");

                entity.Property(e => e.UImage)
                    .IsRequired()
                    .HasColumnName("u_image");

                entity.Property(e => e.UName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("u_name");

                entity.Property(e => e.UPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("u_password");

                entity.Property(e => e.UState)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("u_state");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
