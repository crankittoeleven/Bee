app.controller('picturesCtrl', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
    $scope.pictures = [];
    $http({
        method: 'POST',
        url: '../api/bee/pictures/' + $routeParams.id,
        data: '"' + (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')) + '"',
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(function (response) {
            $scope.pictures = response.data.Pictures;
        }, function (error) {
            alert(error.data.Message);
        });
    $scope.deletePicture = function (evt, fileName) {
        $http({
            method: 'POST',
            url: '../api/bee/deletepicture/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                FileName: fileName,
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
    $scope.preview = function (url) {
        $('.preview').fadeIn(150);
        $('.preview img').attr('src', url);
    };
}]);