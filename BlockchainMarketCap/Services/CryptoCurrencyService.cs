using BlockchainMarketCap.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BlockchainMarketCap.Services
{
    /// <summary>
    /// Service for fetching cryptocurrency data from CoinGecko API
    /// </summary>
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CryptoCurrencyService> _logger;
        private readonly string _baseApiUrl = "https://api.coingecko.com/api/v3";

        public CryptoCurrencyService(HttpClient httpClient, ILogger<CryptoCurrencyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ApiResult<IEnumerable<CryptoCurrencyData>>> GetTopCryptocurrenciesAsync(int limit = 50)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<CoinGeckoMarketResponse>>(
                    $"{_baseApiUrl}/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={limit}&page=1&sparkline=false"
                );

                if (response == null)
                {
                    return ApiResult<IEnumerable<CryptoCurrencyData>>.Failure(
                        "No data received from API",
                        ApiErrorType.Server
                    );
                }

                var result = new List<CryptoCurrencyData>();
                foreach (var item in response)
                {
                    result.Add(MapResponseToCryptoCurrencyData(item));
                }

                return ApiResult<IEnumerable<CryptoCurrencyData>>.Success(result);
            }
            catch (HttpRequestException ex)
            {
                var errorType = DetermineErrorType(ex);
                _logger.LogError(ex, "HTTP error fetching cryptocurrency data: {ErrorMessage}, Status: {StatusCode}",
                    ex.Message, ex.StatusCode);

                string errorMessage = GetFriendlyErrorMessage(ex, errorType);
                return ApiResult<IEnumerable<CryptoCurrencyData>>.Failure(errorMessage, errorType);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout fetching cryptocurrency data");
                return ApiResult<IEnumerable<CryptoCurrencyData>>.Failure(
                    "Request timed out. Please try again later.",
                    ApiErrorType.Timeout
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching cryptocurrency data");
                return ApiResult<IEnumerable<CryptoCurrencyData>>.Failure(
                    "An unexpected error occurred. Please try again later.",
                    ApiErrorType.General
                );
            }
        }

        /// <inheritdoc/>
        public async Task<ApiResult<CryptoCurrencyData>> GetCryptocurrencyDetailsAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ApiResult<CryptoCurrencyData>.Failure(
                    "Cryptocurrency ID is required",
                    ApiErrorType.General
                );
            }

            try
            {
                var response = await _httpClient.GetFromJsonAsync<CoinGeckoDetailsResponse>(
                    $"{_baseApiUrl}/coins/{id}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false"
                );

                if (response == null)
                {
                    return ApiResult<CryptoCurrencyData>.Failure(
                        "Cryptocurrency details not found",
                        ApiErrorType.NotFound
                    );
                }

                var result = new CryptoCurrencyData
                {
                    Id = response.Id,
                    Name = response.Name,
                    Symbol = response.Symbol,
                    Image = response.Image?.Large,
                    CurrentPrice = response.MarketInfo?.CurrentPrice?.Usd ?? 0,
                    MarketCap = response.MarketInfo?.MarketCap?.Usd ?? 0,
                    MarketCapRank = response.MarketInfo?.MarketCapRank ?? 0,
                    PriceChangePercentage24h = response.MarketInfo?.PriceChangePercentage24h ?? 0,
                    CirculatingSupply = response.MarketInfo?.CirculatingSupply ?? 0,
                    TotalSupply = response.MarketInfo?.TotalSupply,
                    LastUpdated = response.LastUpdated
                };

                return ApiResult<CryptoCurrencyData>.Success(result);
            }
            catch (HttpRequestException ex)
            {
                var errorType = DetermineErrorType(ex);
                _logger.LogError(ex, "HTTP error fetching cryptocurrency details for {id}: {ErrorMessage}, Status: {StatusCode}",
                    id, ex.Message, ex.StatusCode);

                string errorMessage = GetFriendlyErrorMessage(ex, errorType);
                return ApiResult<CryptoCurrencyData>.Failure(errorMessage, errorType);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout fetching cryptocurrency details for {id}", id);
                return ApiResult<CryptoCurrencyData>.Failure(
                    "Request timed out. Please try again later.",
                    ApiErrorType.Timeout
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching cryptocurrency details for {id}", id);
                return ApiResult<CryptoCurrencyData>.Failure(
                    "An unexpected error occurred. Please try again later.",
                    ApiErrorType.General
                );
            }
        }

        private string GetFriendlyErrorMessage(HttpRequestException ex, ApiErrorType errorType)
        {
            switch (errorType)
            {
                case ApiErrorType.RateLimit:
                    return "API rate limit exceeded. Please try again later.";
                case ApiErrorType.Network:
                    return "Network connectivity issue. Please check your connection and try again.";
                case ApiErrorType.NotFound:
                    return "The requested resource was not found.";
                case ApiErrorType.Server:
                    return "The cryptocurrency data service is currently unavailable. Please try again later.";
                default:
                    return "An error occurred while fetching cryptocurrency data. Please try again later.";
            }
        }

        private ApiErrorType DetermineErrorType(HttpRequestException ex)
        {
            return ex.StatusCode switch
            {
                HttpStatusCode.NotFound => ApiErrorType.NotFound,
                HttpStatusCode.TooManyRequests => ApiErrorType.RateLimit,
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => ApiErrorType.Authentication,
                HttpStatusCode.InternalServerError or HttpStatusCode.BadGateway or
                HttpStatusCode.ServiceUnavailable or HttpStatusCode.GatewayTimeout => ApiErrorType.Server,
                null => ApiErrorType.Network,
                _ => ApiErrorType.General
            };
        }

        private CryptoCurrencyData MapResponseToCryptoCurrencyData(CoinGeckoMarketResponse response)
        {
            return new CryptoCurrencyData
            {
                Id = response.Id,
                Name = response.Name,
                Symbol = response.Symbol,
                Image = response.Image,
                CurrentPrice = response.CurrentPrice,
                MarketCap = response.MarketCap,
                MarketCapRank = response.MarketCapRank,
                PriceChangePercentage24h = response.PriceChangePercentage24h,
                CirculatingSupply = response.CirculatingSupply,
                TotalSupply = response.TotalSupply,
                LastUpdated = response.LastUpdated
            };
        }

        #region API Response Models

        private class CoinGeckoMarketResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("symbol")]
            public string Symbol { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("image")]
            public string Image { get; set; }

            [JsonPropertyName("current_price")]
            public decimal CurrentPrice { get; set; }

            [JsonPropertyName("market_cap")]
            public decimal MarketCap { get; set; }

            [JsonPropertyName("market_cap_rank")]
            public int MarketCapRank { get; set; }

            [JsonPropertyName("price_change_percentage_24h")]
            public decimal PriceChangePercentage24h { get; set; }

            [JsonPropertyName("circulating_supply")]
            public decimal CirculatingSupply { get; set; }

            [JsonPropertyName("total_supply")]
            public decimal? TotalSupply { get; set; }

            [JsonPropertyName("last_updated")]
            public DateTime LastUpdated { get; set; }
        }

        private class CoinGeckoDetailsResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("symbol")]
            public string Symbol { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("image")]
            public ImageData Image { get; set; }

            [JsonPropertyName("market_data")]
            public MarketDataInfo MarketInfo { get; set; }

            [JsonPropertyName("last_updated")]
            public DateTime LastUpdated { get; set; }

            public class ImageData
            {
                [JsonPropertyName("large")]
                public string Large { get; set; }
            }

            public class MarketDataInfo
            {
                [JsonPropertyName("current_price")]
                public PriceData CurrentPrice { get; set; }

                [JsonPropertyName("market_cap")]
                public PriceData MarketCap { get; set; }

                [JsonPropertyName("market_cap_rank")]
                public int MarketCapRank { get; set; }

                [JsonPropertyName("price_change_percentage_24h")]
                public decimal PriceChangePercentage24h { get; set; }

                [JsonPropertyName("circulating_supply")]
                public decimal CirculatingSupply { get; set; }

                [JsonPropertyName("total_supply")]
                public decimal? TotalSupply { get; set; }
            }

            public class PriceData
            {
                [JsonPropertyName("usd")]
                public decimal Usd { get; set; }
            }
        }

        #endregion
    }
}