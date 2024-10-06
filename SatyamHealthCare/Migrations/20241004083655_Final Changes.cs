using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatyamHealthCare.Migrations
{
    /// <inheritdoc />
    public partial class FinalChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_MedicalHistoryFiles_MedicalHistoryId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "BeforeAfterFood",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "NoOfDays",
                table: "Prescriptions");

            migrationBuilder.AddColumn<string>(
                name: "BeforeAfterFood",
                table: "PrescriptionMedicine",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "PrescriptionMedicine",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DosageFrequency",
                table: "PrescriptionMedicine",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DosageUnit",
                table: "PrescriptionMedicine",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NoOfDays",
                table: "PrescriptionMedicine",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.DropColumn(
                name: "BeforeAfterFood",
                table: "PrescriptionMedicine");

            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "PrescriptionMedicine");

            migrationBuilder.DropColumn(
                name: "DosageFrequency",
                table: "PrescriptionMedicine");

            migrationBuilder.DropColumn(
                name: "DosageUnit",
                table: "PrescriptionMedicine");

            migrationBuilder.DropColumn(
                name: "NoOfDays",
                table: "PrescriptionMedicine");

            migrationBuilder.AddColumn<string>(
                name: "BeforeAfterFood",
                table: "Prescriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "Prescriptions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NoOfDays",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_MedicalHistoryFiles_MedicalHistoryId",
                table: "MedicalRecords",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistoryFiles",
                principalColumn: "MedicalHistoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
