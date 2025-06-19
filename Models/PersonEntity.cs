namespace BarberApi.Models;

public abstract class PersonEntity: BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Picture { get; set; }
}