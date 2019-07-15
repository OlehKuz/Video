using Microsoft.EntityFrameworkCore.Migrations;

namespace VOD.Database.Migrations
{
    public partial class added_foreignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Videos",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Videos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Downloads",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Downloads",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CourseId",
                table: "Videos",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Downloads_CourseId",
                table: "Downloads",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Downloads_Courses_CourseId",
                table: "Downloads",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Courses_CourseId",
                table: "Videos",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Downloads_Courses_CourseId",
                table: "Downloads");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Courses_CourseId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_CourseId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Downloads_CourseId",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Downloads");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Videos",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Downloads",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
