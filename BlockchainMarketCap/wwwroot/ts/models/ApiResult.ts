/**
 * Enum representing different types of API errors
 */
export enum ApiErrorType {
	/** General error */
	General = "general",

	/** Network error (connectivity issues) */
	Network = "network",

	/** API rate limit exceeded */
	RateLimit = "rateLimit",

	/** Authentication error */
	Authentication = "authentication",

	/** Resource not found */
	NotFound = "notFound",

	/** Server error */
	Server = "server",

	/** Request timeout */
	Timeout = "timeout",
}

/**
 * Interface representing the result of an API operation
 */
export interface ApiResult<T> {
	/** The data returned by the API (null if there was an error) */
	data: T | null;

	/** Indicates whether the API request was successful */
	isSuccess: boolean;

	/** Error message if there was an error, otherwise null */
	errorMessage: string | null;

	/** Error type for categorizing different errors */
	errorType: ApiErrorType | null;
}

/**
 * Creates a successful API result with data
 */
export function createSuccessResult<T>(data: T): ApiResult<T> {
	return {
		data,
		isSuccess: true,
		errorMessage: null,
		errorType: null,
	};
}

/**
 * Creates a failure API result with an error message
 */
export function createFailureResult<T>(
	errorMessage: string,
	errorType: ApiErrorType = ApiErrorType.General,
): ApiResult<T> {
	return {
		data: null,
		isSuccess: false,
		errorMessage,
		errorType,
	};
}

/**
 * Interface for error with response and request properties
 */
interface ApiError {
	response?: {
		status: number;
	};
	request?: unknown;
	code?: string;
	message?: string;
}

/**
 * Creates a failure API result from an Error object
 */
export function createFailureResultFromError<T>(
	error: ApiError,
	defaultMessage = "An unexpected error occurred",
): ApiResult<T> {
	let errorType = ApiErrorType.General;
	let errorMessage = defaultMessage;

	if (error.response) {
		// Server responded with error
		const status = error.response.status;

		if (status === 401 || status === 403) {
			errorType = ApiErrorType.Authentication;
			errorMessage = "Authentication error";
		} else if (status === 404) {
			errorType = ApiErrorType.NotFound;
			errorMessage = "Resource not found";
		} else if (status === 429) {
			errorType = ApiErrorType.RateLimit;
			errorMessage = "API rate limit exceeded";
		} else if (status >= 500) {
			errorType = ApiErrorType.Server;
			errorMessage = "Server error occurred";
		}
	} else if (error.request) {
		// Request made but no response received
		if (error.code === "ECONNABORTED") {
			errorType = ApiErrorType.Timeout;
			errorMessage = "Request timed out";
		} else {
			errorType = ApiErrorType.Network;
			errorMessage = "Network error";
		}
	}

	return createFailureResult<T>(errorMessage, errorType);
}
