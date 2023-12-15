app.controller('groupsCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.groups = [];
    $http({
        method: 'POST',
        url: '../api/bee/groups/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
        data: '"' + (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')) + '"',
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(function (response) {
            for (var g in response.data.Groups) {
                $scope.groups.unshift({
                    title: g,
                    count: response.data.Groups[g],
                    image: g.toLowerCase().replace(/ /g, '') + '.jpg'
                });
            }
            response.data.Member.forEach(function (item, index) {
                $scope.groups.find(function (g) {
                    return g.title === item;
                }).isMember = true;
            });
            console.log($scope.groups);
        }, function (error) {
            alert(error.data.Message);
        });
    $scope.joinGroup = function (groupTitle) {
        $http({
            method: 'POST',
            url: '../api/bee/joingroup/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                Title: groupTitle,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $scope.groups.find(function (item) {
                    return item.title === groupTitle;
                }).isMember = true;
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.exitGroup = function (groupTitle) {
        $http({
            method: 'POST',
            url: '../api/bee/exitgroup/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                Title: groupTitle,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $scope.groups.find(function (item) {
                    return item.title === groupTitle;
                }).isMember = false;
            }, function (error) {
                alert(error.data.Message);
            });
    };
}]);