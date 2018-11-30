using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBHelperSingle.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "DeviceProfile",
                schema: "public",
                columns: table => new
                {
                    Uid = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    DevState = table.Column<int>(nullable: false),
                    IdLoactionData = table.Column<Guid>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    TS = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceProfile", x => new { x.Uid, x.Type });
                });

            migrationBuilder.CreateTable(
                name: "TempLocations",
                schema: "public",
                columns: table => new
                {
                    RemoteHostName = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    UniqueId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    X = table.Column<decimal>(nullable: true),
                    Y = table.Column<decimal>(nullable: true),
                    Z = table.Column<decimal>(nullable: true),
                    EPSG = table.Column<int>(nullable: false),
                    ServiceName = table.Column<string>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    Duration = table.Column<decimal>(nullable: false),
                    CustomInterval = table.Column<int>(nullable: true),
                    CollectTime = table.Column<DateTime>(nullable: false),
                    SendTime = table.Column<DateTime>(nullable: true),
                    Speed = table.Column<double>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    Floor = table.Column<int>(nullable: true),
                    Direction = table.Column<int>(nullable: true),
                    RelativePosition1 = table.Column<int>(nullable: true),
                    RelativePosition2 = table.Column<int>(nullable: true),
                    RelativePosition3 = table.Column<int>(nullable: true),
                    OriginalData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempLocations_SendTime",
                schema: "public",
                table: "TempLocations",
                column: "SendTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceProfile",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TempLocations",
                schema: "public");
        }
    }
}
