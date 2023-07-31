using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixedKanjiDidntHaveKanjiNotecard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 7, 19, 1, 49, 2, 644, DateTimeKind.Local).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "毎",
                column: "LastTimeAccess",
                value: new DateTime(2023, 7, 19, 1, 49, 2, 644, DateTimeKind.Local).AddTicks(2490));

            migrationBuilder.InsertData(
                table: "ExtraKanjiInfos",
                columns: new[] { "TopicName", "JLPTLevel", "NewspaperRank" },
                values: new object[] { "毎", 5, 436 });

            migrationBuilder.UpdateData(
                table: "Sentences",
                keyColumn: "ItemQuestion",
                keyValue: "毎日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 7, 19, 1, 49, 2, 644, DateTimeKind.Local).AddTicks(2532));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExtraKanjiInfos",
                keyColumn: "TopicName",
                keyValue: "毎");

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 20, 33, 41, 99, DateTimeKind.Local).AddTicks(8586));

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "毎",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 20, 33, 41, 99, DateTimeKind.Local).AddTicks(8613));

            migrationBuilder.UpdateData(
                table: "Sentences",
                keyColumn: "ItemQuestion",
                keyValue: "毎日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 20, 33, 41, 99, DateTimeKind.Local).AddTicks(8678));
        }
    }
}
