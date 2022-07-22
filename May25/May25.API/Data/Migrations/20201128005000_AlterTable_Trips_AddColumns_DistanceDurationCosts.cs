using Microsoft.EntityFrameworkCore.Migrations;

namespace May25.API.Data.Migrations
{
    public partial class AlterTable_Trips_AddColumns_DistanceDurationCosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Trips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostPerPassenger",
                table: "Trips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "Trips",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Trips",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuggestedCost",
                table: "Trips",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CostPerPassenger",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "SuggestedCost",
                table: "Trips");
        }
    }
}
