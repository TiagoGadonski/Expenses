using Expenses.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

public class CryptoPredictionService
{
    private readonly MLContext _mlContext;
    private ITransformer _model;

    public CryptoPredictionService()
    {
        _mlContext = new MLContext();
        _model = null;
    }

    public void TrainModel(IEnumerable<CryptoPrice> data)
    {
        var dataView = _mlContext.Data.LoadFromEnumerable(data);

        var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(CryptoPrice.DateNumeric), nameof(CryptoPrice.SentimentScore))
            .Append(_mlContext.Transforms.CopyColumns("Label", nameof(CryptoPrice.Price)))
            .Append(_mlContext.Regression.Trainers.LbfgsPoissonRegression(labelColumnName: "Label", featureColumnName: "Features"));

        var crossValidationResults = _mlContext.Regression.CrossValidate(dataView, pipeline, numberOfFolds: 5);

        foreach (var result in crossValidationResults)
        {
            Console.WriteLine($"Fold: {result.Fold}, R-Squared: {result.Metrics.RSquared:0.###}, RMSE: {result.Metrics.RootMeanSquaredError:0.###}");
        }

        _model = pipeline.Fit(dataView);
    }

    public float Predict(CryptoPrice data)
    {
        if (_model == null)
        {
            throw new InvalidOperationException("Model has not been trained yet.");
        }

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<CryptoPrice, CryptoPrediction>(_model);
        var prediction = predictionEngine.Predict(data);
        Console.WriteLine($"Predicted price for {data.LastUpdated}: {prediction.Price}");
        return prediction.Price;
    }

    public class CryptoPrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}
