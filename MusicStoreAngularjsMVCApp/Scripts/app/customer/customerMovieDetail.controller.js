angular.module('app', ['angular.chosen'])

.controller('customerMovieDetailCtrl', function ($scope, $http) {
    let vm = this;

    $http({
        method: "GET",
        url: "/Movies?id=24"
    }).then(function success(response) {
        vm.movie = response.data.Movie;
    }, function error(response) {
        console.error(response.statusText);
    });
});