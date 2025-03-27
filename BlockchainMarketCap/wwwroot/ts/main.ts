import { createApp } from "vue";
import CryptoCurrencyTable from "./components/CryptoCurrencyTable.vue";
import { setupApiService } from "./services/apiService";

// Initialize the API service
setupApiService();

// Mount the Vue application
document.addEventListener("DOMContentLoaded", () => {
	const mountPoint = document.getElementById("crypto-table-app");

	if (mountPoint) {
		const app = createApp(CryptoCurrencyTable);
		app.mount(mountPoint);
		console.log("Vue app mounted successfully");
	} else {
		console.error("Mount point not found: #crypto-table-app");
	}
});
