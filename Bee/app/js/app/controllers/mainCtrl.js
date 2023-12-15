app.controller('mainCtrl', ['$scope', '$http', '$routeParams', '$location', '$timeout', function ($scope, $http, $routeParams, $location, $timeout) {
    $scope.countries = ["Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua & Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia & Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre & Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts & Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad & Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks & Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States (USA)", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe"];
    $scope.user = {};
    $scope.pageId = 0;
    $scope.id = (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id'));
    $scope.token = (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token'));
    $scope.pageInfo = {};
    if ($scope.id) {
        $http({
            method: 'POST',
            url: '../api/bee/init/' + $scope.id,
            data: '"' + (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')) + '"',
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function (response) {
                console.log(response);
                $scope.isMember = response.data.IsMember;
                $scope.user = response.data.User;
                $scope.user.Birthdate = new Date(Date.parse($scope.user.Birthdate));
                $scope.user.avatar = '../users/' + $scope.user.Id + '/avatar.png?rand=' + Math.random();
                $scope.friendNotif = response.data.FriendNotif;
            }, function (error) {
                alert(error.data.Message);
            });
    }
    $scope.search = function (term) {
        if ($('.txt-search').val().length >= 3) {
            $('.search-results').show();
            $http({
                method: 'POST',
                url: '../api/bee/search/' + $routeParams.id,
                data: '"' + $('.txt-search').val() + '"',
                headers: {
                    "Content-Type": "application/json"
                }
            })
                .then(function (response) {
                    console.log(response);
                    $scope.results = response.data.Results;
                }, function (error) {
                    alert(error.data.Message);
                });
        } else {
            $scope.results = null;
            $('.search-results').hide();
        }
    };
    $scope.$on("$routeChangeSuccess", function () {
        $scope.isOwner = (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')) === $routeParams.id;
        $scope.pageId = $routeParams.id;
        $scope.isNews = $routeParams.category;
        $http({
            method: 'POST',
            url: '../api/bee/pageinfo/' + $routeParams.id
        })
            .then(function (response) {
                $scope.pageInfo = response.data.User;
                $scope.pageInfo.avatar = '../users/' + $scope.pageInfo.Id + '/avatar.png?rand=' + Math.random();
                $('.preloader').fadeOut(150);
            }, function (error) {
                $('.preloader').fadeOut(150);
                alert(error.data.Message);
            });
    });
    $scope.saveProfileSettings = function () {
        $http({
            method: 'POST',
            url: '../api/bee/updateprofilesettings/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                City: $('#city').val(),
                Country: $('#country').val(),
                CityOfBirth: $('#city-ob').val(),
                CountryOfBirth: $('#country-ob').val(),
                Birthdate: $('#birthdate').val() + ' 00:00:00',
                Occupation: $('#occupation').val(),
                Work: $('#work').val(),
                College: $('#college').val(),
                School: $('#school').val(),
                Relationship: $('#relationship').val(),
                Website: $('#website').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $('.msg').fadeIn(150).delay(1500).fadeOut(150);
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.savePrivacySettings = function () {
        $http({
            method: 'POST',
            url: '../api/bee/updateprivacysettings/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                PrivatePosts: $('#privatePosts').val(),
                PrivateFriends: $('#privateFriends').val(),
                PrivatePictures: $('#privatePictures').val(),
                PrivateCV: $('#privateCV').val(),
                PrivateEmail: $('#privateEmail').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $('.msg').fadeIn(150).delay(1500).fadeOut(150);
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.saveSettings = function () {
        $http({
            method: 'POST',
            url: '../api/bee/updatesettings/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                FirstName: $('#first').val(),
                LastName: $('#last').val(),
                Email: $('#email').val(),
                ReEmail: $('#reemail').val(),
                Password: $('#pass').val(),
                RePassword: $('#repass').val(),
                IsInvisible: $('#isInvisible').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function (response) {
                if (localStorage.getItem('token')) {
                    localStorage.setItem('token', response.data.token);
                } else if (sessionStorage.getItem('token')) {
                    sessionStorage.setItem('token', response.data.token);
                }
                $('.msg').fadeIn(150).delay(1500).fadeOut(150);
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.goToPage = function (page) {
        $location.url($scope.id + '/' + page);
    };
    $scope.goToUser = function (userId) {
        $location.url(userId + '/');
        $scope.results = null;
    };
    $scope.resetResults = function () {
        $timeout(function () {
            $scope.results = null;
        }, 1500);
    };
    $scope.addFriend = function (evt) {
        $http({
            method: 'POST',
            url: '../api/bee/addfriend/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                UserId: $routeParams.id,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).html('<i class="material-icons">supervised_user_circle</i>Connected');
            }, function (error) {
                alert(error.data.Message);
            });
    };
}]);