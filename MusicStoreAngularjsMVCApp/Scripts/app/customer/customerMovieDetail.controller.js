angular.module('app')

    .controller('customerMovieDetailCtrl', function ($scope, $http) {
        // get id

        $http({
            method: "GET",
            url: "/Movies?id=24"
        }).then(function success(response) {
            $scope.movie = response.data.Movie;
        }, function error(response) {
            console.error(response.statusText);
        });
    });