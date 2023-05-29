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

        $scope.modifySelectedId = function (id) {
            $scope.selectedId = id;
        }

        $scope.deleteMovie = function () {
            $.ajax({
                url: "/Seller/DeleteMovie/" + $scope.selectedId,
                type: 'post',
                success: function (data) {
                    window.location.reload();
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    }]);