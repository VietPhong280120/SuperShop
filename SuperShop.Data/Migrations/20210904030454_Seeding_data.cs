using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SuperShop.Data.Migrations
{
    public partial class Seeding_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "Key", "Value" },
                values: new object[,]
                {
                    { "HomeTitle", "This is home page SuperShop" },
                    { "KeyWord", "This is key word SuperShop" },
                    { "HomeDescription", "This is description SuperShop" }
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("400d7803-c8d3-4d9e-ac74-249a62370ea1"), "e1463ad8-1ec8-4742-b303-45ad23ede597", "Administrator", "admin", "admin" },
                    { new Guid("acde886f-8525-4ee2-bc8b-803b46ade679"), "f53b65ab-b5c3-4cf5-9f4f-a907fad6c022", "User", "user", "user" }
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("6c31bc35-90c6-443b-8a6b-2576ba12f19d"), 0, "a968db70-bef7-4993-b54d-c99b432c4472", new DateTime(2000, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietphong2801@gmail.com", true, "Phong", "Nguyen", false, null, "Vietphong2801@gmail.com", "admin", "AQAAAAEAACcQAAAAEN0ArgX5tB6COq+hOcARAyqLDzzViZGSO3OCCzJWXLKRdfxKzs89jqrUNDazUX7FMQ==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "IsDefault", "Name" },
                values: new object[,]
                {
                    { "vi", true, "Tiếng Việt" },
                    { "en", true, "English" }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("400d7803-c8d3-4d9e-ac74-249a62370ea1"), new Guid("6c31bc35-90c6-443b-8a6b-2576ba12f19d") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeDescription");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeTitle");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "KeyWord");

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("acde886f-8525-4ee2-bc8b-803b46ade679"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("400d7803-c8d3-4d9e-ac74-249a62370ea1"), new Guid("6c31bc35-90c6-443b-8a6b-2576ba12f19d") });

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "en");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "vi");

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("400d7803-c8d3-4d9e-ac74-249a62370ea1"));

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c31bc35-90c6-443b-8a6b-2576ba12f19d"));
        }
    }
}
