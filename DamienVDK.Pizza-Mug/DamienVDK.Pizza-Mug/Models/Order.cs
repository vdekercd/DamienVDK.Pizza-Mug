namespace DamienVDK.Pizza_Mug.Models;

[Table("Orders")]
public class Order
{
    [Key]
    public int OrderId { get; set; }
    [Required]
    public string Session { get; set; }
    
    public DateTime DeliveryDate { get; set; }
    
    public virtual ICollection<OrderPizza> OrderPizzas { get; set; }
}