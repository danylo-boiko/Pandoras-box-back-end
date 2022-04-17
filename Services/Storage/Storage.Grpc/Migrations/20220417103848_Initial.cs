using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Storage.Core.Database;
using Storage.Core.Enums;

#nullable disable

namespace Storage.Grpc.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StorageItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersStorageItems",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StorageItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersStorageItems", x => new { x.UserId, x.StorageItemId });
                    table.ForeignKey(
                        name: "FK_UsersStorageItems_StorageItems_StorageItemId",
                        column: x => x.StorageItemId,
                        principalTable: "StorageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersStorageItems_StorageItemId",
                table: "UsersStorageItems",
                column: "StorageItemId");

            var categories = Enum.GetNames<FileCategory>();
            var categoriesBuilder = new StringBuilder();
            for (var i = 0; i < categories.Length; i++)
            {
                if (i + 1 == categories.Length)
                {
                    categoriesBuilder.Append($"'{categories[i]}'");
                }
                else
                {
                    categoriesBuilder.Append($"'{categories[i]}', ");
                }
            }
            migrationBuilder.Sql($"ALTER TABLE {nameof(StorageDbContext.StorageItems)} ADD CONSTRAINT CK_{nameof(StorageDbContext.StorageItems)}_Category CHECK(Category IN ({categoriesBuilder}));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersStorageItems");

            migrationBuilder.DropTable(
                name: "StorageItems");
        }
    }
}
