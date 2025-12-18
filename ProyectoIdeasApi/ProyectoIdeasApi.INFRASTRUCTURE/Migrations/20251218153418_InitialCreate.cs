using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoIdeasApi.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "log_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    level = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    exception = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    member_name = table.Column<string>(type: "text", nullable: true),
                    line_number = table.Column<int>(type: "integer", nullable: true),
                    trace_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_log_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "miembros",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    auto_descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_miembros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rubros",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rubros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "habilidades",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    miembro_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_habilidades", x => x.id);
                    table.ForeignKey(
                        name: "fk_habilidades_miembros_miembro_id",
                        column: x => x.miembro_id,
                        principalTable: "miembros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "miembros_intimo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    propietario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    intimo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_agregado = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    nota = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_miembros_intimo", x => x.id);
                    table.ForeignKey(
                        name: "fk_miembros_intimo_miembros_intimo_id",
                        column: x => x.intimo_id,
                        principalTable: "miembros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_miembros_intimo_miembros_propietario_id",
                        column: x => x.propietario_id,
                        principalTable: "miembros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_login = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    email_confirmado = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    miembro_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "fk_usuarios_miembros_miembro_id",
                        column: x => x.miembro_id,
                        principalTable: "miembros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ideas_concretas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    proposito = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    activa = table.Column<bool>(type: "boolean", nullable: false),
                    creador_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rubro_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ideas_concretas", x => x.id);
                    table.ForeignKey(
                        name: "fk_ideas_concretas_miembros_creador_id",
                        column: x => x.creador_id,
                        principalTable: "miembros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ideas_concretas_rubros_rubro_id",
                        column: x => x.rubro_id,
                        principalTable: "rubros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "grupos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    idea_concreta_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grupos", x => x.id);
                    table.ForeignKey(
                        name: "fk_grupos_ideas_concretas_idea_concreta_id",
                        column: x => x.idea_concreta_id,
                        principalTable: "ideas_concretas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interesados",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_interes = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    comentario = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    miembro_id = table.Column<Guid>(type: "uuid", nullable: false),
                    idea_concreta_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interesados", x => x.id);
                    table.ForeignKey(
                        name: "fk_interesados_ideas_concretas_idea_concreta_id",
                        column: x => x.idea_concreta_id,
                        principalTable: "ideas_concretas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_interesados_miembros_miembro_id",
                        column: x => x.miembro_id,
                        principalTable: "miembros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "links_externo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    grupo_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_links_externo", x => x.id);
                    table.ForeignKey(
                        name: "fk_links_externo_grupos_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "miembros_grupo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    rol = table.Column<int>(type: "integer", maxLength: 30, nullable: false),
                    miembro_id = table.Column<Guid>(type: "uuid", nullable: false),
                    grupo_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_miembros_grupo", x => x.id);
                    table.ForeignKey(
                        name: "fk_miembros_grupo_grupos_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_miembros_grupo_miembros_miembro_id",
                        column: x => x.miembro_id,
                        principalTable: "miembros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_grupos_idea_concreta_id",
                table: "grupos",
                column: "idea_concreta_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_habilidades_miembro_id",
                table: "habilidades",
                column: "miembro_id");

            migrationBuilder.CreateIndex(
                name: "ix_ideas_concretas_creador_id",
                table: "ideas_concretas",
                column: "creador_id");

            migrationBuilder.CreateIndex(
                name: "ix_ideas_concretas_rubro_id",
                table: "ideas_concretas",
                column: "rubro_id");

            migrationBuilder.CreateIndex(
                name: "ix_interesados_idea_concreta_id",
                table: "interesados",
                column: "idea_concreta_id");

            migrationBuilder.CreateIndex(
                name: "ix_interesados_miembro_id",
                table: "interesados",
                column: "miembro_id");

            migrationBuilder.CreateIndex(
                name: "ix_links_externo_grupo_id",
                table: "links_externo",
                column: "grupo_id");

            migrationBuilder.CreateIndex(
                name: "ix_miembros_grupo_grupo_id",
                table: "miembros_grupo",
                column: "grupo_id");

            migrationBuilder.CreateIndex(
                name: "ix_miembros_grupo_miembro_id",
                table: "miembros_grupo",
                column: "miembro_id");

            migrationBuilder.CreateIndex(
                name: "ix_miembros_intimo_intimo_id",
                table: "miembros_intimo",
                column: "intimo_id");

            migrationBuilder.CreateIndex(
                name: "ix_miembros_intimo_propietario_id",
                table: "miembros_intimo",
                column: "propietario_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_miembro_id",
                table: "usuarios",
                column: "miembro_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "habilidades");

            migrationBuilder.DropTable(
                name: "interesados");

            migrationBuilder.DropTable(
                name: "links_externo");

            migrationBuilder.DropTable(
                name: "log_entries");

            migrationBuilder.DropTable(
                name: "miembros_grupo");

            migrationBuilder.DropTable(
                name: "miembros_intimo");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "grupos");

            migrationBuilder.DropTable(
                name: "ideas_concretas");

            migrationBuilder.DropTable(
                name: "miembros");

            migrationBuilder.DropTable(
                name: "rubros");
        }
    }
}
