var app = angular.module('app', ['ngRoute'])
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/:id/', {
                controller: 'profileCtrl',
                templateUrl: 'js/app/views/profile.html'
            })
            .when('/:id/start', {
                controller: 'startCtrl',
                templateUrl: 'js/app/views/start.html'
            })
            .when('/:id/friends', {
                controller: 'friendsCtrl',
                templateUrl: 'js/app/views/friends.html'
            })
            .when('/:id/pictures', {
                controller: 'picturesCtrl',
                templateUrl: 'js/app/views/pictures.html'
            })
            .when('/:id/news/:category', {
                controller: 'newsCtrl',
                templateUrl: 'js/app/views/news.html'
            })
            .when('/:id/jobs', {
                controller: 'jobsCtrl',
                templateUrl: 'js/app/views/jobs.html'
            })
            .when('/:id/games', {
                controller: 'gamesCtrl',
                templateUrl: 'js/app/views/games.html'
            })
            .when('/:id/messages', {
                controller: 'messagesCtrl',
                templateUrl: 'js/app/views/messages.html'
            })
            .when('/:id/groups', {
                controller: 'groupsCtrl',
                templateUrl: 'js/app/views/groups.html'
            })
            .when('/:id/cv', {
                controller: 'cvCtrl',
                templateUrl: 'js/app/views/cv.html'
            });
    }])
    .filter('unsafe', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    });