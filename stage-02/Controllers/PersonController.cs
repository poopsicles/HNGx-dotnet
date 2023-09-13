using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

using hngx_duo.Models;
using hngx_duo.Services;
using hngx_duo.Requests;
using Newtonsoft.Json;

namespace hngx_duo.Controllers
{
    [Route("api")]
    [ApiController]
    // [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PersonController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a person by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/XXXX-XXXX
        /// 
        /// Sample response:
        /// 
        ///     200 OK
        ///     {
        ///         "id": "XXXX-XXXX",
        ///         "name": "John Smith",
        ///         "age": 42,
        ///         "favouriteColour": "Blue"
        ///     }
        ///    
        /// </remarks>
        /// <param name="id">The id of the person to be queried</param>
        /// <returns>A PersonDTO with the attributes</returns>
        /// <response code="200">Returns the entity.</response>
        /// <response code="404">If the entity is not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPerson(Guid id)
        {   
            if (_context.People == null)
            {
                return NotFound(); // 404
            }

            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound(); // 404
            }

            return person.ToDTO(); // 200 with PersonDTO of entity
        }

        /// <summary>
        /// Create a new person
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api
        ///     {
        ///         "name": "John Smith",
        ///         "age": 42,
        ///         "favouriteColour": "Blue"
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     201 Created
        ///     {
        ///         "id": "XXXX-XXXX",
        ///         "name": "John Smith",
        ///         "age": 42,
        ///         "favouriteColour": "Blue"
        ///     }
        ///    
        /// </remarks>
        /// <returns>A PersonDTO with the attributes</returns>
        /// <response code="200">Returns the entity.</response>
        /// <response code="422">If validation fails.</response>        
        [HttpPost]
        public async Task<ActionResult<PersonDTO>> PostPerson(PostPersonRequest request)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'ApplicationContext.People' is null.");
            }

            var validation = request.Validate(_context);

            if (!validation.IsSuccess())
            {
                // 422 with list of errors
                return UnprocessableEntity(validation);
            }

            var newPerson = request.ToPerson();

            _context.People.Add(newPerson);
            await _context.SaveChangesAsync();

            // 201 with PersonDTO of new object
            return CreatedAtAction("GetPerson", new { id = newPerson.Id }, newPerson.ToDTO());

        }

        /// <summary>
        /// Updates a person
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH /api/XXXX-XXXX
        ///     {
        ///         "name": "Mary Smith",
        ///         "age": 10,
        ///         "favouriteColour": "Yellow"
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     200 OK
        ///     {
        ///         "id": "XXXX-XXXX",
        ///         "name": "Mary Smith",
        ///         "age": 10,
        ///         "favouriteColour": "Yellow"
        ///     }
        ///    
        /// </remarks>
        /// <returns>A PersonDTO with the attributes</returns>
        /// <response code="200">Returns the entity.</response>
        /// <response code="404">If the entity is not found.</response>
        /// <response code="422">If validation fails.</response>  
        [HttpPatch("{id}")]
        public async Task<ActionResult<PersonDTO>> PatchPerson(Guid id, PatchPersonRequest request)
        {

            var queriedPerson = await _context.People.FindAsync(id);

            if (queriedPerson == null)
            {
                return NotFound(); // 404
            }

            var validation = request.Validate(_context, queriedPerson.Name);

            if (!validation.IsSuccess())
            {
                // 422 with list of errors
                return UnprocessableEntity(validation);
            }

            // modify entity if request asks for it
            if (request.Age.HasValue)
            {
                queriedPerson.Age = request.Age.Value;
            }

            if (request.Name != null)
            {
                queriedPerson.Name = request.Name;
            }

            if (request.FavouriteColour != null)
            {
                Enum.TryParse(request.FavouriteColour, true, out Colour parsedColour);

                queriedPerson.FavouriteColour = parsedColour;
            }

            await _context.SaveChangesAsync();

            return queriedPerson.ToDTO();
        }

        /// <summary>
        /// Delete a person
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/XXXX-XXXX
        ///
        /// Sample response:
        /// 
        ///     204 No Content
        ///    
        /// </remarks>
        /// <returns>No Content</returns>
        /// <response code="204">Nothing.</response>
        /// <response code="404">If the entity does not exist.</response>           
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            if (_context.People == null)
            {
                return NotFound();
            }
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
