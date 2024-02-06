using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marvin.IDP.Migrations
{
    public partial class AddEmailValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("45035b8a-729a-4cf2-b9ac-edfcfee16444"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("530491a9-d391-4142-8107-6ddbc896a01f"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("5cc1728e-a2cc-477a-9f38-b7b37fb77d4e"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("6340f38d-fa77-4bdf-a829-4c5601309054"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("a19e8dcb-d94a-465c-86c7-7526c3f8df61"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("abb90f71-b89c-4a78-9447-a2889b400dcc"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("e09f49ee-6986-49eb-896e-26c4e7cf0b0f"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("ef421dbb-4e06-4dd3-9675-38405a88b04f"));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityCode",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SecurityCodeExpirationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ConcurrencyStamp", "Type", "UserId", "Value" },
                values: new object[,]
                {
                    { new Guid("02cbcda8-f9b2-4728-9124-c3f425c0c81f"), "ebf7d281-2b24-4588-b035-d33f4cc90ae0", "country", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "be" },
                    { new Guid("0d58a219-e5be-402a-9341-266aba41acd0"), "eae8be63-98c5-463c-84e5-1eca07a0eefc", "role", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "PayingUser" },
                    { new Guid("29b102bd-9d80-4fbf-b17d-de23b9ffd0a7"), "1b8ef8d7-c81d-4632-82e9-eaab5c14e2d0", "given_name", "f080e554-8ad7-450e-09a8-08dc271a66eb", "David" },
                    { new Guid("44debfe4-aedc-48f5-adb6-4e5cf46737ae"), "6ad1ccf0-0b63-43dd-acb2-139f97631a19", "family_name", "f080e554-8ad7-450e-09a8-08dc271a66eb", "Flagg" },
                    { new Guid("53a74911-feb7-4583-99bc-b577278898d9"), "c3f5948b-b69d-46a8-9a3d-0008b677072e", "given_name", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "Emma" },
                    { new Guid("72f1f46b-000f-42d3-93eb-cb07de4156da"), "ca0c428b-2429-40b1-80c5-cdf8707d4f63", "role", "f080e554-8ad7-450e-09a8-08dc271a66eb", "FreeUser" },
                    { new Guid("9bb67452-4b2c-42f2-952f-ea2610206784"), "5bbfb311-b847-419f-9c0f-028db1e69226", "family_name", "9c1e2bf3-eeb5-4c32-09a7-08dc271a66eb", "Flagg" },
                    { new Guid("ac27f985-3412-42d5-9ece-c803511dace8"), "e2f7a391-96a4-4fc6-96d5-fb7deee92019", "country", "f080e554-8ad7-450e-09a8-08dc271a66eb", "nl" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                columns: new[] { "ConcurrencyStamp", "Email" },
                values: new object[] { "20dafaab-2471-4250-8f1d-3967cf73c2fc", "david@someprovider.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                columns: new[] { "ConcurrencyStamp", "Email" },
                values: new object[] { "d88526d1-9782-4404-b32b-18282bc0fb63", "emma@someprovider.com" });
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
