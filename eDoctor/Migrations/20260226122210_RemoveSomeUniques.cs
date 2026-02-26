using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eDoctor.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSomeUniques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_DoctorId_UserId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_DetailPrescriptions_DrugName",
                table: "DetailPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_DetailInvoices_ServiceName",
                table: "DetailInvoices");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DoctorId_UserId",
                table: "Schedules",
                columns: new[] { "DoctorId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_DoctorId_UserId",
                table: "Schedules");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DoctorId_UserId",
                table: "Schedules",
                columns: new[] { "DoctorId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DetailPrescriptions_DrugName",
                table: "DetailPrescriptions",
                column: "DrugName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetailInvoices_ServiceName",
                table: "DetailInvoices",
                column: "ServiceName",
                unique: true);
        }
    }
}
