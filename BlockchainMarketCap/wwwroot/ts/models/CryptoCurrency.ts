/**
 * Represents a cryptocurrency with market data
 */
export interface CryptoCurrency {
	/** Unique identifier for the cryptocurrency */
	id: string;

	/** Ticker symbol (e.g., BTC, ETH) */
	symbol: string;

	/** Full name of the cryptocurrency */
	name: string;

	/** URL to the cryptocurrency's icon */
	image: string;

	/** Current price in USD */
	currentPrice: number;

	/** Total market capitalization in USD */
	marketCap: number;

	/** Market cap rank compared to other cryptocurrencies */
	marketCapRank: number;

	/** 24-hour price change percentage */
	priceChangePercentage24h: number;

	/** Number of coins in circulation */
	circulatingSupply: number;

	/** Maximum supply of coins (if applicable) */
	totalSupply: number | null;

	/** Last time the data was updated */
	lastUpdated: string;
}
