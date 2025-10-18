using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.SqlEfCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegularUrls",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UrlString = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    NormalizedUrlString = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegularUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortUrls",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "BLOB", nullable: false),
                    RegularUrlId = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ShortUrlCode = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastClickedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Alias = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    NormalizedAlias = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ClickCount = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortUrls_RegularUrls_RegularUrlId",
                        column: x => x.RegularUrlId,
                        principalTable: "RegularUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegularUrls_NormalizedUrlString",
                table: "RegularUrls",
                column: "NormalizedUrlString",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_NormalizedAlias",
                table: "ShortUrls",
                column: "NormalizedAlias",
                unique: true,
                filter: "\"NormalizedAlias\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_RegularUrlId",
                table: "ShortUrls",
                column: "RegularUrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortUrls");

            migrationBuilder.DropTable(
                name: "RegularUrls");
        }
    }
}
