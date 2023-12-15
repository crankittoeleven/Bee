app.controller('messagesCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.friends = [];
    $scope.messages = [];
    $scope.friendId = 0;
    var msgInterval = {};
    $http({
        method: 'POST',
        url: '../api/bee/messages/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
        data: '"' + (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')) + '"',
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(function (response) {
            console.log(response);
            $scope.friends = response.data.Friends;
            $scope.friends.forEach(function (item, index) {
                item.hasNew = response.data.HaveNew.includes(item.Id);
            });
        }, function (error) {
            alert(error.data.Message);
        });
    $scope.$on('$locationChangeStart', function (event) {
        clearInterval(msgInterval);
    });
    $scope.getMessages = function (userId) {
        $scope.friendId = userId;
        clearInterval(msgInterval);
        msgInterval = setInterval(function () {
            $http({
                method: 'POST',
                url: '../api/bee/getmessages/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
                data: {
                    token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                    UserId: userId,
                    Contunt: null
                },
                headers: {
                    "Content-Type": "application/json"
                }
            })
                .then(function (response) {
                    console.log(response);
                    $scope.messages = response.data.Messages;
                }, function (error) {
                    alert(error.data.Message);
                });
        }, 2500);
    };
    $scope.addMessage = function (evt) {
        evt.preventDefault();
        $http({
            method: 'POST',
            url: '../api/bee/addmessage/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                UserId: $scope.friendId,
                Content: $(evt.target).find('input').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function (response) {
                $(evt.target).find('input').val('');
            }, function (error) {
                alert(error.data.Message);
            });
    };
}]);