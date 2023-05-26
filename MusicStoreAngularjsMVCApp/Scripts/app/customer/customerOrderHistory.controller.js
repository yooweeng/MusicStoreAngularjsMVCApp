angular.module('app')

    .controller('customerOrderHistoryCtrl', ['$scope', '$http', '$window', function ($scope, $http, $window) {
        // get order status
        address = $window.location.search;
        parameterList = new URLSearchParams(address);
        orderStatus = parameterList.get('orderStatus');

        $scope.btnColorPending = "btn-secondary"
        $scope.btnColorShipping = "btn-secondary"
        $scope.btnColorDelivered = "btn-secondary"

        if (orderStatus == 'pending') {
            $scope.orderStatusTextColor = "text-success";
            $scope.btnColorPending = "btn-warning"
        }
        else if (orderStatus == 'shipping') {
            $scope.orderStatusTextColor = "text-info";
            $scope.btnColorShipping = "btn-warning"
        }
        else {
            $scope.orderStatusTextColor = "text-warning";
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
    }]);