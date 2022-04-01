namespace DamienVDK.Pizza_Mug.Models;

[Table("Pizzas")]
public class Pizza
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    
    public int PreparationTimeInMinuutes { get; set; }
    
    public decimal Price { get; set; }
}