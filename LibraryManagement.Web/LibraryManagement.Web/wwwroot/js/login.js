var login = {} || login;

login.resetForm = function () {
    $('#loginUser').modal('hide');
    $('#fromLoginUser').trigger('reset');
    $('#msgLoginResult').hide();
    $('#fromLoginUser').validate().resetForm();
}
login.openModal = function () {
    login.resetForm();
    $('#loginUser').modal('show');
}

login.save = function () {
    $('#msgLoginResult').hide();
    if ($('#fromLoginUser').valid()) {
        var saveObj = {};
        saveObj.email = $('#EmailLogin').val();
        saveObj.password = $('#LoginPassword').val();
            $.ajax({
                url: '/user/login',
                method: 'POST',
                dataType: 'JSON',
                contentType: 'application/json',
                data: JSON.stringify(saveObj),
                success: function (response) {
                    debugger;
                    if (response.data.userId != null) {
                        console.log(response);
                        localStorage.setItem("token", response.data.token);
                        console.log(localStorage.getItem("token"));
                        $('#loginUser').modal('hide');
                        bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                            if (response.data.roleName == 'System Admin') {

                                console.log(response);
                                localStorage.setItem("token", response.data.token);
                                console.log(localStorage.getItem("token"));

                                //window.location.href = "/User/Index";
                            } else {

                                console.log(response);
                                localStorage.setItem("token", response.data.token);
                                console.log(localStorage.getItem("token"));

                                //window.location.href = "/LoanCard/Index";
                            }
                        });
                    }
                    else {
                        $('#msgLoginResult').text(`${response.data.message}`);
                        $('#msgLoginResult').show();
                    }
                }
            });
    }
}