using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addTextbooksTableAndUpdateSubjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            

           

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Textbooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    CoursesForEachLevel = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Textbooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Textbooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Textbooks_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_SubjectId",
                table: "Textbooks",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_UserId",
                table: "Textbooks",
                column: "UserId");

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffDocuments_Subjects_SubjectId",
                table: "StaffDocuments");

            migrationBuilder.DropTable(
                name: "Textbooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Subjects");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffDocuments_Subject_SubjectId",
                table: "StaffDocuments",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }
    }
}
