var layoutAdmin = {} || layoutAdmin;

layoutAdmin.logOut = function () {
    $.ajax({
        url: '/user/logOut',
        method: 'POST',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            if (response.data.isSuccess) {
                window.location.href = "/Home";
            }
            else {
                bootbox.alert(`<h5 class="text-success">Đăng xuất không thành công !!!</h5>`)
            }
        }
    });
}