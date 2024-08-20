using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "deliveryman",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TaxPayerId = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LicenseNumber = table.Column<string>(type: "text", nullable: false),
                    LicenseType = table.Column<int>(type: "integer", nullable: false),
                    LicenseImagePath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveryman", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "motorcycle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<long>(type: "bigint", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    LicensePlate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rentplan",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Days = table.Column<long>(type: "bigint", nullable: false),
                    DailyPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    EarlyDeliveryTaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    LateDeliveryRate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentplan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndPrevision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeliveryManId = table.Column<long>(type: "bigint", nullable: false),
                    MotorcycleId = table.Column<long>(type: "bigint", nullable: false),
                    RentPlanId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rent_deliveryman_DeliveryManId",
                        column: x => x.DeliveryManId,
                        principalTable: "deliveryman",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rent_motorcycle_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "motorcycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rent_rentplan_RentPlanId",
                        column: x => x.RentPlanId,
                        principalTable: "rentplan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliveryman_LicenseNumber",
                table: "deliveryman",
                column: "LicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deliveryman_TaxPayerId",
                table: "deliveryman",
                column: "TaxPayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_motorcycle_LicensePlate",
                table: "motorcycle",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rent_DeliveryManId",
                table: "rent",
                column: "DeliveryManId");

            migrationBuilder.CreateIndex(
                name: "IX_rent_MotorcycleId",
                table: "rent",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_rent_RentPlanId",
                table: "rent",
                column: "RentPlanId");

            migrationBuilder.Sql(
                @"INSERT INTO rentplan (""Days"", ""DailyPrice"", ""EarlyDeliveryTaxRate"", ""LateDeliveryRate"")
                    VALUES
                        (7, 30.00, 0.20, 50.00),
                        (15, 28.00, 0.40, 50.00),
                        (30, 22.00, 0.40, 50.00),
                        (45, 20.00, 0.40, 50.00),
                        (50, 18.00, 0.40, 50.00);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rent");

            migrationBuilder.DropTable(
                name: "deliveryman");

            migrationBuilder.DropTable(
                name: "motorcycle");

            migrationBuilder.DropTable(
                name: "rentplan");
        }
    }
}
