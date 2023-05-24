angular.module('app', ['localytics.directives'])

.controller('customerIndexCtrl', ['$scope', '$http', function ($scope, $http) {
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
}]);