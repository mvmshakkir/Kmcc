using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Migrations
{
    public partial class Type_to_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "regModelId",
                table: "Payment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_regModelId",
                table: "Payment",
                column: "regModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_regModel_regModelId",
                table: "Payment",
                column: "regModelId",
                principalTable: "regModel",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_regModel_regModelId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_regModelId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "regModelId",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
