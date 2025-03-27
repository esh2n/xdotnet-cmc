# Technical Context: Blockchain Market Cap Application

## Technologies Used

### Backend
- **C# 8.0**: Primary backend language
- **ASP.NET Core 3.1+**: Web application framework
- **Blazor**: Server-side rendering framework

### Frontend
- **TypeScript**: Type-safe JavaScript for frontend logic
- **JavaScript**: Core frontend scripting
- **Vue.js**: Frontend framework for interactivity
- **SCSS**: CSS preprocessor for styling

### Build Tools
- **Gulp**: Task automation
- **webpack**: Module bundling
- **Node.js**: JavaScript runtime for build processes
- **npm**: Package management

## Development Setup

### Required Software
- **.NET SDK**: Version 5.0 or higher
- **Node.js**: Version 14.x or higher
- **npm**: Version 6.x or higher
- **Visual Studio 2019** or **VS Code** with C# extensions

### Development Environment Setup
```
# Install .NET SDK (if not already installed)
# Download from https://dotnet.microsoft.com/download

# Install Node.js and npm (if not already installed)
# Download from https://nodejs.org/

# Clone repository (when applicable)
git clone [repository-url]

# Restore .NET packages
dotnet restore

# Install Node.js dependencies
npm install

# Build the application
dotnet build
npm run build

# Run the application
dotnet run
```

## Technical Constraints

### API Limitations
- **Rate Limits**: Most cryptocurrency APIs have rate limits that must be respected
- **Data Freshness**: API data may have slight delays (typically 1-5 minutes)
- **API Key Requirements**: Some APIs may require registration and API keys

### Browser Compatibility
- **Modern Browsers**: Primary support for Chrome, Firefox, Safari, Edge
- **IE Support**: Limited or no support for Internet Explorer
- **Mobile Browsers**: Must function correctly on iOS Safari and Android Chrome

### Performance Requirements
- **Initial Load Time**: Under 2 seconds on broadband connection
- **Sorting Operations**: Near-instantaneous (under 200ms)
- **Memory Usage**: Efficient DOM manipulation to prevent memory leaks

## Dependencies

### .NET Packages
- **Microsoft.AspNetCore.App**: Core ASP.NET functionality
- **Microsoft.AspNetCore.Blazor**: Blazor framework
- **System.Net.Http.Json**: JSON handling for HTTP requests
- **Microsoft.Extensions.Http**: HTTP client factory

### NPM Packages
- **vue**: Vue.js core library
- **axios**: HTTP client for API requests
- **sass**: SCSS compiler
- **gulp**: Task automation
- **webpack**: Module bundling
- **typescript**: TypeScript compiler

### External APIs
- **CoinGecko API**: Primary data source for cryptocurrency information
- **Alternative APIs**: CoinMarketCap, CryptoCompare (as fallbacks)

## Security Considerations
- **CORS**: Proper configuration for API requests
- **CSP**: Content Security Policy implementation
- **No Sensitive Data**: Application does not handle user accounts or sensitive information
- **API Key Protection**: If API keys are required, they should not be exposed to the client-side