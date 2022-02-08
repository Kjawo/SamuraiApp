using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class mantomanysimple : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Samurais_SamuraiId",
                table: "Quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes");

            migrationBuilder.RenameTable(
                name: "Quotes",
                newName: "Quote");

            migrationBuilder.RenameIndex(
                name: "IX_Quotes_SamuraiId",
                table: "Quote",
                newName: "IX_Quote_SamuraiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quote",
                table: "Quote",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Battle",
                columns: table => new
                {
                    BattleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battle", x => x.BattleID);
                });

            migrationBuilder.CreateTable(
                name: "BattleSamurai",
                columns: table => new
                {
                    BattlesBattleID = table.Column<int>(type: "int", nullable: false),
                    SamuraisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleSamurai", x => new { x.BattlesBattleID, x.SamuraisId });
                    table.ForeignKey(
                        name: "FK_BattleSamurai_Battle_BattlesBattleID",
                        column: x => x.BattlesBattleID,
                        principalTable: "Battle",
                        principalColumn: "BattleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleSamurai_Samurais_SamuraisId",
                        column: x => x.SamuraisId,
                        principalTable: "Samurais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleSamurai_SamuraisId",
                table: "BattleSamurai",
                column: "SamuraisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quote_Samurais_SamuraiId",
                table: "Quote",
                column: "SamuraiId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quote_Samurais_SamuraiId",
                table: "Quote");

            migrationBuilder.DropTable(
                name: "BattleSamurai");

            migrationBuilder.DropTable(
                name: "Battle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quote",
                table: "Quote");

            migrationBuilder.RenameTable(
                name: "Quote",
                newName: "Quotes");

            migrationBuilder.RenameIndex(
                name: "IX_Quote_SamuraiId",
                table: "Quotes",
                newName: "IX_Quotes_SamuraiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Samurais_SamuraiId",
                table: "Quotes",
                column: "SamuraiId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
