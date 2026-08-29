using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiscalZen.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToFiscalDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FiscalDocuments_AccessKey",
                table: "FiscalDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "FiscalDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FiscalDocuments_AccountId_AccessKey",
                table: "FiscalDocuments",
                columns: new[] { "AccountId", "AccessKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FiscalDocuments_AccountId_AccessKey",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "FiscalDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalDocuments_AccessKey",
                table: "FiscalDocuments",
                column: "AccessKey");
        }
    }
}
