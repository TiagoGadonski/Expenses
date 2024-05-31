using Microsoft.ML;
using Microsoft.ML.Data;

public class SentimentAnalysisService
{
    private readonly MLContext _mlContext;
    private ITransformer _model;

    public SentimentAnalysisService()
    {
        _mlContext = new MLContext();
        _model = TrainModel();
    }

    private ITransformer TrainModel()
    {
        var trainingData = new List<SentimentData>
        {
            new SentimentData { Text = "Crypto market is booming", Sentiment = true },
            new SentimentData { Text = "Crypto market crashes", Sentiment = false },
            // Add more training data here
        };

        var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(_mlContext.Transforms.Concatenate("Features", "Features"))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Sentiment", featureColumnName: "Features"));

        return pipeline.Fit(dataView);
    }

    public float PredictSentiment(string text)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
        var prediction = predictionEngine.Predict(new SentimentData { Text = text });
        return prediction.Probability;
    }

    public class SentimentData
    {
        public string Text { get; set; }
        public bool Sentiment { get; set; }
    }

    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Sentiment { get; set; }
        public float Probability { get; set; }
    }
}
