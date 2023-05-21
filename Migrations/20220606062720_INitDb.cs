using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniChatApplication.Migrations
{
    public partial class INitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    contactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contactTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactMes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactProgress = table.Column<bool>(type: "bit", nullable: false),
                    contactCustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactCustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact", x => x.contactId);
                });

            migrationBuilder.CreateTable(
                name: "login_cookie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login_key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_cookie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "admin_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    account_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_admin_profile_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    teacher_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teacher_profile_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    student_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    class_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_profile_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_profile_class_class_id",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "room_chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_id = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    teacher_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_room_chat_class_class_id",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_chat_subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_chat_teacher_profile_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    _index = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_group_chat_room_chat_room_id",
                        column: x => x.room_id,
                        principalTable: "room_chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_dealine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    expiration_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_dealine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_room_dealine_room_chat_room_id",
                        column: x => x.room_id,
                        principalTable: "room_chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    time_message = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_room_message_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_message_room_chat_room_id",
                        column: x => x.room_id,
                        principalTable: "room_chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_dealine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    expiration_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_dealine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_group_dealine_group_chat_GroupId",
                        column: x => x.GroupId,
                        principalTable: "group_chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_manage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_manage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_group_manage_group_chat_group_id",
                        column: x => x.group_id,
                        principalTable: "group_chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_group_manage_student_profile_student_id",
                        column: x => x.student_id,
                        principalTable: "student_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    time_message = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_group_message_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_group_message_group_chat_group_id",
                        column: x => x.group_id,
                        principalTable: "group_chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_marked_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_message_id = table.Column<int>(type: "int", nullable: false),
                    time_marked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_marked_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_room_marked_message_room_message_room_message_id",
                        column: x => x.room_message_id,
                        principalTable: "room_message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_marked_message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    group_message_id = table.Column<int>(type: "int", nullable: false),
                    time_marked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_marked_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_group_marked_message_group_message_group_message_id",
                        column: x => x.group_message_id,
                        principalTable: "group_message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admin_profile_account_id",
                table: "admin_profile",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_group_chat_room_id",
                table: "group_chat",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_dealine_GroupId",
                table: "group_dealine",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_group_manage_group_id",
                table: "group_manage",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_manage_student_id",
                table: "group_manage",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_marked_message_group_message_id",
                table: "group_marked_message",
                column: "group_message_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_message_account_id",
                table: "group_message",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_message_group_id",
                table: "group_message",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_chat_class_id",
                table: "room_chat",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_chat_subject_id",
                table: "room_chat",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_chat_teacher_id",
                table: "room_chat",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_dealine_room_id",
                table: "room_dealine",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_marked_message_room_message_id",
                table: "room_marked_message",
                column: "room_message_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_message_account_id",
                table: "room_message",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_message_room_id",
                table: "room_message",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_profile_account_id",
                table: "student_profile",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_profile_class_id",
                table: "student_profile",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_profile_account_id",
                table: "teacher_profile",
                column: "account_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_profile");

            migrationBuilder.DropTable(
                name: "contact");

            migrationBuilder.DropTable(
                name: "group_dealine");

            migrationBuilder.DropTable(
                name: "group_manage");

            migrationBuilder.DropTable(
                name: "group_marked_message");

            migrationBuilder.DropTable(
                name: "login_cookie");

            migrationBuilder.DropTable(
                name: "room_dealine");

            migrationBuilder.DropTable(
                name: "room_marked_message");

            migrationBuilder.DropTable(
                name: "student_profile");

            migrationBuilder.DropTable(
                name: "group_message");

            migrationBuilder.DropTable(
                name: "room_message");

            migrationBuilder.DropTable(
                name: "group_chat");

            migrationBuilder.DropTable(
                name: "room_chat");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "teacher_profile");

            migrationBuilder.DropTable(
                name: "account");
        }
    }
}
