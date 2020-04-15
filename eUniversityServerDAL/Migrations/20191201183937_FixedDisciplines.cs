using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eUniversityServer.DAL.Migrations
{
    public partial class FixedDisciplines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AssistantId",
                table: "AcademicDisciplines",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0b0fec5-8958-43ca-99b8-1978f198cf06"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 86, 31, 101, 232, 195, 74, 13, 64, 138, 74, 157, 218, 92, 209, 23, 10, 138, 53, 128, 200, 52, 112, 91, 84, 172, 82, 18, 122, 165, 207, 137, 153, 27, 187, 225, 136, 137, 3, 151, 87, 35, 216, 45, 243, 154, 209, 151, 157, 216, 243, 93, 201, 2, 205, 85, 46, 31, 66, 208, 135, 181, 4, 177, 126 }, new byte[] { 156, 221, 151, 247, 221, 46, 153, 166, 139, 65, 114, 60, 56, 89, 25, 29, 142, 16, 112, 109, 222, 241, 33, 21, 16, 43, 29, 165, 99, 162, 159, 184, 241, 151, 205, 213, 65, 155, 84, 208, 198, 115, 121, 2, 148, 7, 115, 108, 194, 42, 197, 246, 103, 16, 56, 193, 30, 4, 2, 73, 234, 193, 29, 187, 150, 200, 254, 18, 69, 136, 185, 55, 250, 236, 96, 164, 248, 184, 139, 106, 56, 28, 65, 214, 8, 212, 163, 2, 172, 23, 250, 74, 208, 113, 123, 73, 80, 44, 113, 118, 77, 210, 159, 208, 82, 137, 250, 11, 28, 206, 115, 72, 64, 112, 149, 105, 15, 224, 209, 222, 11, 131, 178, 52, 226, 99, 245, 134 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AssistantId",
                table: "AcademicDisciplines",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0b0fec5-8958-43ca-99b8-1978f198cf06"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 238, 73, 139, 236, 170, 237, 2, 183, 64, 176, 12, 167, 119, 104, 59, 173, 131, 22, 173, 81, 3, 26, 18, 19, 218, 55, 93, 21, 63, 233, 192, 142, 208, 247, 149, 87, 209, 81, 96, 63, 174, 178, 36, 18, 24, 176, 159, 55, 54, 91, 227, 195, 193, 208, 198, 255, 97, 139, 47, 196, 204, 47, 177, 144 }, new byte[] { 196, 178, 70, 110, 5, 24, 35, 134, 74, 61, 241, 53, 1, 221, 232, 186, 150, 162, 125, 228, 51, 7, 132, 255, 167, 173, 227, 254, 151, 118, 24, 26, 135, 92, 219, 198, 117, 127, 114, 61, 39, 246, 43, 117, 219, 121, 25, 231, 64, 91, 212, 49, 148, 191, 90, 140, 251, 50, 171, 248, 216, 93, 72, 182, 136, 133, 166, 178, 247, 38, 172, 76, 143, 82, 232, 242, 216, 75, 185, 31, 60, 187, 103, 187, 68, 241, 113, 77, 24, 183, 64, 64, 35, 10, 254, 244, 79, 25, 202, 92, 134, 45, 108, 6, 134, 179, 59, 21, 10, 171, 35, 236, 244, 237, 195, 52, 139, 201, 27, 10, 3, 130, 83, 196, 79, 163, 91, 104 } });
        }
    }
}
