angular.module('app')

    .controller('adminIndexCtrl', ['$scope', '$http', function ($scope, $http) {
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

        $scope.changeActionButtonInModal = function (action, id, filteredCriteria) {
            $('#actionButton')
                .html(action)
                .off().on('click', () => {
                    $.ajax({
                        url: '/Admin/' + action + '/' + id,
                        type: 'POST',
                        success: function (data) {
                            $scope.approvalList.forEach(item => {
                                if (item.Id == id) {
                                    item.Status = (action === 'Approve') ? 1 : 2;
                                }
                            });
                            filterListByCriteria(filteredCriteria);
                        },
                        error: function (error) {
                            console.error(error);
                        }
                    })
                })

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
    }]);