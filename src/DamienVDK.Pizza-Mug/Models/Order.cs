namespace DamienVDK.Pizza_Mug.Models;

[Table("Orders")]
public class Order
{
    public Order()
    {
        OrderPizzas = new HashSet<OrderPizza>();
    }
    
    [Key]
    public int OrderId { get; }

    [Required] public string Session { get; set; } = string.Empty;
    
    public DateTime DeliveryDate { get; set; }
    
    public virtual ICollection<OrderPizza> OrderPizzas { get; set; }
}