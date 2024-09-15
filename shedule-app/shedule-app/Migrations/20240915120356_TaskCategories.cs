using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shedule_app.Migrations
{
    /// <inheritdoc />
    public partial class TaskCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Category_CategoriesIdCategory",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CategoriesIdCategory",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoriesIdCategory",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "IdCategory");

            migrationBuilder.CreateTable(
                name: "TaskCategories",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategories", x => new { x.IdCategory, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskCategories_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCategories_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_TaskId",
                table: "TaskCategories",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "CategoriesIdCategory",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdCategory",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CategoriesIdCategory",
                table: "Tasks",
                column: "CategoriesIdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Category_CategoriesIdCategory",
                table: "Tasks",
                column: "CategoriesIdCategory",
                principalTable: "Category",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
