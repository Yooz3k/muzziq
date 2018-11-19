using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Muzziq.Data.Migrations
{
    public partial class MatchesRecreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Room_RoomId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Room_RoomId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomName",
                table: "Matches",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Matches",
                newName: "StartDate");

            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Songs",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswerId",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentRoundNumber",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRoundsCount",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    MatchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Songs_MatchId",
                table: "Songs",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CorrectAnswerId",
                table: "Matches",
                column: "CorrectAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_MatchId",
                table: "Answer",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Answer_CorrectAnswerId",
                table: "Matches",
                column: "CorrectAnswerId",
                principalTable: "Answer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Rooms_RoomId",
                table: "Matches",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Rooms_RoomId",
                table: "Players",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Matches_MatchId",
                table: "Songs",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Answer_CorrectAnswerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Rooms_RoomId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Rooms_RoomId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Matches_MatchId",
                table: "Songs");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Songs_MatchId",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Matches_CorrectAnswerId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "CorrectAnswerId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "CurrentRoundNumber",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TotalRoundsCount",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Matches",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Matches",
                newName: "RoomName");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Room_RoomId",
                table: "Matches",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Room_RoomId",
                table: "Players",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
