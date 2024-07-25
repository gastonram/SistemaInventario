let datatable;

/*esta funcion carga la funcion de load cuando se ingresa en el index */
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({        

            /* esto es para cambiar el lenguaje del datatable a español */
            "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },

        "ajax": {
            "url": "/Admin/Bodega/ObtenerTodos"

        },
        "columns":
            [
                { "data": "id", "width": "5%" },
                { "data": "nombre", "width": "20%" },
                { "data": "descripcion", "width": "40%" },
                {

                    "data": "estado",
                    "render": function (data) {
                        if (data == true) {
                            return "Activo";
                        } else {
                            return "Inactivo";
                        }
                    }, "width": "20%"
                },
                {
                    /* botones para editar y elminar en la columna */
                    "data": "id",
                    "render": function (data) {
                        return `
                                <div class="text-center">
                                    <a href="/Admin/Bodega/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                        <i class="bi bi-pencil-square"></i>
                                    </a>
                                    <a onclick="Delete('/Admin/Bodega/Delete/${data}')" class="btn btn-danger text-white" style="cursor:pointer;">
                                        <i class="bi bi-trash3-fill"></i>
                                    </a>
                                </div>
                            `;
                    },
                    "width": "15%"


                }


            ]
    });
};

