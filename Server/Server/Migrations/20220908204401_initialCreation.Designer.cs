﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(ServerContext))]
    [Migration("20220908204401_initialCreation")]
    partial class initialCreation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApplication1.Models.InfoHostname", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JukeboxClientToken")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("JukeboxClientToken");

                    b.ToTable("InfoHostname");
                });

            modelBuilder.Entity("WebApplication1.Models.InfoSongName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("JukeboxClientToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoomId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SongName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("JukeboxClientToken");

                    b.HasIndex("RoomId");

                    b.ToTable("InfoSongName");
                });

            modelBuilder.Entity("WebApplication1.Models.JukeboxClient", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Token");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("WebApplication1.Models.JukeboxHost", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BannerUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Token");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Hosts");
                });

            modelBuilder.Entity("WebApplication1.Models.PollOption", b =>
                {
                    b.Property<int>("PollId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PollId"), 1L, 1);

                    b.Property<int>("Place")
                        .HasColumnType("int");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SongName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Votes")
                        .HasColumnType("int");

                    b.HasKey("PollId");

                    b.HasIndex("RoomId");

                    b.ToTable("Poll");
                });

            modelBuilder.Entity("WebApplication1.Models.Room", b =>
                {
                    b.Property<string>("RoomId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPlaying")
                        .HasColumnType("bit");

                    b.Property<int>("OnlineUsers")
                        .HasColumnType("int");

                    b.Property<DateTime>("OpeningTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Position")
                        .HasColumnType("time");

                    b.Property<string>("SongName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoomId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("WebApplication1.Models.InfoHostname", b =>
                {
                    b.HasOne("WebApplication1.Models.JukeboxClient", null)
                        .WithMany("FavoriteHosts")
                        .HasForeignKey("JukeboxClientToken");
                });

            modelBuilder.Entity("WebApplication1.Models.InfoSongName", b =>
                {
                    b.HasOne("WebApplication1.Models.JukeboxClient", null)
                        .WithMany("FavoriteSongs")
                        .HasForeignKey("JukeboxClientToken");

                    b.HasOne("WebApplication1.Models.Room", null)
                        .WithMany("SongHistory")
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("WebApplication1.Models.PollOption", b =>
                {
                    b.HasOne("WebApplication1.Models.Room", null)
                        .WithMany("Poll")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication1.Models.JukeboxClient", b =>
                {
                    b.Navigation("FavoriteHosts");

                    b.Navigation("FavoriteSongs");
                });

            modelBuilder.Entity("WebApplication1.Models.Room", b =>
                {
                    b.Navigation("Poll");

                    b.Navigation("SongHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
