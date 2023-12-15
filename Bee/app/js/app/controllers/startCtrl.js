app.controller('startCtrl', ['$scope', '$http', '$routeParams', '$route', '$location', '$compile', function ($scope, $http, $routeParams, $route, $location, $compile) {
    $scope.posts = [];
    var randomResult = Math.random();
    $http({
        method: 'GET',
        url: 'https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyCDc8S4btsJOLEFD4jVxqK9W5spaK3JFlc&address=London'
    })
        .then(function (response) {
            console.log(response);
        });
    $http({
        method: 'POST',
        url: '../api/bee/start/' + $routeParams.id,
        data: '"' + (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')) + '"',
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(function (response) {
            console.log(response);
            $scope.friendCount = response.data.FriendCount;
            $scope.pictureCount = response.data.PictureCount;
            $scope.postCount = response.data.PostCount;
            if ($scope.friendCount + $scope.pictureCount + $scope.postCount > 49) {
                $scope.level = 'hero';
            } else if ($scope.friendCount + $scope.pictureCount + $scope.postCount > 9) {
                $scope.level = 'babyface';
            } else {
                $scope.level = 'upcomming';
            }
            $scope.posts = response.data.Posts;
            $scope.groups = response.data.Groups;
            $scope.posts.forEach(function (item, index) {
                item.comments = response.data.Comments.filter(function (c) {
                    return c.Post.Id === item.Id;
                });
                item.Created = new Date(item.Created).toDateString() + ' ' + new Date(item.Created).toLocaleTimeString('en-US');
                if (item.Type === 'text') {
                    item.Content = '<article>' + item.Content.replaceAll('\n', '<br />') + '</article>';
                    var re = new RegExp('\\[loc=[^\\[]*\\/\\]');
                    var temp;
                    while ((temp = re.exec(item.Content)) !== null) {
                        var rand = Math.random();
                        console.log(temp[0]);
                        var toAppend = item.Content.replace(temp.input, '<div style="height:300px;" id="map' + rand.toString().replace('.', '') + '" data-map="' + temp[0].replace('[loc=', '').replace('/]', '') + '"></div>');
                        item.Content += toAppend;
                        item.Content = item.Content.replace(temp[0], '');
                        var renderMap = function (mapId) {
                            $http({
                                method: 'GET',
                                url: 'https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyCDc8S4btsJOLEFD4jVxqK9W5spaK3JFlc&address=' + temp[0].replace('[loc=', '').replace('/]', '')
                            })
                                .then(function (response) {
                                    console.log(rand);
                                    console.log(response);
                                    var map = new google.maps.Map(document.getElementById(mapId), {
                                        disableDefaultUI: true,
                                        center: { lat: response.data.results[0].geometry.location.lat, lng: response.data.results[0].geometry.location.lng },
                                        scrollwheel: false,
                                        zoom: 14
                                    });
                                    var marker = new google.maps.Marker({
                                        position: new google.maps.LatLng(response.data.results[0].geometry.location.lat, response.data.results[0].geometry.location.lng)
                                    });
                                    marker.setMap(map);
                                });
                        };
                        renderMap('map' + rand.toString().replace('.', ''));
                    }
                    item.Content = item.Content.replace(/\[\/url\]/g, '</a>').replace(/\[url=/g, '<a href="').replace(/\]/g, '" target="_blank">');
                } else if (item.Type === 'image') {
                    if (item.Content.indexOf('http') === 0) {
                        item.Content = '<img onclick="$(\'.preview\').fadeIn(150); $(\'.preview img\').attr(\'src\', $(this).attr(\'data-preview\'));" data-preview="' + item.Content + '" class="img-post" src="' + item.Content + '" alt="" style="cursor:pointer;" />';
                    } else {
                        item.Content = '<img onclick="$(\'.preview\').fadeIn(150); $(\'.preview img\').attr(\'src\', $(this).attr(\'data-preview\'));" data-preview="../users/' + item.Author.Id + '/' + item.Content + '" class="img-post" src="../users/' + item.Author.Id + '/' + item.Content + '?' + Math.random() + '" alt="" style="cursor:pointer;" />';
                    }
                } else if (item.Type === 'video') {
                    item.Content = '<iframe style="height:348px;width:100%;max-width:100%;border:0;" frameborder="0" src="https://www.youtube.com/embed/' + item.Content.replace('https://www.youtube.com/watch?v=', '') + '?hl=en&amp;autoplay=0&amp;cc_load_policy=0&amp;loop=0&amp;iv_load_policy=0&amp;fs=1&amp;showinfo=0"></iframe>';
                }
            });
        }, function (error) {
            alert(error.data.Message);
        });
    $scope.addPost = function () {
        $http({
            method: 'POST',
            url: '../api/bee/addpost/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                AuthorId: (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
                Group: ($('.slc-group').val() === '') ? null : $('.slc-group').val(),
                Content: $('.post').val(),
                Type: $('.post-type').val()
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
    $scope.addComment = function (evt, postId) {
        $http({
            method: 'POST',
            url: '../api/bee/addcomment/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                AuthorId: (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
                PostId: postId,
                Content: $(evt.target).find('.txt-comment').val()
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $('.frm-comment').fadeOut(150)
                $('.frm-comment').val('');
                $scope.posts.find(function (p) {
                    return p.Id === postId;
                }).comments.push({
                    Author: { Id: (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')), FirstName: $scope.user.FirstName },
                    PostId: postId,
                    Content: $(evt.target).find('.txt-comment').val().replace('<', '').replace('>', '')
                });
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.addLike = function (evt, postId) {
        $http({
            method: 'POST',
            url: '../api/bee/like/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                UserId: (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
                PostId: postId,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).attr('title', $(evt.target).attr('title') + 1);
            }, function (error) {
                
            });
    };
    $scope.deletePost = function (evt, postId) {
        $http({
            method: 'POST',
            url: '../api/bee/deletepost/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                PostId: postId,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                $(evt.target).closest('.item').fadeOut(150);
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.sharePost = function (postId) {
        $http({
            method: 'POST',
            url: '../api/bee/sharepost/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')),
            data: {
                token: (localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token')),
                PostId: postId,
            },
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(function () {
                location.href = '../app/#!/' + (localStorage.getItem('id') ? localStorage.getItem('id') : sessionStorage.getItem('id')) + '/start';
            }, function (error) {
                alert(error.data.Message);
            });
    };
    $scope.getRandom = function () {
        return randomResult;
    };
}]);