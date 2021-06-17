using API_Negocio_Libros.Contexts;
using API_Negocio_Libros.Entities;
using API_Negocio_Libros.Helpers;
using API_Negocio_Libros.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Negocio_Libros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;

        /*
         * IMPORTATE: hay varios tipos de Loggin y por niveles
         *  - Critical
         *  - Error
         *  - Warning
         *  - Information
         *  - Debug
         *  - Trace
         */

        public AutoresController(ApplicationDbContext context, ILogger<AutoresController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        //Un IEnumerable es una coleccion u objeto.
        //Se puede hacer responsivo a dos tipos de rutas
        [HttpGet("/listado")] //GET /listado
        [HttpGet("listado")] //GET api/autores/listado
        [HttpGet]
        //Aqui agrego mi filtro de accion personalizado. Como estoy usando inyecc. de depend. uso el [ServiceFilter]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            logger.LogInformation("Obteniendo los autores");
            var autores = await context.Autores.Include(x => x.Libros).ToListAsync();
            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);
            return autoresDTO;
        }

        //GET api/autores/primer
        [HttpGet("primer")]
        public ActionResult<Autor> GetPrimerAutor()
        {
            return context.Autores.FirstOrDefault();
        }

        //Get por ID
        // Para asignar otro param se debe hacer asi: "{id}/{param2}". Y para hacerlo opcional solo hay que poner un signo de interrogacion.
        // Si se desea poner uno por defecto es asi: "{id}/{param2=Daniel}"
        [HttpGet("{id}", Name = "ObtenerAutor")]
        public ActionResult<AutorDTO> Get(int id)
        {
            var autor = context.Autores.Include(x => x.Libros).FirstOrDefault(x => x.Id == id);
            if (autor == null)
            {
                logger.LogWarning($"El autor de ID {id} no ha sido encontrado");
                //Esto es para retornar un Error 404
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return autorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacion_DTO autorCreacion)
        {

            var autor = mapper.Map<Autor>(autorCreacion);
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            //aqui le pasamos el id del autor al metodo GET que recibe un ID por medio del nombre de ruta, y retornamos un autorDTO, dado que eso es lo que retornamos en todo lado. 
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autorDTO);
        }

        //Actualizar 
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacion_DTO autorActualizacion)
            //Aqui uso un AutorCreacionDTO porque las reglas de negocio para actualizar son las mismas que para crear
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            //Aqui se retorna un 204 No Content
            return NoContent();     
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacion_DTO> patchDocument )
        {
            //Evaluamos si lo que viene del body es nulo
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var autorBD = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            //Evaluamos si el id de la base de datos coincide con el del body
            if (autorBD == null)
            {
                return NotFound();
            }
            //Aplico las reglas del AutorCreacion DTO para autorDB 
            var autorBD_DTO = mapper.Map<AutorCreacion_DTO>(autorBD);
            //Aplico el patch al modelo
            patchDocument.ApplyTo(autorBD_DTO, ModelState);
            //Paso la data del DTO al modelo original para hacer la validacion de negocio DEL DTO
            mapper.Map(autorBD_DTO, autorBD);

            var isValid = TryValidateModel(autorBD);
            //Evaluamos si lo que viene del body no viola nuestras reglas de negocio (como esta la entidad)
            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            //Salvamos cambios y retornamos un 204 No Content
            await context.SaveChangesAsync();

            return NoContent();

        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            var autorId = await context.Autores.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);

            if (autorId == default(int))
            {
                return NotFound();
            }

            context.Autores.Remove(new Autor { Id = autorId });
            context.SaveChanges();
            return NoContent();
        }


    }
}
