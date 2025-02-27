﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaInventario.AccesoDatos.Migrations
{
    public partial class AgregarInventarioDetalleMigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventarioDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventarioId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    StockAnterior = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventarioDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventarioDetalles_Inventarios_InventarioId",
                        column: x => x.InventarioId,
                        principalTable: "Inventarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_inventarioDetalles_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventarioDetalles_InventarioId",
                table: "InventarioDetalles",
                column: "InventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_inventarioDetalles_ProductoId",
                table: "InventarioDetalles",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventarioDetalles");
        }
    }
}
