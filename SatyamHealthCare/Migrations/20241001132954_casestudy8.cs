using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatyamHealthCare.Migrations
{
    /// <inheritdoc />
    public partial class casestudy8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultationDateTime",
                table: "MedicalRecords");

            migrationBuilder.AddColumn<int>(
                name: "MedicalHistoryId",
                table: "MedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_MedicalHistoryId",
                table: "MedicalRecords",
                column: "MedicalHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_MedicalHistoryFiles_MedicalHistoryId",
                table: "MedicalRecords",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistoryFiles",
                principalColumn: "MedicalHistoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_MedicalHistoryFiles_MedicalHistoryId",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_MedicalHistoryId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "MedicalHistoryId",
                table: "MedicalRecords");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsultationDateTime",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
