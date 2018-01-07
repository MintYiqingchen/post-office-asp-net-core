function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return decodeURI(r[2]); return null; //返回参数值
}
$(document).ready(function () {
    $(".ui.sticky").sticky({
        context: '#content-box'
    });
});

// 筛选逻辑部分
var app = angular.module("myApp", []);
app.controller('myFilter', function ($scope, $http) {
    fm = $scope.filtermap = {
        'content': new Array(),
        'periodic': new Array()
    };

    $scope.items = new Array();
    $scope.addCondition = function (myEvent, kind) {
        let v = myEvent.target.innerText;
        let idx = $scope.filtermap[kind].indexOf(v);
        if (idx === -1) {
            $scope.filtermap[kind].push(v);
        }
        $('#content-box>div').hide();
        for (let a in fm['content']) {
            let temp = fm['content'][a].split("/");
            console.log('#content-box>div[class*=' + temp[0] + ']');
            $('#content-box>div[class*=' + temp[0] + ']').show();
        }
        for (let a in fm['periodic']) {
            $('#content-box>div.' + fm['content'][a]).show();
        }
    };
    $scope.deleteCondition = function (myEvent, kind) {
        let v = myEvent.target.parentNode.innerText;
        let idx = $scope.filtermap[kind].indexOf(v);
        $scope.filtermap[kind].splice(idx, 1);
        if (fm['content'].length > 0 || fm['periodic'].length > 0) {
            $('#content-box>div').hide();
            for (let a in fm['content']) {
                let temp = fm['content'][a].split("/");
                $('#content-box>div[class*=' + temp[0] + ']').show();
            }
            for (let a in fm['periodic']) {
                $('#content-box>div[class*=' + temp[0] + ']').show();
            }
        }
        else {
            $scope.clearTags();
        }
    };
    $scope.clearTags = function () {
        $scope.filtermap['content'] = new Array();
        $scope.filtermap['periodic'] = new Array();
        $('#content-box>div').show();
    };
});