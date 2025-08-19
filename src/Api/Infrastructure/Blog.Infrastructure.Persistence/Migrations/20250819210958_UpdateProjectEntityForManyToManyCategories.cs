using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectEntityForManyToManyCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_categories_CategoryId",
                schema: "dbo",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId",
                schema: "dbo",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "dbo",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "project_categories",
                schema: "dbo",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_categories", x => new { x.ProjectId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_project_categories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_project_categories_CategoryId",
                schema: "dbo",
                table: "project_categories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_project_categories_ProjectId",
                schema: "dbo",
                table: "project_categories",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project_categories",
                schema: "dbo");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "dbo",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                schema: "dbo",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_categories_CategoryId",
                schema: "dbo",
                table: "Projects",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "categories",
                principalColumn: "ID");
        }
    }
}
