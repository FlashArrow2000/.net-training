using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Negocio_Libros.Contexts;
using API_Negocio_Libros.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Negocio_Libros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Libro>> Get()
        {
            //Autor es una propiedad de navegacion, esta sirve para traer datos de la entidad Autor
            //Include es para incluir la informacion dentro de Autor en Libros
            return context.Libros.Include(x => x.Autor).ToList();
        }

        [HttpGet("{id}", Name = "ObtenerLibro")]
        public ActionResult<Libro> Get(int id)
        {
            var libro = context.Libros.Include(x => x.Autor).FirstOrDefault(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Libro libro)
        {
            context.Libros.Add(libro);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerLibro", new { id = libro.Id }, libro);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }

            context.Entry(libro).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Libro> Delete(int id)
        {
            var libro = context.Libros.FirstOrDefault(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            context.Libros.Remove(libro);
            context.SaveChanges();
            return libro;
        }


    }
}
