using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eUniversityServer.DAL.Migrations
{
    public partial class FixedStudentDocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_EducationDocuments_EducationDocumentId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_IdentificationCodes_IdentificationCodeId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Passports_PassportId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EducationDocumentId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_IdentificationCodeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PassportId",
                table: "Students");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassportId",
                table: "Students",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "IdentificationCodeId",
                table: "Students",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "EducationDocumentId",
                table: "Students",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0b0fec5-8958-43ca-99b8-1978f198cf06"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 238, 73, 139, 236, 170, 237, 2, 183, 64, 176, 12, 167, 119, 104, 59, 173, 131, 22, 173, 81, 3, 26, 18, 19, 218, 55, 93, 21, 63, 233, 192, 142, 208, 247, 149, 87, 209, 81, 96, 63, 174, 178, 36, 18, 24, 176, 159, 55, 54, 91, 227, 195, 193, 208, 198, 255, 97, 139, 47, 196, 204, 47, 177, 144 }, new byte[] { 196, 178, 70, 110, 5, 24, 35, 134, 74, 61, 241, 53, 1, 221, 232, 186, 150, 162, 125, 228, 51, 7, 132, 255, 167, 173, 227, 254, 151, 118, 24, 26, 135, 92, 219, 198, 117, 127, 114, 61, 39, 246, 43, 117, 219, 121, 25, 231, 64, 91, 212, 49, 148, 191, 90, 140, 251, 50, 171, 248, 216, 93, 72, 182, 136, 133, 166, 178, 247, 38, 172, 76, 143, 82, 232, 242, 216, 75, 185, 31, 60, 187, 103, 187, 68, 241, 113, 77, 24, 183, 64, 64, 35, 10, 254, 244, 79, 25, 202, 92, 134, 45, 108, 6, 134, 179, 59, 21, 10, 171, 35, 236, 244, 237, 195, 52, 139, 201, 27, 10, 3, 130, 83, 196, 79, 163, 91, 104 } });

            migrationBuilder.CreateIndex(
                name: "IX_Students_EducationDocumentId",
                table: "Students",
                column: "EducationDocumentId",
                unique: true,
                filter: "[EducationDocumentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdentificationCodeId",
                table: "Students",
                column: "IdentificationCodeId",
                unique: true,
                filter: "[IdentificationCodeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_PassportId",
                table: "Students",
                column: "PassportId",
                unique: true,
                filter: "[PassportId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_EducationDocuments_EducationDocumentId",
                table: "Students",
                column: "EducationDocumentId",
                principalTable: "EducationDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_IdentificationCodes_IdentificationCodeId",
                table: "Students",
                column: "IdentificationCodeId",
                principalTable: "IdentificationCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Passports_PassportId",
                table: "Students",
                column: "PassportId",
                principalTable: "Passports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_EducationDocuments_EducationDocumentId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_IdentificationCodes_IdentificationCodeId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Passports_PassportId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EducationDocumentId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_IdentificationCodeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PassportId",
                table: "Students");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassportId",
                table: "Students",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdentificationCodeId",
                table: "Students",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EducationDocumentId",
                table: "Students",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0b0fec5-8958-43ca-99b8-1978f198cf06"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 89, 33, 171, 24, 175, 100, 4, 18, 135, 159, 201, 244, 109, 25, 111, 95, 55, 6, 204, 243, 71, 220, 125, 169, 197, 218, 207, 13, 6, 226, 200, 38, 115, 11, 128, 249, 118, 17, 149, 21, 29, 213, 64, 232, 238, 39, 238, 161, 210, 85, 241, 126, 29, 231, 84, 113, 148, 64, 50, 176, 129, 5, 112, 33 }, new byte[] { 254, 195, 179, 232, 139, 63, 110, 232, 152, 23, 132, 215, 28, 175, 142, 119, 12, 137, 45, 36, 231, 209, 44, 24, 245, 183, 125, 33, 7, 0, 119, 208, 4, 68, 203, 137, 69, 147, 100, 10, 100, 191, 249, 249, 205, 62, 227, 218, 159, 127, 219, 11, 20, 200, 102, 50, 236, 50, 48, 52, 115, 34, 89, 230, 36, 102, 242, 205, 163, 140, 107, 81, 232, 114, 251, 152, 241, 13, 81, 110, 12, 197, 11, 163, 39, 104, 186, 184, 181, 61, 130, 95, 72, 198, 16, 139, 104, 133, 32, 15, 126, 204, 169, 44, 248, 11, 129, 162, 180, 202, 126, 174, 129, 155, 86, 45, 139, 115, 170, 125, 227, 138, 52, 163, 132, 144, 89, 155 } });

            migrationBuilder.CreateIndex(
                name: "IX_Students_EducationDocumentId",
                table: "Students",
                column: "EducationDocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdentificationCodeId",
                table: "Students",
                column: "IdentificationCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_PassportId",
                table: "Students",
                column: "PassportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_EducationDocuments_EducationDocumentId",
                table: "Students",
                column: "EducationDocumentId",
                principalTable: "EducationDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_IdentificationCodes_IdentificationCodeId",
                table: "Students",
                column: "IdentificationCodeId",
                principalTable: "IdentificationCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Passports_PassportId",
                table: "Students",
                column: "PassportId",
                principalTable: "Passports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
