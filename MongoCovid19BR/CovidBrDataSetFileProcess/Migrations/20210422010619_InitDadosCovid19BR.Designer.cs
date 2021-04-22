﻿// <auto-generated />
using System;
using CursoMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CovidBrDataSetFileProcess.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20210422010619_InitDadosCovid19BR")]
    partial class InitDadosCovid19BR
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("CovidBrDataSetFileProcess.Model.DadosCovid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("city")
                        .HasColumnType("TEXT");

                    b.Property<string>("city_ibge_code")
                        .HasColumnType("TEXT");

                    b.Property<long>("city_ibglast_available_confirmede_code")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("date")
                        .HasColumnType("TEXT");

                    b.Property<string>("epidemiological_week")
                        .HasColumnType("TEXT");

                    b.Property<long>("estimated_population")
                        .HasColumnType("INTEGER");

                    b.Property<long>("estimated_population_2019")
                        .HasColumnType("INTEGER");

                    b.Property<string>("is_last")
                        .HasColumnType("TEXT");

                    b.Property<string>("is_repeated")
                        .HasColumnType("TEXT");

                    b.Property<double>("last_available_confirmed_per_100k_inhabitants")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("last_available_date")
                        .HasColumnType("TEXT");

                    b.Property<double>("last_available_death_rate")
                        .HasColumnType("REAL");

                    b.Property<long>("last_available_deaths")
                        .HasColumnType("INTEGER");

                    b.Property<long>("new_confirmed")
                        .HasColumnType("INTEGER");

                    b.Property<long>("new_deaths")
                        .HasColumnType("INTEGER");

                    b.Property<long>("order_for_place")
                        .HasColumnType("INTEGER");

                    b.Property<string>("place_type")
                        .HasColumnType("TEXT");

                    b.Property<string>("state")
                        .HasColumnType("TEXT");

                    b.Property<string>("uId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OsDadosDoCovid");
                });
#pragma warning restore 612, 618
        }
    }
}
