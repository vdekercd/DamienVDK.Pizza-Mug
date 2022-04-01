namespace DamienVDK.Pizza_Mug.Models;

public class OrderPizza
{
    [Key]
    public int OrderPizzaId { get; set; }
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }
    [ForeignKey(nameof(Pizza))]
    public int PizzaID { get; set; }
    public int Quantity { get; set; }
    public virtual Order Order { get; set; } = null!;
    public virtual Pizza Pizza { get; set; } = null!;
}