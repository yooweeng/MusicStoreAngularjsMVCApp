angular.module('app')

    .controller('sellerIndexCtrl', ['$scope', '$http', function ($scope, $http) {
        $http({
            method: "GET",
            url: "/Seller/Movies"
        }).then(function success(response) {
            $scope.movies = response.data.Movies;
        }, function error(response) {
            console.error(response.statusText);
        });
    }]);