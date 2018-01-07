// Write your JavaScript code.
'use strict'
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return decodeURI(r[2]); return null; //返回参数值
}
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
    
    $(document).keyup(function (event) {
        if (event.keyCode == 13) {
            $(location).attr("href", "/Home/items?q=" + searchTerm+"&k="+getUrlParam('k'));
        }
    });
}