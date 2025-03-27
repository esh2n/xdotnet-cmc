using System;

namespace BlockchainMarketCap.Models
{
    /// <summary>
    /// Represents cryptocurrency market data
    /// </summary>
    public class CryptoCurrencyData
    {
        /// <summary>
        /// Unique identifier for the cryptocurrency
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Ticker symbol (e.g., BTC, ETH)
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Full name of the cryptocurrency
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL to the cryptocurrency's icon
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Current price in USD
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Total market capitalization in USD
        /// </summary>
        public decimal MarketCap { get; set; }

        /// <summary>
        /// Market cap rank compared to other cryptocurrencies
        /// </summary>
        public int MarketCapRank { get; set; }

        /// <summary>
        /// 24-hour price change percentage
        /// </summary>
        public decimal PriceChangePercentage24h { get; set; }

        /// <summary>
        /// Number of coins in circulation
        /// </summary>
        public decimal CirculatingSupply { get; set; }

        /// <summary>
        /// Maximum supply of coins (if applicable)
        /// </summary>
        public decimal? TotalSupply { get; set; }

        /// <summary>
        /// Last time the data was updated
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}