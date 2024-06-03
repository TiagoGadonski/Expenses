namespace Expenses.Models
{
    public class CryptoTransaction
    {
        public int Id { get; set; }
        public string CryptoName { get; set; }
        public decimal Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime PriceDate { get; set; }
    }
}
