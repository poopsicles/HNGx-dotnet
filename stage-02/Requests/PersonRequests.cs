using hngx_duo.Models;
using hngx_duo.Services;

namespace hngx_duo.Requests;

// a possible list of errors
public class ValidationResponse {
    public List<string> errors = new();

    public bool IsSuccess() {
        return errors.Count() == 0;
    }
}

// represents the request parameters for POST /api
public class PostPersonRequest
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string FavouriteColour { get; set; } = null!;

    // connects to the db and validates rules
    public ValidationResponse Validate(ApplicationContext _context) {
        var response = new ValidationResponse();

        // age cannot be zero or negative
        if (Age < 1) {
            response.errors.Add("Age cannot be zero or negative");
        }

        // colour must be one of the accepted values
        if (!Enum.TryParse(FavouriteColour, true, out Colour _))
        {
            response.errors.Add("Invalid colour");
        }

        // names must be unique
        if (_context.People.Any(p => p.Name == Name.Trim())) {
            response.errors.Add("Name already exists in database");
        }

        return response;
    }

    // transforms the request to a person, Validate() should be called first
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

// represents the request parameters for PUT /api
public class PatchPersonRequest
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? FavouriteColour { get; set; } 

    // connects to the db and validates rules
    public ValidationResponse Validate(ApplicationContext _context, string CurrentName) {
        var response = new ValidationResponse();

        // age cannot be zero or negative
        if (Age.HasValue && Age < 1) {
            response.errors.Add("Age cannot be zero or negative");
        }

        // colour must be one of the accepted values
        if (FavouriteColour != null && !Enum.TryParse(FavouriteColour, true, out Colour _))
        {
            response.errors.Add("Invalid colour");
        }

        // names must be unique
        if (Name != null && _context.People.Any(p => p.Name == Name.Trim())) {
            response.errors.Add("Duplicate name");
        }

        return response;
    }
}