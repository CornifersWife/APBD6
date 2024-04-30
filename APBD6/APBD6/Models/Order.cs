using System.ComponentModel.DataAnnotations;

namespace APBD6.Models;

using System.ComponentModel.DataAnnotations;

public class Order {
    [Required] private int IdOrder;
    [Required] private int IdProduct;
    [Required] [Range(1, int.MaxValue)] private int Amount;
    [Required] private DateTime CreatedAt;
    private DateTime FulfiledAt;
}