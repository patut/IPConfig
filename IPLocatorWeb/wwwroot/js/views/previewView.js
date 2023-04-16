import View from "./view.js";

class PreviewView extends View {
    _parentElement = '';

    _generateMarkup() {
        // const id = window.location.hash.slice(1);
        // {"country":"cou_ED","region":"reg_Uk Rur","postal":"pos_633983","city":"cit_A Or","organization":"org_Ired Q Filaqefufyt Tavym","latitude":76.3988,"longitude":75.7555}
        // Generating markup for the table tow
        return `
        <tr>
            <td>${this._data.country}</td>
            <td>${this._data.region}</td>
            <td>${this._data.postal}</td>
            <td>${this._data.city}</td>
            <td>${this._data.organization}</td>
            <td>${this._data.latitude}</td>
            <td>${this._data.longitude}</td>
        </tr>`
    }
}
export default new PreviewView();