import searchView from "./views/searchView.js";
import resultsView from './views/resultsView.js';
import * as model from './model.js';
import menuView from "./views/menuView.js";

const controlSearchResults = async function () {
    try {
        resultsView.renderSpinner();

        // 1) Get search query
        const query = searchView.getQuery();
        if (!query) return;

        // 2) Load search results
        await model.loadSearchResults(query, model.state.searchType);

        // 3) Render results
        resultsView.render(model.state.search.results);

    } catch (err) {
        console.err(err);
    }
}

const controlMenuClicks = async function (targetSearchType) {
    try {

       model.state.searchType = targetSearchType;
       searchView.updateView(model.state.searchType);
       resultsView.clear();

    } catch (err) {
        console.err(err);
    }
}

const init = function () {
    searchView.addHandlerRender(controlSearchResults);
    menuView.addHandlerRender(controlMenuClicks);
    searchView.updateView(model.state.searchType);
    resultsView.clear();
};

init();