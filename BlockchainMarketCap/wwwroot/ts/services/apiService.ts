import axios from "axios";
import type { CryptoCurrency } from "../models/CryptoCurrency";

// API configuration
const COINGECKO_API_URL = "https://api.coingecko.com/api/v3";
const FETCH_INTERVAL_MS = 60000; // 1 minute

// In-memory cache
let cryptocurrencyData: CryptoCurrency[] = [];
let lastUpdated: Date | null = null;

/**
 * Initializes the API service
 */
export function setupApiService(): void {
	// Initial fetch
	fetchCryptocurrencyData();

	// Set up interval for periodic updates
	setInterval(fetchCryptocurrencyData, FETCH_INTERVAL_MS);
}

/**
 * Fetches cryptocurrency data from the API
 */
async function fetchCryptocurrencyData(): Promise<void> {
	try {
		const response = await axios.get(`${COINGECKO_API_URL}/coins/markets`, {
			params: {
				vs_currency: "usd",
				order: "market_cap_desc",
				per_page: 50,
				page: 1,
				sparkline: false,
			},
		});

		if (response.status === 200) {
			cryptocurrencyData = mapApiResponseToModel(response.data);
			lastUpdated = new Date();

			// Dispatch custom event to notify components of new data
			const event = new CustomEvent("cryptocurrency-data-updated", {
				detail: {
					data: cryptocurrencyData,
					timestamp: lastUpdated,
				},
			});
			document.dispatchEvent(event);
		}
	} catch (error) {
		console.error("Error fetching cryptocurrency data:", error);
	}
}

/**
 * Maps the API response to our application model
 */
function mapApiResponseToModel(apiData: any[]): CryptoCurrency[] {
	return apiData.map((item) => ({
		id: item.id,
		symbol: item.symbol,
		name: item.name,
		image: item.image,
		currentPrice: item.current_price,
		marketCap: item.market_cap,
		marketCapRank: item.market_cap_rank,
		priceChangePercentage24h: item.price_change_percentage_24h,
		circulatingSupply: item.circulating_supply,
		totalSupply: item.total_supply,
		lastUpdated: item.last_updated,
	}));
}

/**
 * Gets the cached cryptocurrency data
 */
export function getCryptocurrencyData(): {
	data: CryptoCurrency[];
	lastUpdated: Date | null;
} {
	return {
		data: cryptocurrencyData,
		lastUpdated,
	};
}

/**
 * Manually triggers a refresh of the cryptocurrency data
 */
export async function refreshCryptocurrencyData(): Promise<void> {
	await fetchCryptocurrencyData();
}
