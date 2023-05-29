angular.module('app')

    .controller('sellerAddMovieCtrl', ['$scope', '$http', function ($scope, $http) {
        $http({
            method: "GET",
            url: "/Genres"
        }).then(function success(response) {
            if (response.data.Status = true) {
                $scope.genres = response.data.Genres;
            }
        }, function error(response) {
            console.error(response.statusText);
        });
    }]);