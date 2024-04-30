using System.ComponentModel.DataAnnotations;

namespace APBD6.Models;

public class AddProductWarehouse {
    [Required] public int IdProduct { get; set; }
    [Required] public int IdWarehouse { get; set; }
    [Required] [Range(1, int.MaxValue)] public int Amount { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
}