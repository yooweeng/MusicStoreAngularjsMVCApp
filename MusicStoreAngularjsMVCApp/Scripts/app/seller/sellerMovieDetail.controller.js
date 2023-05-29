﻿angular.module('app')

    .controller('sellerMovieDetailCtrl', ['$scope', '$http', '$window', function ($scope, $http, $window) {
        // get id
        address = $window.location.search;
        parameterList = new URLSearchParams(address);
        id = parameterList.get('id');

        $http({
            method: "GET",
            url: "/Seller/Movies/" + id
        }).then(function success(response) {
            $scope.movie = response.data.Movie;
        }, function error(response) {
            console.error(response.statusText);
        });
    }]);