using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatyamHealthCare.Migrations
{
    /// <inheritdoc />
    public partial class casestudy2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Medicines_MedicineID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Tests_TestID",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_MedicineID",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_TestID",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "MedicineID",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "TestID",
                table: "Prescriptions");

            migrationBuilder.CreateTable(
                name: "PrescriptionMedicine",
                columns: table => new
                {
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    MedicineID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedicine", x => new { x.PrescriptionID, x.MedicineID });
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicine_Medicines_MedicineID",
                        column: x => x.MedicineID,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicine_Prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionTest",
                columns: table => new
                {
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    TestID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionTest", x => new { x.PrescriptionID, x.TestID });
                    table.ForeignKey(
                        name: "FK_PrescriptionTest_Prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionTest_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicine_MedicineID",
                table: "PrescriptionMedicine",
                column: "MedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionTest_TestID",
                table: "PrescriptionTest",
                column: "TestID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionMedicine");

            migrationBuilder.DropTable(
                name: "PrescriptionTest");

            migrationBuilder.AddColumn<int>(
                name: "MedicineID",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TestID",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicineID",
                table: "Prescriptions",
                column: "MedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_TestID",
                table: "Prescriptions",
                column: "TestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Medicines_MedicineID",
                table: "Prescriptions",
                column: "MedicineID",
                principalTable: "Medicines",
                principalColumn: "MedicineID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Tests_TestID",
                table: "Prescriptions",
                column: "TestID",
                principalTable: "Tests",
                principalColumn: "TestID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
