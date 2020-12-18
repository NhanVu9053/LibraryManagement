using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryManagement.API.Migrations
{
    public partial class EditNameColUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "AspNetUsers",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "AspNetUsers",
                newName: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AspNetUsers",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "AspNetUsers",
                newName: "CreateBy");
        }
    }
}
