import { API_URL } from './config.js';
import { AJAX } from './helpers.js';

export const state = {
    searchType: 'ip',
    search: {
        query: '',
        results: []
    },
};

export const loadSearchResults = async function (query, type) {
    try {
        // 1) Loading locations
        const url = type == 'ip' ? `${API_URL}ip/location?ip=${query}` : `${API_URL}city/locations?city=${query}`;
        const data = await AJAX(url);

        state.search.query = query;
        state.search.results = type == 'ip' ? [data] : data;
    }
    catch (err) {
        // Temporaty error handling
        throw err;
    }
}