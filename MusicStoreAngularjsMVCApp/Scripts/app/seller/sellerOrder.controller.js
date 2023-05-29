angular.module('app')

    .controller('sellerOrderCtrl', ['$scope', '$http', function ($scope, $http) {
        fetchOrders = function () {
            $http({
                method: "GET",
                url: "/Orders"
            }).then(function success(response) {
                if (response.data.Status = true) {
                    $scope.orders = response.data.Orders;
                }
            }, function error(response) {
                console.error(response.statusText);
            });
        };

        fetchOrders();

        $scope.updateOrderStatus = function (id, orderStatus) {
            $http({
                method: "post",
                url: '/Seller/Order/' + id,
                data: {
                    orderStatus: orderStatus
                }
            }).then(function success(response) {
                if (response.data.Status = true) {
                    fetchOrders();
                }
            }, function error(response) {
                console.error(response.statusText);
            });
        }
    }]);