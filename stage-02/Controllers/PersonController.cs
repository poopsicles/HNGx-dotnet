using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

using hngx_duo.Models;
using hngx_duo.Services;
using hngx_duo.Requests;

namespace hngx_duo.Controllers
{
    [Route("api")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PersonController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/XXXX-XXXX
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

        // POST: /api
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
                return UnprocessableEntity(validation.ToJson());
            }

            var newPerson = request.ToPerson();

            _context.People.Add(newPerson);
            await _context.SaveChangesAsync();

            // 201 with PersonDTO of new object
            return CreatedAtAction("GetPerson", new { id = newPerson.Id }, newPerson.ToDTO());

        }

        // PATCH: api/XXXX-XXXX
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
                return UnprocessableEntity(validation.ToJson());
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

        // DELETE: api/XXXX-XXXX
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

        // drop all data
        [HttpDelete("super-secret-drop-all")]
        public async Task<IActionResult> DeleteAll()
        {
            if (_context.People == null)
            {
                return NotFound();
            }

            _context.People.RemoveRange(_context.People);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
