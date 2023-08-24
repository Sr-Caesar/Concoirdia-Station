﻿// <auto-generated />
using System;
using ConcordiaStation.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConcordiaStation.Data.Migrations
{
    [DbContext(typeof(ConcordiaLocalDbContext))]
    [Migration("20230620103637_Add_Dat")]
    partial class Add_Dat
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConcordiaStation.Data.Models.Comment", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCommentTrello")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdPhaseTrello")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PhaseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ScientistId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PhaseId");

                    b.HasIndex("ScientistId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Credential", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ScientistId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ScientistId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Experiment", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("IdListTrello")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Phase", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ExperimentId")
                        .HasColumnType("int");

                    b.Property<string>("IdCardTrello")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastActivity")
                        .HasColumnType("datetime2");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId");

                    b.ToTable("Phases");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Scientist", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("FamilyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GivenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdTrello")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PhaseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PhaseId");

                    b.ToTable("Scientists");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Comment", b =>
                {
                    b.HasOne("ConcordiaStation.Data.Models.Phase", "Phase")
                        .WithMany("Comments")
                        .HasForeignKey("PhaseId");

                    b.HasOne("ConcordiaStation.Data.Models.Scientist", "Scientist")
                        .WithMany()
                        .HasForeignKey("ScientistId");

                    b.Navigation("Phase");

                    b.Navigation("Scientist");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Credential", b =>
                {
                    b.HasOne("ConcordiaStation.Data.Models.Scientist", "Scientist")
                        .WithMany()
                        .HasForeignKey("ScientistId");

                    b.Navigation("Scientist");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Phase", b =>
                {
                    b.HasOne("ConcordiaStation.Data.Models.Experiment", "Experiment")
                        .WithMany("Phases")
                        .HasForeignKey("ExperimentId");

                    b.Navigation("Experiment");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Scientist", b =>
                {
                    b.HasOne("ConcordiaStation.Data.Models.Phase", "Phase")
                        .WithMany("Scientists")
                        .HasForeignKey("PhaseId");

                    b.Navigation("Phase");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Experiment", b =>
                {
                    b.Navigation("Phases");
                });

            modelBuilder.Entity("ConcordiaStation.Data.Models.Phase", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Scientists");
                });
#pragma warning restore 612, 618
        }
    }
}
