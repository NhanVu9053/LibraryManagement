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
                localStorage.setItem("token", response.data.token);
                console.log("token");
                console.log(localStorage.getItem("token"));
                if (response.data.userId != null) {
                    $('#loginUser').modal('hide');
                    bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                        //if (response.data.roleName == 'System Admin') {
                        //    //console.log(response);
                        //    //localStorage.setItem("token", response.data.token);
                        //    //console.log(localStorage.getItem("token"));
                        //    window.location.href = "/User/Index";
                        //} else {
                        //    console.log(response);
                        //    localStorage.setItem("token", response.data.token);
                        //    console.log(localStorage.getItem("token"));
                        //    window.location.href = "/LoanCard/Index";
                        //}
                        console.log('ok');
                        $.ajax({
                            url: `https://localhost:44367/api/user/gets`,
                            method: 'GET',
                            dataType: 'JSON',
                             headers: {
                                 Authorization: 'Bearer ' + localStorage.getItem("token")
                             },
                            contentType: 'application/json',
                            success: function (data) {
                                console.log(data);
                            }
                        });
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