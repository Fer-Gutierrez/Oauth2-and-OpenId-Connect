using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marvin.IDP.Migrations
{
    public partial class AddEmailValidation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("02cbcda8-f9b2-4728-9124-c3f425c0c81f"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("0d58a219-e5be-402a-9341-266aba41acd0"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("29b102bd-9d80-4fbf-b17d-de23b9ffd0a7"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("44debfe4-aedc-48f5-adb6-4e5cf46737ae"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("53a74911-feb7-4583-99bc-b577278898d9"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("72f1f46b-000f-42d3-93eb-cb07de4156da"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("9bb67452-4b2c-42f2-952f-ea2610206784"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("ac27f985-3412-42d5-9ece-c803511dace8"));

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ConcurrencyStamp", "Type", "UserId", "Value" },
                values: new object[,]
                {
                    { new Guid("11ee4302-b7c7-4a96-adef-0f3652a3ddfd"), "f4ed3c31-0c2d-45d8-b000-a3e1c4b37233", "country", "f080e554-8ad7-450e-09a8-08dc271a66eb", "in" },
                    { new Guid("336b94c6-0163-4cdd-af6d-f77327f7e696"), "24020f93-f7de-4401-a413-5df386e48cd3", "role", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "PayingUser" },
                    { new Guid("3d60a747-369a-4f3d-b178-21d915791555"), "048909f7-e96f-4126-88b7-36585d4847a2", "role", "f080e554-8ad7-450e-09a8-08dc271a66eb", "FreeUser" },
                    { new Guid("4afa6636-3921-4854-9982-8c512a4db253"), "b89868c4-0613-43fa-b7e1-88cfd5312f07", "family_name", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "Flagg" },
                    { new Guid("73124f18-72f7-40b1-80fe-739e436efcbe"), "ce15ef92-b297-4601-bc61-b477a534c220", "family_name", "f080e554-8ad7-450e-09a8-08dc271a66eb", "Flagg" },
                    { new Guid("7ffa0c4f-651f-4587-b5b8-52df722dbf15"), "1f9f6392-cb0b-485c-ba5b-8b63763f756e", "given_name", "f080e554-8ad7-450e-09a8-08dc271a66eb", "David" },
                    { new Guid("afef4e4e-88ba-4669-bc69-b8f8557d51ec"), "f3dc0a4d-0a30-454c-a15c-f6e7e1a1a540", "country", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "be" },
                    { new Guid("c3af382a-f9cf-44cb-a2a1-f221945fd85f"), "c9906bfb-c0b2-4d7a-9a30-a37d5e784f21", "given_name", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "Emma" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "f080e554-8ad7-450e-09a8-08dc271a66eb",
                column: "ConcurrencyStamp",
                value: "1860972d-af02-47c7-82fb-01d3a863a703");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb",
                column: "ConcurrencyStamp",
                value: "dac10fd9-b2d8-4ad7-9e18-b46f364579fc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "UserClaims");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
