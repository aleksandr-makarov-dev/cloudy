using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloudy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddParentToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "items",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_ParentId",
                table: "items",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_items_ParentId",
                table: "items",
                column: "ParentId",
                principalTable: "items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_items_ParentId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_ParentId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "items");
        }
    }
}
