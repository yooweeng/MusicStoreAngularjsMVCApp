angular.module('app')

    .controller('customerCartCtrl', ['$scope', '$http', '$window', 'orderByFilter', function ($scope, $http, $window, orderBy) {
        let selectedMovieIds = [];
        $scope.total = 0;
        $scope.selectedMovies = [];
        $scope.isSelectedMovie = {};

        $http({
            method: "GET",
            url: "/Customer/Carts"
        }).then(function success(response) {
            $scope.carts = orderBy(response.data.Carts, 'Movie.Seller.SellerId');

            let quantity = 1;
            //for first item since loop maybe not be executed
            $scope.carts[0].Movie.Quantity = quantity;
            for (let i = 0; i < $scope.carts.length - 1; i++) {
                if ($scope.carts[i].Movie.Id == $scope.carts[i + 1].Movie.Id) {
                    quantity++;

                    //for last iteration
                    if (i == $scope.carts.length - 2) {
                        $scope.carts[i - quantity + 2].Movie.Quantity = quantity;
                    }
                }
                else {
                    $scope.carts[i - quantity + 1].Movie.Quantity = quantity;
                    quantity = 1;

                    //for last iteration
                    if (i == $scope.carts.length - 2) {
                        $scope.carts[i + 1].Movie.Quantity = 1;
                    }
                }
            }
            console.log($scope.carts)
        }, function error(response) {
            console.error(response.statusText);
        });

        $scope.checkMovie = function (movieId, movieTitle, price, quantity, isChecked) {
            if (isChecked) {
                $scope.selectedMovies.push({
                    movieId: movieId,
                    movieTitle: movieTitle,
                    quantity: quantity
                });
                selectedMovieIds.push(movieId);
                $scope.total += price * quantity;
            }
            else {
                for (let i = 0; i < $scope.selectedMovies.length; i++) {
                    if (JSON.stringify($scope.selectedMovies[i]) == JSON.stringify({
                        movieId: movieId,
                        movieTitle: movieTitle,
                        quantity: quantity
                    })) {
                        $scope.selectedMovies.splice(i, 1);
                    }
                }

                const index = selectedMovieIds.indexOf(movieId);
                if (index > -1) {
                    selectedMovieIds.splice(index, 1);
                }

                $scope.total -= price * quantity;
            }
        }

        $scope.increaseQuantity = function (movieId) {
            $http({
                url: '/Customer/Carts',
                method: 'post',
                data: {
                    "MovieId": movieId
                }
            }).then(function success(data) {
                $window.location.reload();
            },
                function error(error) {
                    console.error(error);
                }
            );
        }

        $scope.decreaseQuantity = function (movieId) {
            $http({
                url: '/Customer/RemoveCarts',
                method: 'post',
                data: {
                    "MovieId": movieId
                }
            }).then(function success(data) {
                $window.location.reload();
            },
                function error(error) {
                    console.error(error);
                }
            );
        }

        $scope.checkoutMovies = function () {
            $http({
                url: '/Customer/Checkout',
                method: 'post',
                data: {
                    selectedMovieIds: selectedMovieIds
                }
            }).then(function success(data) {
                $window.location.replace('/Customer/Checkout/' + data.data.OrderId);
            },
                function error(error) {
                    console.error(error);
                }
            );
        }
    }]);