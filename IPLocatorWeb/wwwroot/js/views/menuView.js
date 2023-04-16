class MenuView {
    _parentElement = document.querySelector('.menu');

    addHandlerRender(handler) {
        this._parentElement.addEventListener('click', function (e) {
            if (!e.target.href) return;
            
            // gets a link that is stored in anchor tags. These link is used for targeting the query type
            e.preventDefault();
            const [_, targetSearch] = e.target.href.split('#');
            handler(targetSearch);
        })
    }

}
export default new MenuView();