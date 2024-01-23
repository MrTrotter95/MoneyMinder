using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMinderWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingAutomaticPaymentLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AccountT__3214EC27522B8DBA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentFrequency",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentF__3214EC279C0A526C", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCategory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__3214EC274C7960D3", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__3214EC272AEE813A", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_AccountTypeID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    HouseSavingsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3214EC27BA9D8EFB", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Account_AccountTypeID",
                        column: x => x.FK_AccountTypeID,
                        principalTable: "AccountType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomaticPayment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_AccountID = table.Column<int>(type: "int", nullable: true),
                    FK_FrequencyID = table.Column<int>(type: "int", nullable: true),
                    FK_CategoryID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Automati__3214EC278300C151", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Automatic_Payment_AccountID",
                        column: x => x.FK_AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Automatic_Payment_CategoryID",
                        column: x => x.FK_CategoryID,
                        principalTable: "TransactionCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Automatic_Payment_FrequencyID",
                        column: x => x.FK_FrequencyID,
                        principalTable: "PaymentFrequency",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseSaving",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_AccountID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    GoalDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    GoalAmount = table.Column<decimal>(type: "decimal(8,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HouseSav__3214EC273F4DCF4A", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HouseSaving_AccountID",
                        column: x => x.FK_AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_AccountID = table.Column<int>(type: "int", nullable: true),
                    FK_TransactionTypeID = table.Column<int>(type: "int", nullable: true),
                    FK_TransactionCategoryID = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__3214EC278CDE2116", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transaction_Account",
                        column: x => x.FK_AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_TransactionCategory",
                        column: x => x.FK_TransactionCategoryID,
                        principalTable: "TransactionCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_TransactionType",
                        column: x => x.FK_TransactionTypeID,
                        principalTable: "TransactionType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomaticPaymentLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_AutomaticPaymentID = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AutomaticPaymentLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AutomaticPaymentLog_Payment",
                        column: x => x.FK_AutomaticPaymentID,
                        principalTable: "AutomaticPayment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_FK_AccountTypeID",
                table: "Account",
                column: "FK_AccountTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_AutomaticPayment_FK_AccountID",
                table: "AutomaticPayment",
                column: "FK_AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_AutomaticPayment_FK_CategoryID",
                table: "AutomaticPayment",
                column: "FK_CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_AutomaticPayment_FK_FrequencyID",
                table: "AutomaticPayment",
                column: "FK_FrequencyID");

            migrationBuilder.CreateIndex(
                name: "IX_AutomaticPaymentLog_FK_AutomaticPaymentID",
                table: "AutomaticPaymentLog",
                column: "FK_AutomaticPaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_HouseSaving_FK_AccountID",
                table: "HouseSaving",
                column: "FK_AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FK_AccountID",
                table: "Transaction",
                column: "FK_AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FK_TransactionCategoryID",
                table: "Transaction",
                column: "FK_TransactionCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FK_TransactionTypeID",
                table: "Transaction",
                column: "FK_TransactionTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomaticPaymentLog");

            migrationBuilder.DropTable(
                name: "HouseSaving");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "AutomaticPayment");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "TransactionCategory");

            migrationBuilder.DropTable(
                name: "PaymentFrequency");

            migrationBuilder.DropTable(
                name: "AccountType");
        }
    }
}
