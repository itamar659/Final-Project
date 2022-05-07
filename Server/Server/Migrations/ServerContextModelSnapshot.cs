﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(ServerContext))]
    partial class ServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Server.Models.JukeboxClient", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SessionKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Token");

                    b.ToTable("JukeboxClient");
                });

            modelBuilder.Entity("Server.Models.JukeboxHost", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SessionKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Token");

                    b.ToTable("JukeboxHost");
                });

            modelBuilder.Entity("Server.Models.JukeboxSession", b =>
                {
                    b.Property<string>("SessionKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ActiveUsers")
                        .HasColumnType("int");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SongDuration")
                        .HasColumnType("float");

                    b.Property<string>("SongName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SongPosition")
                        .HasColumnType("float");

                    b.Property<int>("TotalUsers")
                        .HasColumnType("int");

                    b.HasKey("SessionKey");

                    b.ToTable("JukeboxSession");
                });
#pragma warning restore 612, 618
        }
    }
}
