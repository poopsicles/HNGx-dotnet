using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hngx_duo.Models;
using hngx_duo.Services;
using hngx_duo.Requests;
using NuGet.Protocol;

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
        public async Task<ActionResult<Person>> GetPerson(Guid id)
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

            return person;
        }

        // PUT: api/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(Guid id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: /api
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonDTO>> PostPerson(PostPersonRequest request)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'ApplicationContext.People'  is null.");
            }

            var validation = request.Validate(_context);

            if (validation.IsSuccess()) {
                var newPerson = request.ToPerson();

                _context.People.Add(newPerson);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPerson", new { id = newPerson.Id }, newPerson.ToDTO());
            } else {
                return UnprocessableEntity(validation.ToJson());
            }
        }

        // DELETE: api/Person/5
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

        // GET: api/super-secret
        [HttpGet("super-secret")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            if (_context.People == null)
            {
                return NotFound();
            }
            return await _context.People.ToListAsync();
        }

        // GET: api/drop-all
        [HttpGet("drop-all")]
        public async Task<ActionResult> DropAll()
        {
            _context.People.RemoveRange(await _context.People.ToListAsync());
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PersonExists(Guid id)
        {
            return (_context.People?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
