using Microsoft.EntityFrameworkCore.Migrations;

namespace ResApi.Migrations
{
    public partial class changedPhotoLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "CategoryMenu",
                type: "varchar(400)",
                unicode: false,
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "CategoryMenu",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(400)",
                oldUnicode: false,
                oldMaxLength: 400,
                oldNullable: true);
        }
    }
}
