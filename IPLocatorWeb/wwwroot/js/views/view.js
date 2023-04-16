// import icons from 'url:../../img/icons.svg'
import {ICONS_URL} from '../config.js';

export default class View {
    _data;

    render(data, render = true) {
        if (!data || (Array.isArray(data) && data.length === 0)) return this.renderError();

        this._data = data;

        const markup = this._generateMarkup();

        if (!render) return markup;

        this._clear();
        this._parentElement.insertAdjacentHTML('afterbegin', markup);
    }

    renderSpinner() {
        const markup = `
      <div class="spinner">
        <svg>
          <use href="${ICONS_URL}#icon-loader"></use>
        </svg>
      </div>`;
        this._clear();
        this._parentElement.insertAdjacentHTML('afterbegin', markup);
    }

    renderError(message = this._errorMessage) {
        const markup = `<div class="error">
        <div>
          <svg>
            <use href="${ICONS_URL}#icon-alert-triangle"></use>
          </svg>
        </div>
        <p>${message}</p>
      </div>`;
        this._clear();
        this._parentElement.insertAdjacentHTML('afterbegin', markup);
    }

    _clear() {
        this._parentElement.innerHTML = '';
    }
}