angular.module('app')

    .controller('customerCartCtrl', ['$scope', '$http', 'orderByFilter', function ($scope, $http, orderBy) {
    $http({
        method: "GET",
        url: "/Customer/Carts"
    }).then(function success(response) {
        $scope.carts = orderBy(response.data.Carts, 'Movie.Seller.SellerId');

        let quantity = 1;
        for (let i = 0; i < $scope.carts.length - 1; i++) {
            if ($scope.carts[i].Movie.Id == $scope.carts[i + 1].Movie.Id) {
                quantity++;

                //for last iteration
                if (i == $scope.carts.length - 2) {
                    $scope.carts[i].Movie.Quantity = quantity;
                }
            }
            else {
                $scope.carts[i - quantity + 1].Movie.Quantity = quantity;
                quantity = 1;
            }
        }
        console.log($scope.carts)
    }, function error(response) {
        console.error(response.statusText);
    });
}]);