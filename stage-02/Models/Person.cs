using Microsoft.EntityFrameworkCore;

namespace hngx_duo.Models;

// actual database model object
[Index(nameof(Name), IsUnique = true)]
public class Person {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public Colour FavouriteColour { get; set; }

    public PersonDTO ToDTO() {
        return new() {
            Id = Id,
            Name = Name,
            Age = Age,
            FavouriteColour = FavouriteColour.ToString()
        };
    }
}

// DTO to hide colour representation
public class PersonDTO {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string FavouriteColour { get; set; } = null!;
}

public enum Colour {
    Red,
    Green,
    Blue,
    Yellow,
    Orange,
    Purple,
    Black
}