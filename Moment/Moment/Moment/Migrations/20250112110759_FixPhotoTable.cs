﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moment.Migrations
{
    /// <inheritdoc />
    public partial class FixPhotoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Photos_PhotoId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PhotoId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Photos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PhotoId",
                table: "Photos",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Photos_PhotoId",
                table: "Photos",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }
    }
}
