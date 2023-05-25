angular.module('app')

.controller('customerIndexCtrl', ['$scope', '$http', function ($scope, $http) {
    let movieId;

    $http({
        method: "GET",
        url: "/Movies"
    }).then(function success(response) {
        $scope.genres = response.data.Data.Genres;
        $scope.movies = response.data.Data.Movies;

        $scope.filteredMovies = $scope.movies;
    }, function error(response) {
        console.error(response.statusText);
    });

    $scope.filteredMoviesBySelectedGenreTypes = function () {
        $scope.filteredMovies = $scope.movies;

        if ($scope.selectedGenreTypes.length > 0) {
            let filteredArr = [];

            $scope.movies.forEach(movie => {
                movie.GenreTypes.forEach(genreType => {
                    if ($scope.selectedGenreTypes.indexOf(genreType) > -1) {
                        return filteredArr.push(movie);
                    }
                });
            });

            $scope.filteredMovies = filteredArr;
        }
    };

    $scope.setCurrentSelectedMovie = function (id) {
        movieId = id;
    }

    $scope.addToCart = function () {
        $http({
            method: 'POST',
            url: '/Customer/Carts',
            data: {
                "MovieId": movieId
            }
        })
        .then(function success(response) {
            bootstrap.Toast.getOrCreateInstance($('#liveToast')).show();
            bootstrap.Modal.getInstance($('#confirmAddToCartModal')).hide();
        }, function error(response) {
            console.error(response.statusText);
        });
    }
}]);