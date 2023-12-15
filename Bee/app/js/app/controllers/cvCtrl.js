app.controller('cvCtrl', ['$scope', '$http', '$routeParams', '$route', function ($scope, $http, $routeParams, $route) {
    $scope.work = [];
    $scope.education = [];
    $scope.skills = [];
    $scope.languages = [];
    $http({
        method: 'POST',
        url: '../api/bee/cv/' + $routeParams.id,
        data: '"' + (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')) + '"',
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(function (response) {
            console.log(response);
            $scope.work = response.data.Work;
            $scope.work.forEach(function (item, index) {
                item.From = new Date(item.From).toDateString();
                item.To = new Date(item.To).toDateString();
            });
            $scope.education = response.data.Education;
            $scope.education.forEach(function (item, index) {
                item.From = new Date(item.From).toDateString();
                item.To = new Date(item.To).toDateString();
            });
            $scope.skills = response.data.Skills;
            $scope.languages = response.data.Languages;
        }, function (error) {
            alert(error.data.Message);
        });
    $scope.addWork = function (evt) {
        $http({
            method: 'POST',
            url: '../api/bee/addwork/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                Title: $('#work-title').val(),
                From: $('#work-from').val() + ' 00:00:00',
                To: $('#work-to').val() + ' 00:00:00',
                Company: $('#work-company').val(),
                Size: $('#work-size').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().hide();
                $route.reload();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.deleteWork = function (evt, workId) {
        $http({
            method: 'POST',
            url: '../api/bee/deletework/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                WorkId: workId
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().parent().hide();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.addEducation = function (evt) {
        $http({
            method: 'POST',
            url: '../api/bee/addeducation/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                Title: $('#edu-title').val(),
                From: $('#edu-from').val() + ' 00:00:00',
                To: $('#edu-to').val() + ' 00:00:00',
                Institute: $('#edu-institute').val(),
                Description: $('#edu-description').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().hide();
                $route.reload();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.deleteEducation = function (evt, educationId) {
        $http({
            method: 'POST',
            url: '../api/bee/deleteeducation/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                EducationId: educationId
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().parent().hide();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.addSkill = function (evt) {
        $http({
            method: 'POST',
            url: '../api/bee/addskill/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                Title: $('#skills-title').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().hide();
                $route.reload();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.deleteSkill = function (evt, skillId) {
        $http({
            method: 'POST',
            url: '../api/bee/deleteskill/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                SkillId: skillId
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().hide();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.addLanguage = function (evt) {
        $http({
            method: 'POST',
            url: '../api/bee/addlanguage/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                Title: $('#lang-title').val(),
                Level: $('#lang-level').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().hide();
                $route.reload();
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.deleteLanguage = function (evt, languageId) {
        $http({
            method: 'POST',
            url: '../api/bee/deletelanguage/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                LanguageId: languageId
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).parent().parent().hide();
            }, function (error) {
                alert(error.data.Message);
            });
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
}]);