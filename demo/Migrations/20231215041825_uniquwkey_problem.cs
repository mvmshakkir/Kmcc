using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Migrations
{
    public partial class uniquwkey_problem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_TermId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_UserId",
                table: "Payment");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_TermId",
                table: "Payment",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_TermId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_UserId",
                table: "Payment");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_TermId",
                table: "Payment",
                column: "TermId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
