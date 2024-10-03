using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatyamHealthCare.Migrations
{
    /// <inheritdoc />
    public partial class casestudy7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionTest_Prescriptions_PrescriptionID",
                table: "PrescriptionTest",
                column: "PrescriptionID",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionID",
                onDelete: ReferentialAction.Cascade);

         
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionTest_Prescriptions_PrescriptionID",
                table: "PrescriptionTest");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionTest_Tests_TestID",
                table: "PrescriptionTest");

            migrationBuilder.CreateTable(
                name: "PrescribedTest",
                columns: table => new
                {
                    PrescriptionID = table.Column<int>(type: "int", nullable: true),
                    TestID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_PrescribedTest_Prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescribedTest_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
