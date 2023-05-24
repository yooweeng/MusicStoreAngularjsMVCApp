angular.module('app', ['localytics.directives'])

.controller('guestIndexCtrl', function ($scope, $http) {
    $http({
        method: "GET",
        url: "/Movies"
    }).then(function success(response) {
        $scope.genres = response.data.Data.Genres;
        $scope.movies = response.data.Data.Movies;
    }, function error(response) {
        console.error(response.statusText);
    });
});