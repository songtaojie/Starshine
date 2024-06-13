using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hx.Test.Entity.Migrations
{
    public partial class InitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    UserName = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    PassWord = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    NickName = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreaterId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    Creater = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    LastModifyTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifier = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
                    LastModifierId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "Id", "CreateTime", "Creater", "CreaterId", "LastModifier", "LastModifierId", "LastModifyTime", "NickName", "PassWord", "UserName" },
                values: new object[] { "66227dc2-17fc-421a-9002-8f31d9431661", new DateTime(2021, 12, 13, 21, 17, 41, 701, DateTimeKind.Local).AddTicks(4279), "SuperAdmin", "SuperAdmin", "", "", null, "宋", "123456", "songtaojie" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
