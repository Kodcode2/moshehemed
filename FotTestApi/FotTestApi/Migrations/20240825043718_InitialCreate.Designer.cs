﻿// <auto-generated />
using FotTestApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FotTestApi.Migrations
{
    [DbContext(typeof(ApplicationBDContext))]
    [Migration("20240825043718_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FotTestApi.Models.AgentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Location_x")
                        .HasColumnType("int");

                    b.Property<int>("Location_y")
                        .HasColumnType("int");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("FotTestApi.Models.MissionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActualExecutionTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdAgent")
                        .HasColumnType("int");

                    b.Property<int>("IdTarget")
                        .HasColumnType("int");

                    b.Property<string>("RemainingTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdAgent");

                    b.HasIndex("IdTarget");

                    b.ToTable("Missions");
                });

            modelBuilder.Entity("FotTestApi.Models.TargetModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Location_x")
                        .HasColumnType("int");

                    b.Property<int>("Location_y")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("FotTestApi.Models.MissionModel", b =>
                {
                    b.HasOne("FotTestApi.Models.AgentModel", "Agent")
                        .WithMany("Missions")
                        .HasForeignKey("IdAgent")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FotTestApi.Models.TargetModel", "Target")
                        .WithMany()
                        .HasForeignKey("IdTarget")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agent");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("FotTestApi.Models.AgentModel", b =>
                {
                    b.Navigation("Missions");
                });
#pragma warning restore 612, 618
        }
    }
}
