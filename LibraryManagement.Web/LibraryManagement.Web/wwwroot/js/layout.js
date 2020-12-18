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
                if (response.data.userId != null) {
                    $('#loginUser').modal('hide');
                    bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                        if (response.data.roleName == 'System Admin') {
                            window.location.href = "/User/Index";
                        } else {
                            window.location.href = "/LoanCard/Index";
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

$(document).ready(function () {
    $.ajax({
        url: `/book/getsCategory`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            $.each(response.data, function (i, v) {
                $('#dataCategory').append(
                    ` <li><a href="/Home/GetBookByCategory/${v.categoryId}">${v.categoryName}</a></li>`
                );
            });
        }
     });
});