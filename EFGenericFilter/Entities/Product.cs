namespace EFGenericFilter.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}