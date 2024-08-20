namespace Shared.Models;

public class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public byte Age { get; set; }
}

public enum Gender : byte
{
    Male,
    Female
}