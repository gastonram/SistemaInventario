using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos.Especificaciones
{
    public class MetaData
    {
        public int TotalPages { get; set; }//total de paginas de toda la lista
        public int PageSize { get; set; }//cantidad de elementos por pagina
        public int TotalCount { get; set; }//total de elementos en la lista

    }
}
