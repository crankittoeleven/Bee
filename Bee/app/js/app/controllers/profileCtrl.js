app.controller('profileCtrl', ['$scope', '$http', '$routeParams', '$location', function ($scope, $http, $routeParams, $location) {
    $scope.profile = {};
    $scope.friends = [];
    $scope.pictures = [];
    var randomResult = Math.random();
    $http({
        method: 'POST',
        url: '../api/bee/profile/' + $routeParams.id,
    })
        .then(function (response) {
            console.log(response);
            $scope.profile = response.data.User;
            $scope.friends = response.data.Friends;
            $scope.pictures = response.data.Pictures;
            $scope.postCount = response.data.PostCount;
            $scope.friendCount = response.data.FriendCount;
            if ($scope.friendCount + $scope.pictures.length + $scope.postCount > 49) {
                $scope.level = 'hero';
            } else if ($scope.friendCount + $scope.pictures.length + $scope.postCount > 9) {
                $scope.level = 'babyface';
            } else {
                $scope.level = 'upcomming';
            }
        }, function (error) {
            alert(error.data.Message);
        });
    $scope.goToFriend = function (friendId) {
        $location.url(friendId);
    };
    $scope.definition = function (term) {
        $http({
            method: 'POST',
            url: '../api/bee/getdefinition/' + $routeParams.id,
            data: '"' + term + '"',
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function (response) {
                console.log(response);
                $('.popup3').find('h3').text(term);
                $('.popup3').find('p').html('No definition available.');
                $('.popup3').find('p').html(function () {
                    var temp = JSON.parse(response.data);
                    for (var key in temp.query.pages) {
                        if (temp.query.pages.hasOwnProperty(key)) {
                            return temp.query.pages[key].extract;
                        }
                    }
                });
                $('.popup3').fadeIn(150);
            });
    };
    $scope.getRandom = function () {
        return randomResult;
    };
}]);