angular.module('app')

.controller('customerMovieDetailCtrl', ['$scope', '$http', function ($scope, $http) {
    // get id
    address = window.location.search;
    parameterList = new URLSearchParams(window.location.search);
    id = parameterList.get('id');

    $http({
        method: "GET",
        url: "/Movies",
        params: { id: id }
    }).then(function success(response) {
        $scope.movie = response.data.Movie;
    }, function error(response) {
        console.error(response.statusText);
    });
}]);