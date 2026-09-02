using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiscalZen.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "FiscalDocuments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FiscalDocuments_AccountId_AccessKey",
                table: "FiscalDocuments",
                newName: "IX_FiscalDocuments_UserId_AccessKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FiscalDocuments",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_FiscalDocuments_UserId_AccessKey",
                table: "FiscalDocuments",
                newName: "IX_FiscalDocuments_AccountId_AccessKey");
        }
    }
}
