using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThumbsApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Thumbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Pid = table.Column<string>(maxLength: 7, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<bool>(nullable: false),
                    Product = table.Column<string>(nullable: true),
                    Group = table.Column<string>(maxLength: 255, nullable: true),
                    EndPoint = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Thumbs");
        }
    }
}
