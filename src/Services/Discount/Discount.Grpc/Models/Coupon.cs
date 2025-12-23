namespace Discount.Grpc.Models;

public class Coupon
{
    public int Id { get; set; }
    public required string ProductName { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Amount { get; set; }
}