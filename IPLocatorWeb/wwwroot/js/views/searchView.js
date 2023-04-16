class SearchView {
    _parentElement = document.querySelector('.search');

    getQuery() {
        const val = this._parentElement.querySelector('.search__field').value;
        this._clearInput();
        return val;
    }

    _clearInput() {
        this._parentElement.querySelector('.search__field').value = '';
    }

    updateView(searchType) {
        const searchField = this._parentElement.querySelector('.search__field');
        switch(searchType)
        {
            case 'ip':
                searchField.setAttribute("pattern", "^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)(\\.(?!$)|$)){4}$"); 
                searchField.setAttribute("placeholder", `Введите IP адрес для поиска локаций`); 
                break;
            case 'city':
            default:
                if (searchField.hasAttribute('pattern')) {
                    searchField.removeAttribute('pattern');
                }
                searchField.setAttribute("placeholder", `Введите город для поиска локаций`); 
                break;
        }
    }

    addHandlerRender(handler) {
        this._parentElement.addEventListener('submit', function (e) {
            e.preventDefault();
            handler();
        })
    }

}
export default new SearchView();