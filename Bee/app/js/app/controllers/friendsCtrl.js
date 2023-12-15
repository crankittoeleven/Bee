app.controller('friendsCtrl', ['$scope', '$http', '$routeParams', '$route', function ($scope, $http, $routeParams, $route) {
    $scope.friends = {};
    $scope.friends.accepted = [];
    $scope.friends.pending = [];
    $http({
        method: 'POST',
        url: '../api/bee/friends/' + $routeParams.id,
        data: '"' + (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')) + '"',
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(function (response) {
            console.log(response);
            response.data.Friends.forEach(function (item, index) {
                if (item.To.Id.toString() === $routeParams.id && item.Status === 'pending') {
                    $scope.friends.pending.push(item);
                } else if (item.Status === 'accepted'){
                    $scope.friends.accepted.push((item.To.Id.toString() === $routeParams.id ? item.From : item.To));
                }
            });
            console.log($scope);
        }, function (error) {
            alert(error.data.Message);
        });
    $scope.acceptFriend = function (friendId) {
        $http({
            method: 'POST',
            url: '../api/bee/acceptfriend/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                UserId: friendId,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $route.reload();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.declineFriend = function (friendId) {
        $http({
            method: 'POST',
            url: '../api/bee/declinefriend/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                UserId: friendId,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $route.reload();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.deleteFriend = function (evt, friendId) {
        $http({
            method: 'POST',
            url: '../api/bee/deletefriend/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                UserId: friendId,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().fadeOut(150);
            }, function (error) {
                alert(error.data.Message);
            });
    };
}]);