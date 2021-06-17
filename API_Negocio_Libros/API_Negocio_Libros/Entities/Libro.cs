using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Negocio_Libros.Entities
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public int AutorId { get; set; }

        //Autor es una propiedad de navegacion, esta sirve para traer datos de la entidad Autor
        public Autor Autor { get; set; }
    }
}
