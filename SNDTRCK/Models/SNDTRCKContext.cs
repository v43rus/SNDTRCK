using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SNDTRCK.Models.User;

namespace SNDTRCK.Models;

public partial class SNDTRCKContext : DbContext
{
	public SNDTRCKContext()
	{
	}

	public SNDTRCKContext(DbContextOptions<SNDTRCKContext> options)
		: base(options)
	{
	}

	public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

	public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

	public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

	public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

	public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

	public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

	public virtual DbSet<NewsletterSignup> NewsletterSignups { get; set; }

	public virtual DbSet<Product> Products { get; set; }
	public virtual DbSet<Order> Orders { get; set; }
	public virtual DbSet<OrderDetail> OrderDetails { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AspNetRole>(entity =>
		{
			entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
				.IsUnique()
				.HasFilter("([NormalizedName] IS NOT NULL)");

			entity.Property(e => e.Name).HasMaxLength(256);
			entity.Property(e => e.NormalizedName).HasMaxLength(256);
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD38CB737A");

			entity.Property(e => e.CoverImageLink)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Description).HasColumnType("text");
			entity.Property(e => e.Genre)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
			entity.Property(e => e.Title)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.RecordLabel)
				.HasMaxLength(250)
				.IsUnicode(false);
			entity.Property(e => e.ReleaseYear)
				.HasColumnType("int");
			entity.Property(e => e.DiscogId)
				.HasColumnType("int");
		});

		modelBuilder.Entity<NewsletterSignup>(entity =>
		{
			entity.HasKey(e => e.NewsletterId).HasName("PK__Newslett__34A1DFFDD50BE1E7");

			entity.ToTable("NewsletterSignup");

			entity.Property(e => e.UserId).HasMaxLength(450);

			entity.HasOne(d => d.User).WithMany(p => p.NewsletterSignups)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK__Newslette__UserI__04E4BC85");
		});

		{
			modelBuilder.Entity<AspNetUserRole>(entity =>
			{
				entity.HasKey(e => new { e.UserId, e.RoleId });

				entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");
			});

			OnModelCreatingPartial(modelBuilder);
		};

		modelBuilder.Entity<AspNetRoleClaim>(entity =>
		{
			entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

			entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
		});

		modelBuilder.Entity<AspNetUser>(entity =>
		{
			entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

			entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
				.IsUnique()
				.HasFilter("([NormalizedUserName] IS NOT NULL)");

			entity.Property(e => e.Address).HasMaxLength(255);
			entity.Property(e => e.City).HasMaxLength(100);
			entity.Property(e => e.Email).HasMaxLength(256);
			entity.Property(e => e.FirstName).HasMaxLength(50);
			entity.Property(e => e.LastName).HasMaxLength(50);
			entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
			entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
			entity.Property(e => e.PostalCode).HasMaxLength(20);
			entity.Property(e => e.UserName).HasMaxLength(256);
		});

		modelBuilder.Entity<AspNetUserClaim>(entity =>
		{
			entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

			entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
		});

		modelBuilder.Entity<AspNetUserLogin>(entity =>
		{
			entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

			entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

			entity.Property(e => e.LoginProvider).HasMaxLength(128);
			entity.Property(e => e.ProviderKey).HasMaxLength(128);

			entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
		});

		modelBuilder.Entity<AspNetUserToken>(entity =>
		{
			entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

			entity.Property(e => e.LoginProvider).HasMaxLength(128);
			entity.Property(e => e.Name).HasMaxLength(128);

			entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
		});

		modelBuilder.Entity<Order>(entity =>
		{
			entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFE81A436E");

			entity.Property(e => e.Address).HasMaxLength(200);
			entity.Property(e => e.City).HasMaxLength(50);
			entity.Property(e => e.Email).HasMaxLength(100);
			entity.Property(e => e.FirstName).HasMaxLength(50);
			entity.Property(e => e.LastName).HasMaxLength(50);
			entity.Property(e => e.OrderDate).HasColumnType("datetime");
			entity.Property(e => e.OrderStatus).HasMaxLength(50);
			entity.Property(e => e.OrderSum).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.PhoneNumber).HasMaxLength(20);
			entity.Property(e => e.PostalCode).HasMaxLength(10);
			entity.Property(e => e.UserId).HasMaxLength(450);
		});

		modelBuilder.Entity<OrderDetail>(entity =>
		{
			entity.HasNoKey();

			entity.HasOne(d => d.Order).WithMany()
				.HasForeignKey(d => d.OrderId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_OrderDetails_Orders");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
