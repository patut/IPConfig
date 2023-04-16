import View from "./view.js";
import previewView from "./previewView.js";


class ResultsView extends View {
    _parentElement = document.querySelector('.results');
    _errorMessage = 'По введенному запросу локации не найдены';
    _message = '';

    _generateMarkup() {
        const rows = this._data.map(result => previewView.render(result, false)).join('');
        return this._getTableMarkup(rows);
    }

    _getTableMarkup(rows) {
        // generating table markup for viewing results
        return `
        <table class="results-table">
            <caption>Найденные локации</caption>
            <thead>
                <tr>
                    <th>Страна</th>
                    <th>Регион</th>
                    <th>Индекс</th>
                    <th>Город</th>
                    <th>Организация</th>
                    <th>Широта</th>
                    <th>Долгота</th>
                </tr>
            </thead>
            <tbody>
            ${rows}
            </tbody>
        </table>`
    }
    clear() {
        this._clear();
    }
}
export default new ResultsView();