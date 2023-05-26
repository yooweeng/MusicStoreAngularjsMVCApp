angular.module('app')

    .controller('adminIndexCtrl', ['$scope', '$http', '$window', function ($scope, $http, $window) {
        let selectedApprovalItemId;
        $scope.statusMapping = ['Pending', 'Approve', 'Reject'];

        $http({
            method: "GET",
            url: "/Admin/ApprovalList"
        }).then(function success(response) {
            $scope.approvalList = response.data.ApprovalList;
            $scope.filterListByCriteria(-1);
        }, function error(response) {
            console.error(response.statusText);
        });

        $scope.filterListByCriteria = function (filterCriteria) {
            $scope.filterCriteria = filterCriteria;
            $scope.filteredList = $scope.approvalList.filter(item => { return item.Status === filterCriteria });

            if (filterCriteria === -1) {
                $scope.filteredList = $scope.approvalList;
            }

            $('.filter-btn')
                .removeClass('btn-dark')
                .addClass('btn-secondary');
            $('#button' + filterCriteria)
                .removeClass('btn-secondary')
                .addClass('btn-dark');
        }

        $scope.changeActionButtonInModal = function (action, id) {
            $scope.action = action;
            selectedApprovalItemId = id;

            if (action === 'Approve') {
                $('#actionButton')
                    .addClass('btn-success')
                    .removeClass('btn-danger')
            }
            else if (action === 'Reject') {
                $('#actionButton')
                    .addClass('btn-danger')
                    .removeClass('btn-success')
            }
        }

        $scope.changeApprovalStatus = function () {
            $http({
                method: "POST",
                url: "/Admin/" + $scope.action + "/" + selectedApprovalItemId
            }).then(function success(response) {
                if (response.data.Status == true) {
                    $window.location.reload();
                }
            }, function error(response) {
                console.error(response.statusText);
            });
        }
    }]);