var manageUsersPage = (function () {
    function init($container) {
        let $banBtn = $container.find('.ban-btn'),
            $makeAdminBtn = $container.find('.make-admin-btn'),
            $removeAdminBtn = $container.find('.remove-admin-btn'),
            $makeWorkerBtn = $container.find('.make-worker-btn'),
            $removeWorkerBtn = $container.find('.remove-worker-btn');

        $banBtn.click(function () {
            var userEmail = $(this).closest('tr').find('td:eq(2)').text();

            var btnClickedUserId = $(this).attr('id');

            Swal.fire({
                title: 'WARNING?',
                text: "Are you sure you want to ban "+ userEmail+" ?",
                input: 'text',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                inputPlaceholder: "What is the reason for the ban",
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Ban!'
            }).then((result) => {
                if (result.isConfirmed && result.value.trim()) {
                    commonFuncs.showLoader();
                    $.post('/Admin/BanUser', { userId: btnClickedUserId, reasonOfBanning: result.value.trim() }, function (response) {
                        
                    });
                    commonFuncs.hideLoader();
                    location.reload();
                }
            })
        });

        $makeAdminBtn.click(function () {
            var userEmail = $(this).closest('tr').find('td:eq(2)').text();

            Swal.fire({
                title: 'WARNING?',
                text: "Are you sure you want to make " + userEmail + " an admin?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes!'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: 'Do you want to save the changes?',
                        showDenyButton: true,
                        showCancelButton: true,
                        confirmButtonText: 'Save',
                        denyButtonText: `Don't save`,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            commonFuncs.showLoader();
                            $.post('/Admin/MakeAdmin', { userId: $(this).attr('id') }, function (response) {
                                commonFuncs.hideLoader();
                                location.reload();
                            }).fail(function (error) {
                                commonFuncs.hideLoader();
                                alert('AJAX request failed: ', error);
                            });
                        } else if (result.isDenied) {
                            commonFuncs.hideLoader();
                            Swal.fire('Changes are not saved', '', 'info');
                        }
                    })

                }
            })
        });

        $removeAdminBtn.click(function () {
            var userEmail = $(this).closest('tr').find('td:eq(2)').text();
            Swal.fire({
                title: 'WARNING?',
                text: "Are you sure you want to remove "+userEmail+"'s admin rights?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes!'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: 'Do you want to save the changes?',
                        showDenyButton: true,
                        showCancelButton: true,
                        confirmButtonText: 'Save',
                        denyButtonText: `Don't save`,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            commonFuncs.showLoader();
                            $.post('/Admin/RemoveAdmin', { userId: $(this).attr('id') }, function (response) {
                                commonFuncs.hideLoader();
                                location.reload();
                            }).fail(function (error) {
                                commonFuncs.hideLoader();
                                alert('AJAX request failed: ', error);
                            });
                        } else if (result.isDenied) {
                            commonFuncs.hideLoader();
                            Swal.fire('Changes are not saved', '', 'info');
                        }
                    })

                }
            })
        });

        $makeWorkerBtn.click(function () {
            var userEmail = $(this).closest('tr').find('td:eq(2)').text();

            Swal.fire({
                title: 'WARNING?',
                text: "Are you sure you want to make " + userEmail + " a worker?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes!'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: 'Enter the full name job title:',
                        input: 'text',
                        inputPlaceholder: 'Full Name',
                        showCancelButton: true,
                        confirmButtonText: 'Submit',
                    }).then((inputResult) => {
                        if (inputResult.isConfirmed && inputResult.value) {
                            const fullName = inputResult.value.trim();
                            commonFuncs.showLoader();
                            $.post('/Admin/MakeWorker', { userId: $(this).attr('id'), position: fullName }, function (response) {
                                commonFuncs.hideLoader();
                                location.reload();
                            }).fail(function (error) {
                                commonFuncs.hideLoader();
                                alert('AJAX request failed: ' + error.responseText);
                            });
                        } else {
                            commonFuncs.hideLoader();
                            Swal.fire('Operation canceled', '', 'info');
                        }
                    });
                }
            });
        });



        $removeWorkerBtn.click(function () {
            var userEmail = $(this).closest('tr').find('td:eq(2)').text();
            Swal.fire({
                title: 'WARNING?',
                text: "Are you sure you want to remove " + userEmail + "'s worker position?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes!'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: 'Do you want to save the changes?',
                        showDenyButton: true,
                        showCancelButton: true,
                        confirmButtonText: 'Save',
                        denyButtonText: `Don't save`,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            commonFuncs.showLoader();
                            $.post('/Admin/RemoveWorker', { userId: $(this).attr('id') }, function (response) {
                                commonFuncs.hideLoader();
                                location.reload();
                            }).fail(function (error) {
                                commonFuncs.hideLoader();
                                alert('AJAX request failed: ', error);
                            });
                        } else if (result.isDenied) {
                            commonFuncs.hideLoader();
                            Swal.fire('Changes are not saved', '', 'info');
                        }
                    })

                }
            })
        });
    }
    return {
        init
    };
})();