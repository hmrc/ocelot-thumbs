using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThumbsApi.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deletions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Pid = table.Column<string>(unicode: false, fixedLength: true, maxLength: 7, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<bool>(nullable: false),
                    Product = table.Column<string>(maxLength: 100, nullable: false),
                    Group = table.Column<string>(maxLength: 100, nullable: false),
                    EndPoint = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(unicode: false, fixedLength: true, maxLength: 7, nullable: false),
                    DeletedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deletions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thumbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Pid = table.Column<string>(unicode: false, fixedLength: true, maxLength: 7, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<bool>(nullable: false),
                    Product = table.Column<string>(maxLength: 100, nullable: false),
                    Group = table.Column<string>(maxLength: 100, nullable: false),
                    EndPoint = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deletions");

            migrationBuilder.DropTable(
                name: "Thumbs");
        }
    }
}
