using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BikeScanner.DAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Published = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.ID);
                    table.UniqueConstraint("AK_Contents_Url", x => x.Url);
                });

            migrationBuilder.CreateTable(
                name: "DevMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevMessages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JobExecutionInfo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExecutionInfo", x => x.ID);
                    table.UniqueConstraint("AK_JobExecutionInfo_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "NotificationsQueue",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsQueue", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SearchQuery = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.UniqueConstraint("AK_Users_UserId", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Published",
                table: "Contents",
                column: "Published");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_State",
                table: "Contents",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Text",
                table: "Contents",
                column: "Text")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "russian");

            migrationBuilder.CreateIndex(
                name: "IX_DevMessages_State",
                table: "DevMessages",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsQueue_State",
                table: "NotificationsQueue",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_State",
                table: "Subscriptions",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_State",
                table: "Users",
                column: "State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "DevMessages");

            migrationBuilder.DropTable(
                name: "JobExecutionInfo");

            migrationBuilder.DropTable(
                name: "NotificationsQueue");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
