﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(KanjiDbContext))]
    [Migration("20230719064902_FixedKanjiDidntHaveKanjiNotecard")]
    partial class FixedKanjiDidntHaveKanjiNotecard
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("DataLayer.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Japanese Vocab"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.ChapterNoteCard", b =>
                {
                    b.Property<string>("TopicName")
                        .HasColumnType("TEXT");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GradeLevel")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastTimeAccess")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("TopicDefinition")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TopicName");

                    b.HasIndex("CategoryId");

                    b.ToTable("Chapters");

                    b.HasData(
                        new
                        {
                            TopicName = "日",
                            CategoryId = 1,
                            GradeLevel = 1,
                            LastTimeAccess = new DateTime(2023, 7, 19, 1, 49, 2, 644, DateTimeKind.Local).AddTicks(2463),
                            TopicDefinition = "day, sun, Japan"
                        },
                        new
                        {
                            TopicName = "毎",
                            CategoryId = 1,
                            GradeLevel = 2,
                            LastTimeAccess = new DateTime(2023, 7, 19, 1, 49, 2, 644, DateTimeKind.Local).AddTicks(2490),
                            TopicDefinition = "every"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.ChapterNoteCardSentenceNoteCard", b =>
                {
                    b.Property<string>("ChapterNoteCardTopicName")
                        .HasColumnType("TEXT");

                    b.Property<string>("SentenceNoteCardItemQuestion")
                        .HasColumnType("TEXT");

                    b.HasKey("ChapterNoteCardTopicName", "SentenceNoteCardItemQuestion");

                    b.HasIndex("SentenceNoteCardItemQuestion");

                    b.ToTable("ChapterSentences");

                    b.HasData(
                        new
                        {
                            ChapterNoteCardTopicName = "毎",
                            SentenceNoteCardItemQuestion = "毎日"
                        },
                        new
                        {
                            ChapterNoteCardTopicName = "日",
                            SentenceNoteCardItemQuestion = "毎日"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.ExtraJishoInfoOnBridge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChapterNoteCardTopicName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PageNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SentenceNoteCardItemQuestion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChapterNoteCardTopicName", "SentenceNoteCardItemQuestion")
                        .IsUnique();

                    b.ToTable("ExtraJishoInfos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ChapterNoteCardTopicName = "毎",
                            Order = 1,
                            PageNumber = 1,
                            SentenceNoteCardItemQuestion = "毎日"
                        },
                        new
                        {
                            Id = 2,
                            ChapterNoteCardTopicName = "日",
                            Order = 1,
                            PageNumber = 1,
                            SentenceNoteCardItemQuestion = "毎日"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.JapaneseWordNoteCard", b =>
                {
                    b.Property<string>("ItemQuestion")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCommonWord")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("JLPTLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemQuestion");

                    b.ToTable("JapaneseWordNoteCards");

                    b.HasData(
                        new
                        {
                            ItemQuestion = "毎日",
                            IsCommonWord = true,
                            JLPTLevel = 5
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.KanjiNoteCard", b =>
                {
                    b.Property<string>("TopicName")
                        .HasColumnType("TEXT");

                    b.Property<int>("JLPTLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NewspaperRank")
                        .HasColumnType("INTEGER");

                    b.HasKey("TopicName");

                    b.ToTable("ExtraKanjiInfos");

                    b.HasData(
                        new
                        {
                            TopicName = "日",
                            JLPTLevel = 5,
                            NewspaperRank = 1
                        },
                        new
                        {
                            TopicName = "毎",
                            JLPTLevel = 5,
                            NewspaperRank = 436
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.KanjiReading", b =>
                {
                    b.Property<string>("KanjiNoteCardTopicName")
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeOfReading")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reading")
                        .HasColumnType("TEXT");

                    b.HasKey("KanjiNoteCardTopicName", "TypeOfReading", "Reading");

                    b.ToTable("KanjiReadings");

                    b.HasData(
                        new
                        {
                            KanjiNoteCardTopicName = "日",
                            TypeOfReading = "kun",
                            Reading = "ひ"
                        },
                        new
                        {
                            KanjiNoteCardTopicName = "日",
                            TypeOfReading = "kun",
                            Reading = "び"
                        },
                        new
                        {
                            KanjiNoteCardTopicName = "日",
                            TypeOfReading = "kun",
                            Reading = "か"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.SentenceNoteCard", b =>
                {
                    b.Property<string>("ItemQuestion")
                        .HasColumnType("TEXT");

                    b.Property<string>("Hint")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsUserWantsToFocusOn")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemAnswer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastTimeAccess")
                        .HasColumnType("TEXT");

                    b.Property<int>("MemorizationLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemQuestion");

                    b.ToTable("Sentences");

                    b.HasData(
                        new
                        {
                            ItemQuestion = "毎日",
                            Hint = "まい·にち",
                            IsUserWantsToFocusOn = false,
                            ItemAnswer = "every day​",
                            LastTimeAccess = new DateTime(2023, 7, 19, 1, 49, 2, 644, DateTimeKind.Local).AddTicks(2532),
                            MemorizationLevel = 0
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.ChapterNoteCard", b =>
                {
                    b.HasOne("DataLayer.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DataLayer.Entities.ChapterNoteCardSentenceNoteCard", b =>
                {
                    b.HasOne("DataLayer.Entities.ChapterNoteCard", null)
                        .WithMany("ChapterSentences")
                        .HasForeignKey("ChapterNoteCardTopicName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Entities.SentenceNoteCard", null)
                        .WithMany("ChapterSentences")
                        .HasForeignKey("SentenceNoteCardItemQuestion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLayer.Entities.ExtraJishoInfoOnBridge", b =>
                {
                    b.HasOne("DataLayer.Entities.ChapterNoteCardSentenceNoteCard", "ChapterNoteCardSentenceNoteCard")
                        .WithOne("ExtraJishoInfo")
                        .HasForeignKey("DataLayer.Entities.ExtraJishoInfoOnBridge", "ChapterNoteCardTopicName", "SentenceNoteCardItemQuestion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChapterNoteCardSentenceNoteCard");
                });

            modelBuilder.Entity("DataLayer.Entities.JapaneseWordNoteCard", b =>
                {
                    b.HasOne("DataLayer.Entities.SentenceNoteCard", "SentenceNoteCard")
                        .WithOne()
                        .HasForeignKey("DataLayer.Entities.JapaneseWordNoteCard", "ItemQuestion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SentenceNoteCard");
                });

            modelBuilder.Entity("DataLayer.Entities.KanjiNoteCard", b =>
                {
                    b.HasOne("DataLayer.Entities.ChapterNoteCard", "ChapterNoteCard")
                        .WithOne()
                        .HasForeignKey("DataLayer.Entities.KanjiNoteCard", "TopicName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChapterNoteCard");
                });

            modelBuilder.Entity("DataLayer.Entities.KanjiReading", b =>
                {
                    b.HasOne("DataLayer.Entities.KanjiNoteCard", null)
                        .WithMany("KanjiReadings")
                        .HasForeignKey("KanjiNoteCardTopicName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLayer.Entities.ChapterNoteCard", b =>
                {
                    b.Navigation("ChapterSentences");
                });

            modelBuilder.Entity("DataLayer.Entities.ChapterNoteCardSentenceNoteCard", b =>
                {
                    b.Navigation("ExtraJishoInfo");
                });

            modelBuilder.Entity("DataLayer.Entities.KanjiNoteCard", b =>
                {
                    b.Navigation("KanjiReadings");
                });

            modelBuilder.Entity("DataLayer.Entities.SentenceNoteCard", b =>
                {
                    b.Navigation("ChapterSentences");
                });
#pragma warning restore 612, 618
        }
    }
}
