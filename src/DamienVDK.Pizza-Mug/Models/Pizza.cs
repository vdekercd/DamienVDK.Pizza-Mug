namespace DamienVDK.Pizza_Mug.Models;

[Table("Pizzas")]
public class Pizza
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public int PreparationTimeInMinuutes { get; set; }
    
    public decimal Price { get; set; }
}