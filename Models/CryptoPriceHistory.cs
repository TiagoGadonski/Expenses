namespace Expenses.Models
{
    public class CryptoPriceHistory
    {
        public int Id { get; set; }
        public string CryptoName { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
