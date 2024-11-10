using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicQueueDataBaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class cabinetNum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CabinetNumber",
                table: "Doctors",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CabinetNumber",
                table: "Doctors",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
