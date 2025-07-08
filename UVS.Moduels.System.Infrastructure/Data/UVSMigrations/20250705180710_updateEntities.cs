using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UVS.Modules.System.Infrastructure.Data.UVSMigrations
{
    /// <inheritdoc />
    public partial class updateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_instructors_instructor_id",
                table: "courses");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "students",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "national_id",
                table: "students",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "students",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "students",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AddColumn<int>(
                name: "academic_year",
                table: "semesters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "department_id",
                table: "semesters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "is_withdrawn",
                table: "enrollments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "withdrawal_date",
                table: "enrollments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "max_credit_hours_per_semester",
                table: "departments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "instructor_id",
                table: "courses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "semester_course",
                columns: table => new
                {
                    semester_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_semester_course", x => new { x.course_id, x.semester_id });
                    table.ForeignKey(
                        name: "fk_semester_course_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_semester_course_semesters_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semesters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_semesters_department_id",
                table: "semesters",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_courses_department_id",
                table: "courses",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_semester_course_semester_id",
                table: "semester_course",
                column: "semester_id");

            migrationBuilder.AddForeignKey(
                name: "fk_courses_departments_department_id",
                table: "courses",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_courses_instructors_instructor_id",
                table: "courses",
                column: "instructor_id",
                principalTable: "instructors",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_semesters_departments_department_id",
                table: "semesters",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_departments_department_id",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "fk_courses_instructors_instructor_id",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "fk_semesters_departments_department_id",
                table: "semesters");

            migrationBuilder.DropTable(
                name: "semester_course");

            migrationBuilder.DropIndex(
                name: "ix_semesters_department_id",
                table: "semesters");

            migrationBuilder.DropIndex(
                name: "ix_courses_department_id",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "academic_year",
                table: "semesters");

            migrationBuilder.DropColumn(
                name: "department_id",
                table: "semesters");

            migrationBuilder.DropColumn(
                name: "is_withdrawn",
                table: "enrollments");

            migrationBuilder.DropColumn(
                name: "withdrawal_date",
                table: "enrollments");

            migrationBuilder.DropColumn(
                name: "max_credit_hours_per_semester",
                table: "departments");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "students",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "national_id",
                table: "students",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "students",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "students",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "instructor_id",
                table: "courses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_courses_instructors_instructor_id",
                table: "courses",
                column: "instructor_id",
                principalTable: "instructors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
