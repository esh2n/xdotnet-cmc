# System Patterns: Blockchain Market Cap Application

## System Architecture
The application follows a hybrid architecture that combines server-side rendering with client-side interactivity:

```
┌─────────────────────────────────────────┐
│ Client Browser                          │
│                                         │
│  ┌─────────────┐     ┌───────────────┐  │
│  │ Blazor      │     │ Vue.js        │  │
│  │ Components  │◄────┤ Components    │  │
│  └─────────────┘     └───────────────┘  │
│         ▲                    ▲           │
└─────────┼────────────────────┼─────────┘
          │                    │
┌─────────┼────────────────────┼─────────┐
│         │                    │         │
│  ┌─────────────┐     ┌───────────────┐ │
│  │ ASP.NET Core│     │ Cryptocurrency│ │
│  │ Blazor      │     │ APIs          │ │
│  └─────────────┘     └───────────────┘ │
│                                        │
└────────────────────────────────────────┘
```

## Key Technical Decisions

### Backend Framework: ASP.NET Core Blazor
- **Reasoning**: Provides server-side rendering capabilities with C#, supporting efficient data processing
- **Implementation**: Hosts the main application, handles API calls to cryptocurrency services, and renders the initial HTML

### Frontend Framework: Vue.js
- **Reasoning**: Lightweight framework for enhanced client-side interactivity
- **Implementation**: Manages the sorting functionality and dynamic UI updates without full page refreshes

### Styling: SCSS
- **Reasoning**: Offers enhanced CSS capabilities with variables and nesting for maintainable styling
- **Implementation**: Organized in component-specific files that compile to optimized CSS

### Build System: Gulp & webpack
- **Reasoning**: Gulp for task automation, webpack for module bundling
- **Implementation**: Gulp orchestrates the build process, webpack handles JavaScript bundling and optimization

## Design Patterns

### Component Pattern
- Modular components that encapsulate specific functionality
- Each cryptocurrency entry is a reusable component

### Repository Pattern
- Abstracts data access logic for cryptocurrency API calls
- Centralizes API interaction and error handling

### Observer Pattern
- Implemented through Vue.js reactivity system
- Updates UI components when data changes

### Adapter Pattern
- Normalizes data from various cryptocurrency APIs into a consistent format
- Shields the application from API-specific implementation details

## Component Relationships

### Data Flow
1. **API Service** retrieves cryptocurrency data
2. **Data Adapter** normalizes the data into a consistent format
3. **Blazor Components** render the initial view with the data
4. **Vue.js Components** handle user interactions and sorting

### Dependency Hierarchy
- **API Service**: No dependencies within the application
- **Data Adapter**: Depends on API Service
- **View Components**: Depend on normalized data from Data Adapter
- **Sorting Components**: Depend on View Components for data access