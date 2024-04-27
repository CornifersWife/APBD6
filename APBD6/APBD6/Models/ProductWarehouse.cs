using System.ComponentModel.DataAnnotations;

namespace APBD6.Models;

public class ProductWarehouse {
    [Required] private int IdProduct { get; set; }
    [Required] private int IdWarehouse { get; set; }
    [Required] [Range(1, int.MaxValue)] private int Amount { get; set; }
    [Required] private DateTime CreatedAt { get; set; }
}