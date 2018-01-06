// Write your JavaScript code.
'use strict'

$(document).ready(function () {
    $(".ui.dropdown").dropdown({ on: "hover" });
    $.fn.search.settings.error.noResults = '搜索不到结果';
    $.fn.search.settings.onSearchQuery = searchKey;
    $(".ui.search").search({
        apiSettings: { url: '/Home/search/?q={query}' },
        minCharacters:2
    });

});

function searchKey(searchTerm) {
    console.log(searchTerm);
    
    $(document).keyup(function (event) {
        if (event.keyCode == 13) {
            $(location).attr("href", "/Test?q=" + searchTerm);
        }
    });
}