using API_Negocio_Libros.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Negocio_Libros.Entities
{
    //Para hacer validaciones por modelo, se tiene que heredar la clase a la interfaz IValidatableObject. Recordad tocar CRTL + . para generar la interfaz
    public class Autor: IValidatableObject
    {
        /*
         * IMPORTANTE: primero se leen las validaciones por atributo y despues se leen las validaciones por modelo 
         */

        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        //Esta es una validacion por atributo personalizada. el Archivo esta en Helpers
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public int Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<Libro> Libros { get; set; }



        /*Como aqui hacemos una validacino por modelo, tenemos acceso a todas las propiedades del modelo
         * sin embargo, estas validaciones no son reutilizables para poder acceder a ellas desde otros modelos */
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula", new string[] { nameof(Nombre) });
                }

            }
        }
    }
}
