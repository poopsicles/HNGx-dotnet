using hngx_duo.Models;
using hngx_duo.Services;
namespace hngx_duo.Requests;

public class ValidationResponse {
    public List<string> errors = new();

    public bool IsSuccess() {
        return errors.Count() == 0;
    }
}

public class PostPersonRequest
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string FavouriteColour { get; set; } = null!;

    public ValidationResponse Validate(ApplicationContext _context) {
        var response = new ValidationResponse();

        if (Age < 1) {
            response.errors.Add("Invalid age");
        }

        if (!Enum.TryParse(FavouriteColour, true, out Colour _))
        {
            response.errors.Add("Invalid colour");
        }

        if (_context.People.Any(p => p.Name == Name)) {
            response.errors.Add("Duplicate name");
        }

        return response;
    }

    public Person ToPerson()
    { 
        Enum.TryParse(FavouriteColour, true, out Colour parsedColour);

        return new Person()
        {
            Name = Name.Trim(),
            Age = Age,
            FavouriteColour = parsedColour
        };
    }
}