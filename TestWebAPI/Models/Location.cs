using API.Models.Base;

namespace API.Models;

public class Location : Entity {

    public Location() {}

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;


}