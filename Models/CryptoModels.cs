namespace Expenses.Models
{
    public class CryptoPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public float Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public float SentimentScore { get; set; }
        public float DateNumeric => (float)LastUpdated.ToOADate();
    }

    public class CryptoNews
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime PublishedAt { get; set; }
    }

    public class CryptoFeedback
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float PredictedPrice { get; set; }
        public float ActualPrice { get; set; }
        public float SentimentScore { get; set; }
    }
}
