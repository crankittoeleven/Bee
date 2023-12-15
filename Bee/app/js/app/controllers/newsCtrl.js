app.controller('newsCtrl', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
    $scope.news = [];
    $http({
        method: 'GET',
        url: '../api/bee/getnews?category=' + $routeParams.category
    })
        .then(function (response) {
            response.data.articles.forEach(function (item, index) {
                if (item.urlToImage) {
                    $scope.news.push(item);
                }
            });
        });
}]);