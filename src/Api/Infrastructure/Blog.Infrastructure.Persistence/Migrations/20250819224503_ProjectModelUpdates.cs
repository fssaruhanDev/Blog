using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectModelUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "dbo",
                table: "tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "dbo",
                table: "tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "dbo",
                table: "tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                schema: "dbo",
                table: "tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                schema: "dbo",
                table: "categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "dbo",
                table: "categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "dbo",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "dbo",
                table: "categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                schema: "dbo",
                table: "categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                schema: "dbo",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "dbo",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "dbo",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dbo",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                schema: "dbo",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "Color",
                schema: "dbo",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "dbo",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "dbo",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dbo",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                schema: "dbo",
                table: "categories");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                schema: "dbo",
                table: "categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
