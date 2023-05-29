angular.module('app')

    .controller('customerOrderHistoryCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.getOrderHistory = function (orderStatus) {
            $scope.btnColorPending = "btn-secondary"
            $scope.btnColorShipping = "btn-secondary"
            $scope.btnColorDelivered = "btn-secondary"
            
            if (orderStatus == 'pending') {
                $scope.orderStatusTextColor = "text-warning";
                $scope.btnColorPending = "btn-warning"
            }
            else if (orderStatus == 'shipping') {
                $scope.orderStatusTextColor = "text-info";
                $scope.btnColorShipping = "btn-warning"
            }
            else if (orderStatus == 'delivered') {
                $scope.orderStatusTextColor = "text-success";
                $scope.btnColorDelivered = "btn-warning"
            }

            $http({
                method: "GET",
                url: "/OrderHistories",
                params: { orderStatus: orderStatus }
            }).then(function success(response) {
                if (response.data.Status) {
                    $scope.orderHistories = response.data.OrderHistories;
                }
            }, function error(response) {
                console.error(response.statusText);
            });
        }

        // default order status
        $scope.getOrderHistory('pending');
    }]);