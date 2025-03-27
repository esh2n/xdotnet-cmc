<template>
  <div class="crypto-table-container">
    <div class="controls">
      <h1>Blockchain Market Cap</h1>
      <div class="refresh-info">
        <button @click="refreshData" class="refresh-button">
          Refresh Data
        </button>
        <span v-if="lastUpdated" class="last-updated">
          Last updated: {{ formatLastUpdated(lastUpdated) }}
        </span>
      </div>
    </div>

    <div v-if="loading" class="loading">Loading cryptocurrency data...</div>

    <table v-else class="crypto-table">
      <thead>
        <tr>
          <th
            @click="sortBy('marketCapRank')"
            :class="{ active: sortColumn === 'marketCapRank' }"
          >
            Rank
            <span v-if="sortColumn === 'marketCapRank'" class="sort-indicator">
              {{ sortDirection === "asc" ? "▲" : "▼" }}
            </span>
          </th>
          <th>Name</th>
          <th
            @click="sortBy('currentPrice')"
            :class="{ active: sortColumn === 'currentPrice' }"
          >
            Price (USD)
            <span v-if="sortColumn === 'currentPrice'" class="sort-indicator">
              {{ sortDirection === "asc" ? "▲" : "▼" }}
            </span>
          </th>
          <th
            @click="sortBy('marketCap')"
            :class="{ active: sortColumn === 'marketCap' }"
          >
            Market Cap
            <span v-if="sortColumn === 'marketCap'" class="sort-indicator">
              {{ sortDirection === "asc" ? "▲" : "▼" }}
            </span>
          </th>
          <th
            @click="sortBy('priceChangePercentage24h')"
            :class="{ active: sortColumn === 'priceChangePercentage24h' }"
          >
            24h Change
            <span
              v-if="sortColumn === 'priceChangePercentage24h'"
              class="sort-indicator"
            >
              {{ sortDirection === "asc" ? "▲" : "▼" }}
            </span>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="crypto in sortedCryptocurrencies" :key="crypto.id">
          <td>{{ crypto.marketCapRank }}</td>
          <td class="crypto-name">
            <img :src="crypto.image" :alt="crypto.name" class="crypto-icon" />
            <span>{{ crypto.name }}</span>
            <span class="crypto-symbol">{{ crypto.symbol.toUpperCase() }}</span>
          </td>
          <td>${{ formatNumber(crypto.currentPrice) }}</td>
          <td>${{ formatNumber(crypto.marketCap) }}</td>
          <td :class="getPriceChangeClass(crypto.priceChangePercentage24h)">
            {{ formatNumber(crypto.priceChangePercentage24h) }}%
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, onMounted, ref } from "vue";
import type { CryptoCurrency } from "../models/CryptoCurrency";
import {
  getCryptocurrencyData,
  refreshCryptocurrencyData,
} from "../services/apiService";

export default defineComponent({
  name: "CryptoCurrencyTable",

  setup() {
    const cryptocurrencies = ref<CryptoCurrency[]>([]);
    const lastUpdated = ref<Date | null>(null);
    const loading = ref(true);
    const sortColumn = ref<keyof CryptoCurrency>("marketCapRank");
    const sortDirection = ref<"asc" | "desc">("asc");

    // Initialize with existing data and listen for updates
    onMounted(() => {
      const initialData = getCryptocurrencyData();
      cryptocurrencies.value = initialData.data;
      lastUpdated.value = initialData.lastUpdated;
      loading.value = false;

      document.addEventListener("cryptocurrency-data-updated", ((
        event: CustomEvent
      ) => {
        cryptocurrencies.value = event.detail.data;
        lastUpdated.value = event.detail.timestamp;
        loading.value = false;
      }) as EventListener);
    });

    // Computed property for sorted cryptocurrencies
    const sortedCryptocurrencies = computed(() => {
      return [...cryptocurrencies.value].sort((a, b) => {
        let valueA = a[sortColumn.value];
        let valueB = b[sortColumn.value];

        if (valueA === null || valueA === undefined) valueA = 0;
        if (valueB === null || valueB === undefined) valueB = 0;

        if (typeof valueA === "string") valueA = valueA.toLowerCase();
        if (typeof valueB === "string") valueB = valueB.toLowerCase();

        if (sortDirection.value === "asc") {
          return valueA > valueB ? 1 : -1;
        }
        return valueA < valueB ? 1 : -1;
      });
    });

    // Change sorting
    const sortBy = (column: keyof CryptoCurrency) => {
      if (sortColumn.value === column) {
        sortDirection.value = sortDirection.value === "asc" ? "desc" : "asc";
      } else {
        sortColumn.value = column;
        sortDirection.value = "desc";
      }
    };

    // Manually refresh data
    const refreshData = async () => {
      loading.value = true;
      await refreshCryptocurrencyData();
    };

    // Format number with commas and limit decimals
    const formatNumber = (num: number): string => {
      if (num > 1) {
        return num.toLocaleString("en-US", {
          maximumFractionDigits: 2,
          minimumFractionDigits: 2,
        });
      }
      return num.toLocaleString("en-US", {
        maximumFractionDigits: 6,
        minimumFractionDigits: 2,
      });
    };

    // Format the last updated timestamp
    const formatLastUpdated = (date: Date): string => {
      return date.toLocaleString();
    };

    // Get CSS class for price change cells
    const getPriceChangeClass = (change: number): string => {
      return change >= 0 ? "positive-change" : "negative-change";
    };

    return {
      cryptocurrencies,
      lastUpdated,
      loading,
      sortColumn,
      sortDirection,
      sortedCryptocurrencies,
      sortBy,
      refreshData,
      formatNumber,
      formatLastUpdated,
      getPriceChangeClass,
    };
  },
});
</script>

<style>
/* スタイルはCSSファイルに移動しました */
</style>
