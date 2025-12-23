using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsRelationToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id");
        }
    }
}
