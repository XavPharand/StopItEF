using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StopItEF.Models
{
    public partial class cloakinglebg_webserverContext : DbContext
    {
        public cloakinglebg_webserverContext()
        {
        }

        public cloakinglebg_webserverContext(DbContextOptions<cloakinglebg_webserverContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Link> Links { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Vote> Votes { get; set; } = null!;

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=mysql-cloakinglebg.alwaysdata.net;port=3306;database=cloakinglebg_webserver;uid=281659;pwd=projets2022!;sslmode=Preferred", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.7-mariadb"));
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.HasIndex(e => e.LinkId, "fk2_lid");

                entity.HasIndex(e => e.UserId, "fk3_uid");

                entity.Property(e => e.CommentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("comment_id");

                entity.Property(e => e.LinkId)
                    .HasColumnType("int(11)")
                    .HasColumnName("link_id");

                entity.Property(e => e.PublicationDate).HasColumnName("publicationDate");

                entity.Property(e => e.Text)
                    .HasMaxLength(2000)
                    .HasColumnName("text");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Link)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.LinkId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk2_lid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk3_uid");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.ToTable("Link");

                entity.HasIndex(e => e.UserId, "fk1_uid");

                entity.Property(e => e.LinkId)
                    .HasColumnType("int(11)")
                    .HasColumnName("link_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("description");

                entity.Property(e => e.PublicationDate).HasColumnName("publicationDate");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Links)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk1_uid");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Pseudo)
                    .HasMaxLength(30)
                    .HasColumnName("pseudo");
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasKey(e => new { e.VoteId, e.UserId, e.LinkId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("Vote");

                entity.HasIndex(e => e.LinkId, "fk1_lid");

                entity.HasIndex(e => e.UserId, "fk2_uid");

                entity.Property(e => e.VoteId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("vote_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.Property(e => e.LinkId)
                    .HasColumnType("int(11)")
                    .HasColumnName("link_id");

                entity.Property(e => e.Value)
                    .HasColumnType("int(11)")
                    .HasColumnName("value");

                entity.HasOne(d => d.Link)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(d => d.LinkId)
                    .HasConstraintName("fk1_lid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk2_uid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
