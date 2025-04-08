namespace EFCoreDemo.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Avoid null warnings
    public decimal Price { get; set; }
}