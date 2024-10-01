﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatyamHealthCare.Migrations
{
    /// <inheritdoc />
    public partial class casestudy4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfFilePath",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfFilePath",
                table: "Prescriptions");
        }
    }
}
