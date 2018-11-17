using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Muzziq.Data.Migrations
{
    public partial class EditedMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Matches_MatchId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_MatchId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "ModeratorId",
                table: "Matches",
                newName: "WinnerId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Room",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Room",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RoomId",
                table: "Matches",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Room_RoomId",
                table: "Matches",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Room_RoomId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_RoomId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "Matches",
                newName: "ModeratorId");

            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Room",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room_MatchId",
                table: "Room",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Matches_MatchId",
                table: "Room",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
