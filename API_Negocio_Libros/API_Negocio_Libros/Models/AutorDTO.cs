using API_Negocio_Libros.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Negocio_Libros.Models
{
    public class AutorDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<LibroDTO> Libros { get; set; }
    }
}
