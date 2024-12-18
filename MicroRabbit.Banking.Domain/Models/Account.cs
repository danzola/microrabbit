namespace MicroRabbit.Banking.Domain.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountType { get; set; } = default!;
        public decimal AccountBalance { get; set; }
    }
}
