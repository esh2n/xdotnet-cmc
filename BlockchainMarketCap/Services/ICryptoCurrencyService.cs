using BlockchainMarketCap.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockchainMarketCap.Services
{
    /// <summary>
    /// Defines the interface for cryptocurrency data retrieval
    /// </summary>
    public interface ICryptoCurrencyService
    {
        /// <summary>
        /// Gets the top cryptocurrencies by market cap
        /// </summary>
        /// <param name="limit">Maximum number of cryptocurrencies to return</param>
        /// <returns>ApiResult containing a list of cryptocurrency data or error information</returns>
        Task<ApiResult<IEnumerable<CryptoCurrencyData>>> GetTopCryptocurrenciesAsync(int limit = 50);

        /// <summary>
        /// Gets detailed information for a specific cryptocurrency
        /// </summary>
        /// <param name="id">The ID of the cryptocurrency</param>
        /// <returns>ApiResult containing detailed cryptocurrency data or error information</returns>
        Task<ApiResult<CryptoCurrencyData>> GetCryptocurrencyDetailsAsync(string id);
    }
}