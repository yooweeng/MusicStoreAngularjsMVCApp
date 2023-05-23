angular.module('app', [])
.controller('movieCtrl', function ($http) {
    let vm = this;
    $http({
        method: "GET",
        url: "/Movies"
    }).then(function success(response) {
        vm.genres = response.data.Data.Genres;
        vm.movies = response.data.Data.Movies;
    }, function error(response) {
        console.error(response.statusText);
    });
});