using System;

namespace BlockchainMarketCap.Models
{
    /// <summary>
    /// Generic wrapper for API responses that includes success/failure status and error information
    /// </summary>
    /// <typeparam name="T">The type of data returned by the API</typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// The data returned by the API (null if there was an error)
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Indicates whether the API request was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if there was an error, otherwise null
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Error type for categorizing different errors
        /// </summary>
        public ApiErrorType ErrorType { get; set; }

        /// <summary>
        /// Creates a successful result with data
        /// </summary>
        public static ApiResult<T> Success(T data)
        {
            return new ApiResult<T>
            {
                Data = data,
                IsSuccess = true
            };
        }

        /// <summary>
        /// Creates a failure result with an error message
        /// </summary>
        public static ApiResult<T> Failure(string errorMessage, ApiErrorType errorType = ApiErrorType.General)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorType = errorType
            };
        }

        /// <summary>
        /// Creates a failure result from an exception
        /// </summary>
        public static ApiResult<T> Failure(Exception ex, ApiErrorType errorType = ApiErrorType.General)
        {
            return Failure(ex.Message, errorType);
        }
    }

    /// <summary>
    /// Enum representing different types of API errors
    /// </summary>
    public enum ApiErrorType
    {
        /// <summary>
        /// General error
        /// </summary>
        General,

        /// <summary>
        /// Network error (connectivity issues)
        /// </summary>
        Network,

        /// <summary>
        /// API rate limit exceeded
        /// </summary>
        RateLimit,

        /// <summary>
        /// Authentication error
        /// </summary>
        Authentication,

        /// <summary>
        /// Resource not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Server error
        /// </summary>
        Server,

        /// <summary>
        /// Request timeout
        /// </summary>
        Timeout
    }
}