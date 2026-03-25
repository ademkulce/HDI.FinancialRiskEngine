using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HDI.FinancialRiskEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsActive", "IsDeleted", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1, "HDI", null, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), true, false, "HDI Default Tenant", null, null });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "Id", "BaseRiskRate", "Code", "CreatedBy", "CreatedDate", "Description", "EndDate", "IsActive", "IsDeleted", "Name", "StartDate", "TenantId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1, 1.50m, "AGR001", null, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Sistem başlangıcı için oluşturulmuş örnek agreement kaydı.", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "Genel Finansal Risk Sözleşmesi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, null });

            migrationBuilder.InsertData(
                table: "AgreementKeywords",
                columns: new[] { "Id", "AgreementId", "CreatedBy", "CreatedDate", "IsActive", "IsDeleted", "Keyword", "RiskScore", "TenantId", "UpdatedBy", "UpdatedDate", "Weight" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), true, false, "ceza", 20, 1, null, null, 1.00m },
                    { 2, 1, null, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), true, false, "iptal", 15, 1, null, null, 1.00m },
                    { 3, 1, null, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), true, false, "dava", 50, 1, null, null, 1.00m }
                });

            migrationBuilder.InsertData(
                table: "BusinessPartners",
                columns: new[] { "Id", "AgreementId", "ApiKey", "Code", "CreatedBy", "CreatedDate", "Email", "IsActive", "IsDeleted", "Name", "Phone", "TenantId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1, 1, "STATIC-SEED-API-KEY-001", "PRT001", null, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), "partner@test.com", true, false, "ABC Partner", "05551234567", 1, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AgreementKeywords",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AgreementKeywords",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AgreementKeywords",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BusinessPartners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Agreements",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
