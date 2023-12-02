using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject_GymManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMemberSubscriptionTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberSubscriptions_Subscriptions_SubscriptionsID",
                table: "MemberSubscriptions");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_MemberSubscriptions_SubscriptionsID",
                table: "MemberSubscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionsID",
                table: "MemberSubscriptions");

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfMonths = table.Column<int>(type: "int", nullable: false),
                    WeekFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalNumberOfSessions = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_SubscriptionID",
                table: "MemberSubscriptions",
                column: "SubscriptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberSubscriptions_Subscription_SubscriptionID",
                table: "MemberSubscriptions",
                column: "SubscriptionID",
                principalTable: "Subscription",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberSubscriptions_Subscription_SubscriptionID",
                table: "MemberSubscriptions");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_MemberSubscriptions_SubscriptionID",
                table: "MemberSubscriptions");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionsID",
                table: "MemberSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfMonths = table.Column<int>(type: "int", nullable: false),
                    TotalNumberOfSessions = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeekFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_SubscriptionsID",
                table: "MemberSubscriptions",
                column: "SubscriptionsID");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberSubscriptions_Subscriptions_SubscriptionsID",
                table: "MemberSubscriptions",
                column: "SubscriptionsID",
                principalTable: "Subscriptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
