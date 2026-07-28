using System.Runtime.CompilerServices;

namespace ServerSentEvents.StockUpdates;

public class StockService
{
	public async IAsyncEnumerable<StockPriceEvent> GenerateStockPrices(
		[EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var symbols = new[] { "MSFT", "AAPL", "GOOG", "AMZN" };

		while (!cancellationToken.IsCancellationRequested)
		{
			// Pick a random symbol and price
			var symbol = symbols[Random.Shared.Next(symbols.Length)];
			var price  = Math.Round((decimal)(100 + Random.Shared.NextDouble() * 50), 2);

			var id = DateTime.UtcNow.ToString("o");

			yield return new StockPriceEvent(id, symbol, price, DateTime.UtcNow);

			// Wait 2 seconds before sending the next update
			await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
		}
	}

	public async IAsyncEnumerable<StockPriceEvent> GenerateStockPricesSince(
		string? lastEventId,
		[EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var symbols = new[] { "MSFT", "AAPL", "GOOG", "AMZN" };

		while (!cancellationToken.IsCancellationRequested)
		{
			// Pick a random symbol and price
			var symbol = symbols[Random.Shared.Next(symbols.Length)];
			var price  = Math.Round((decimal)(100 + Random.Shared.NextDouble() * 50), 2);

			var id = DateTime.UtcNow.ToString("o");

			yield return new StockPriceEvent(id, symbol, price, DateTime.UtcNow);

			// Wait 2 seconds before sending the next update
			await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
		}
	}
}
