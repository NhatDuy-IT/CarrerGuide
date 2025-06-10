using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyAspNetCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEstablishedYearToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add temporary column
            migrationBuilder.AddColumn<int>(
                name: "EstablishedYearTemp",
                table: "Schools",
                type: "int",
                nullable: false,
                defaultValue: 2000);

            // Update data: extract year from datetime
            migrationBuilder.Sql("UPDATE Schools SET EstablishedYearTemp = YEAR(EstablishedYear)");

            // Drop old column
            migrationBuilder.DropColumn(
                name: "EstablishedYear",
                table: "Schools");

            // Rename temp column
            migrationBuilder.RenameColumn(
                name: "EstablishedYearTemp",
                table: "Schools",
                newName: "EstablishedYear");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: "f'(x) = 2x + 3");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: "f'(x) = x² + 3");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: "f'(x) = 2x - 2");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4,
                column: "Text",
                value: "f'(x) = 2x + 5");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5,
                column: "Text",
                value: "x = 4");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 6,
                column: "Text",
                value: "x = 3");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 7,
                column: "Text",
                value: "x = 5");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 8,
                column: "Text",
                value: "x = 6");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 9,
                column: "Text",
                value: "6");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 10,
                column: "Text",
                value: "4");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 11,
                column: "Text",
                value: "8");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 12,
                column: "Text",
                value: "5");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 13,
                column: "Text",
                value: "go");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 14,
                column: "Text",
                value: "goes");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 15,
                column: "Text",
                value: "going");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 16,
                column: "Text",
                value: "went");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 17,
                column: "Text",
                value: "went");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 18,
                column: "Text",
                value: "go");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 19,
                column: "Text",
                value: "goes");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 20,
                column: "Text",
                value: "going");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 21,
                column: "Text",
                value: "v = s/t");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 22,
                column: "Text",
                value: "v = s*t");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 23,
                column: "Text",
                value: "v = t/s");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 24,
                column: "Text",
                value: "v = s + t");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Answers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstablishedYear",
                table: "Schools",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Description", "EstablishedYear", "ImageUrl", "Name", "PhoneNumber", "TuitionFee", "Website" },
                values: new object[,]
                {
                    { 1, "475A Điện Biên Phủ, Phường 25, Quận Bình Thạnh, TP.HCM", "Trường đại học công nghệ hàng đầu tại TP.HCM với nhiều ngành học hot", new DateTime(1995, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/image/HUTECH2.jpg", "ĐẠI HỌC CÔNG NGHỆ TP.HCM (HUTECH)", "028-5445-7777", 25000000m, "https://www.hutech.edu.vn" },
                    { 2, "10-12 Đinh Tiên Hoàng, Phường Bến Nghé, Quận 1, TP.HCM", "Trường đại học chuyên về khoa học xã hội và nhân văn", new DateTime(1957, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/image/nhanvan.png", "ĐẠI HỌC KHOA HỌC XÃ HỘI VÀ NHÂN VĂN", "028-3822-4271", 20000000m, "https://www.ussh.edu.vn" },
                    { 3, "268 Lý Thường Kiệt, Phường 14, Quận 10, TP.HCM", "Trường đại học kỹ thuật hàng đầu Việt Nam", new DateTime(1957, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/image/bachkhoa.jpg", "ĐẠI HỌC BÁCH KHOA TP.HCM", "028-3865-4321", 30000000m, "https://www.hcmut.edu.vn" }
                });
        }
    }
}
