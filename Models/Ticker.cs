namespace Expenses.Models
{
    public class Ticker
    {
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Vol { get; set; }
        public decimal Last { get; set; }
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
        public int Date { get; set; }
    }

    public class TickerResponse
    {
        public Ticker Ticker { get; set; }
    }
}
