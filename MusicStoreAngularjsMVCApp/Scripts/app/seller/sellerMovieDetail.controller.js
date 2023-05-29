angular.module('app')

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
            $scope.genres = response.data.Genres;
        }, function error(response) {
            console.error(response.statusText);
        });

        $scope.updateMovie = function () {
            $http({
                method: "post",
                url: "/Seller/EditMovie/" + id,
                data: {
                    movie: {
                        MovieTitle: $scope.movie.MovieTitle,
                        Description: $scope.movie.Description,
                        Price: $scope.movie.Price,
                        ReleasedYear: $scope.movie.ReleasedYear
                    },
                    selectedGenresId: $scope.movie.GenreIds
                }
            }).then(function success(response) {
                $window.location.replace('/Seller');
            }, function error(response) {
                console.error(response.statusText);
            });
        }
    }]);