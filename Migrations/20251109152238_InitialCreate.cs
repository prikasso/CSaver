using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSaver.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tpep_pickup_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tpep_dropoff_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    passenger_count = table.Column<int>(type: "int", nullable: false),
                    trip_distance = table.Column<float>(type: "real", nullable: false),
                    store_and_fwd_flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PULocationID = table.Column<int>(type: "int", nullable: false),
                    DOLocationID = table.Column<int>(type: "int", nullable: false),
                    fare_amount = table.Column<float>(type: "real", nullable: false),
                    tip_amount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_tpep_pickup_datetime_tpep_dropoff_datetime_passenger_count",
                table: "Deliveries",
                columns: new[] { "tpep_pickup_datetime", "tpep_dropoff_datetime", "passenger_count" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliveries");
        }
    }
}
