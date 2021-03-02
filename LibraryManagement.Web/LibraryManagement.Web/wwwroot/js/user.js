var user = {} || user;
var table = $('#tbUsers').DataTable();

user.showData = function () {
    $.ajax({
        url: '/user/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            table.clear().destroy();
            $('#tbUsers>tbody').empty();
            $.each(response.data, function (i, v) {
                if (v.statusName != 'Deleted') {
                    var actions = "";
                    switch (v.statusId) {
                        case 1:
                            {
                                actions = `<a href='javascript: void(0)' class='text-dark ml-2' onclick="user.changeStatusToBlocked('${v.userId}','${v.email}')" title='Khóa'><i class='fa fa-ban'></i></a>`;
                                break;
                            }
                        case 2:
                            {
                                actions = `<a href='javascript: void(0)' class='text-success ml-2' onclick="user.changeStatusToActive('${v.userId}','${v.email}')" title='Hoạt dộng'><i class='far fa-play-circle'></i></a>`;
                                actions += `<a href='javascript: void(0)' class='text-danger ml-2' onclick="user.delete('${v.userId}','${v.email}','${v.email}')" title='Xóa'><i class='fas fa-trash'></i></a>`;
                                break;
                            }
                    }
                    $('#tbUsers>tbody').append(
                        `<tr>
                        <td>${v.email}</td>
                        <td>${v.fullName}</td>
                        <td>${v.phoneNumber}</td>
                        <td>${(v.gender == true ? "Nam" : "Nữ")}</td>
                        <td>${v.roleName}</td>
                        <td class="text-center">
                            <span class="${(v.statusId == 1 ? "btn btn-primary" : (v.statusId == 2 ? "btn btn-warning" : "btn btn-info"))}" style="width: 115px; height: 40px;">
                                ${v.statusName}
                            </span>
                        </td>
                        <td class="text-center">
                            <img src="/img/${v.avatarPath}" style="width: 55px; height: 60px;" />
                        </td>
                        <td>
                            <a href="javascript:void(0)"  onclick="user.details('${v.userId}')" class="text-primary ml-2" title="Chi tiết"><i class="fas fa-eye"></i></a>
                            <a href="javascript:void(0)" class="text-warning ml-2" onclick="user.edit('${v.userId}')" title="Cập nhật"><i class="fas fa-edit"></i></a>
                            ${actions}
                        </td>
                    </tr>`
                    );
                }
            });
            user.drawDataTable();
        }
    });
}

user.details = function (userId) {
    $('#dataModal').empty();
    $.ajax({
        url: `/user/get/${userId}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            if (response.data.userId != null) {
                $('#dataModalTitle').text('THÔNG TIN TÀI KHOẢN');
                $('#dataModal').append(
                    `<h5 class="text-info text-center m-2">Tên đăng nhập: ${response.data.email}</h5>
                    <br />
                    <div class="row justify-content-center col-xl-12">
                        <div class="col-xl-6 col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <p><b>Mã tài khoản: </b> ${response.data.userId}</p>
                                    <p><b>Họ và tên: </b> ${response.data.fullName}</p>
                                    <p><b>Giới tính: </b> ${(response.data.gender == true ? "Nam" : "Nữ")}</p>
                                    <p><b>Ngày sinh: </b> ${response.data.dobStr}</p>
                                    <p><b>Số điện thoại: </b> ${response.data.phoneNumber}</p>
                                    <p><b>Ngày bắt đầu làm: </b> ${response.data.hireDateStr}</p>
                                    <p><b>Chức vụ: </b>${response.data.roleName}</p>
                                    <p><b>Địa chỉ: </b>${response.data.addressStr}</p>
                                    <p><b>Trạng thái: </b> ${response.data.statusName}</p>
                                    <p><b>Ngày tạo: </b> ${response.data.createdDateStr}</p>
                                    <p><b>Người tạo: </b> ${response.data.createdByName}</p>
                                    <p><b>Ngày cập nhật: </b> ${response.data.modifiedDateStr}</p>
                                    <p><b>Người cập nhật: </b> ${response.data.modifiedByName}</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-6 col-md-12 text-center">
                            <img src="/img/${response.data.avatarPath}" class="m-2" style="width: 300px; height: 400px;" />
                        </div>
                    </div>`
                );
                $('#detailsData').modal('show');
            }
            else {
                bootbox.alert(`<h5 class="text-danger">Tài khoản này không tồn tại !!!</h5>`)
            }
        }
    });
}

//---------------- Check Save -----------------
user.checkEmail = function () {
    var email = $('#Email').val();
    var em_regex = /^\w+([\.-]?\w+)+@\w+([\.:]?\w+)+(\.[a-zA-Z0-9]{2,3})+$/;
    if (em_regex.test(email) == false) {
        $("#msgEmail").text('Email không đúng định dạng');
        $("#msgEmail").show();
    }
    else {
        $("#msgEmail").hide();
    }
}

user.checkConfirmPassword = function () {
    var password = $('#Password').val();
    var confirmPassword = $('#ConfirmPassword').val();
    if (password != confirmPassword) {
        $("#msgConfirmPassword").text('Xác nhận mật khẩu không khớp');
        $("#ConfirmPassword-error").hide();
        $("#msgConfirmPassword").show();
    }
    else {
        $("#msgConfirmPassword").hide();
    }
}
user.checkPassword = function () {
    var pw_regex = /^(?=.*?[0-9])(?=.*?[A-Z])(?=.*?[#?!@$%^&*\-_]).{6,}$/;
    var password = $('#Password').val();
    if (pw_regex.test(password) == false) {
        $("#msgPassword").text('Mật khẩu phải ít nhất 6 ký tự, có chữ cái hoa, có chữ thường và số');
        $("#msgPassword").show();
        $("#Password-error").hide();
    }
    else {
        $("#msgPassword").hide();
    }
}

user.checkPhone = function () {
    var vnf_regex = /((09|03|07|08|05)+([0-9]{8}))$/;
    var mobile = $('#PhoneNumber').val();
    if (vnf_regex.test(mobile) == false) {
        $("#msgPhone").text('Số điện thoại không đúng định dạng');
        $("#msgPhone").show();
        $("#Phone-error").hide();
    }
    else {
        $("#msgPhone").hide();
    }
}


user.checkSave = function () {
    let checkPw = false;
    let checkCpw = false;
    let checkP = false;
    let checkE = false;
    if ($('#fromAddEditUser').valid()) {
        var password = $('#Password').val();
        var confirmPassword = $('#ConfirmPassword').val();
        if (password == confirmPassword) {
            checkCpw = true;
        }
        else {
            checkCpw = false;
            $("#msgConfirmPasswword").show();
        }
        var pw_regex = /^(?=.*?[0-9])(?=.*?[A-Z])(?=.*?[#?!@$%^&*\-_]).{6,}$/;
        var password = $('#Password').val();
        if (pw_regex.test(password)) {
            checkPw = true;
        }
        else {
            checkPw = false;
            $("#msgPassword").show();
        }
        var vnf_regex = /((09|03|07|08|05)+([0-9]{8}))$/;
        var mobile = $('#PhoneNumber').val();
        if (vnf_regex.test(mobile)) {
            checkP = true;
        }
        else {
            checkP = false;
            $("#msgPhone").show();
        }
        var email = $('#Email').val();
        var em_regex = /^\w+([\.-]?\w+)+@\w+([\.:]?\w+)+(\.[a-zA-Z0-9]{2,3})+$/;
        if (em_regex.test(email)) {
            checkE = true;
        }
        else {
            $("#msgEmail").show();
            checkE = false;
        }
    }
    var userId = $('#UserId').val();
    if (userId == '') {
        if (checkPw && checkCpw && checkP && checkE) {
            user.save();
        } else {
            user.checkConfirmPassword();
            user.checkPassword();
            user.checkPhone();
            user.checkEmail();
        }
    } else {
        if (checkP && checkE) {
            user.save();
        } else {
            user.checkPhone();
            user.checkEmail();
        }
    }
}

//---------------- Edit -----------------
user.edit = function (id) {
    $.ajax({
        url: `/user/get/${id}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            user.resetForm();
            user.initDistricts(response.data.provinceId, response.data.districtId);
            user.initWards(response.data.districtId, response.data.wardId);
            $('#UserId').val(response.data.userId);
            $('#AvatarPath').val(response.data.avatarPath);
            $('#FullName').val(response.data.fullName);
            $('#Gender').val(`${response.data.gender}`);
            var dobString = response.data.dob.toString();
            $('#Dob').val(dobString.slice(0, 10));
            var hireDateString = response.data.dob.toString();
            $('#HireDate').val(hireDateString.slice(0, 10));
            $('#Email').val(response.data.email);
            $('#Roles').val(response.data.roleId);
            $('#PhoneNumber').val(response.data.phoneNumber);
            $('#Province').val(response.data.provinceId);
            $('#District').val(response.data.districtId);
            $('#Ward').val(response.data.wardId);
            $('#Address').val(response.data.address);
            $('#image_upload_preview').attr('src', `/img/${response.data.avatarPath}`);
            $('#modalUserTitle').text('CẬP NHẬT TÀI KHOẢN');
            $('#pw').hide();
            $('#pw').empty();
            $('#cpw').hide();
            $('#pw').empty();
            $('#addEditUserModal').modal('show');
        }
    });
}

//---------------- Save -----------------

user.save = function () {
    if ($('#fromAddEditUser').valid()) {
        var formData = new FormData();
        formData.append("userId", $('#UserId').val());
        formData.append("email", $('#Email').val());
        formData.append("password", $('#Password').val());
        formData.append("fullName", $('#FullName').val());
        formData.append("gender", $('#Gender').val());
        formData.append("dob", $('#Dob').val());
        formData.append("hireDate", $('#HireDate').val());
        formData.append("roleId", $('#Roles').val());
        formData.append("phoneNumber", $('#PhoneNumber').val());
        formData.append("provinceId", parseInt($('#Province').val()));
        formData.append("districtId", parseInt($('#District').val()));
        formData.append("wardId", parseInt($('#Ward').val()));
        formData.append("address", $('#Address').val());
        var checkAvatarFile = $('#Avatar')[0].files[0];
        var checkAvatar = $('#AvatarPath').val();
        if (checkAvatarFile == null && checkAvatar == '') {
            $('#AvatarPath').val('none-avatar.png')
        }
        formData.append("avatarPath", $('#AvatarPath').val());
        formData.append('avatar', $('#Avatar')[0].files[0]);
        var userId = $('#UserId').val();
        var nameUrl = 'create';
        var nameMethod = 'POST';
        if (userId != '') {
            nameUrl = 'edit';
            nameMethod = 'PATCH';
        }
        $.ajax({
            url: `/user/${nameUrl}`,
            method: `${nameMethod}`,
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.data.userId != null) {
                    bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                        $('#addEditUserModal').modal('hide');
                        user.showData();
                    });
                }
                else {
                    $('#msgResult').text(`${response.data.message}`);
                    $('#msgResult').show();
                }
            }
        });
    }
    else {
        var msg = $("#Email-error").text();
        if (msg == 'Please enter a valid email address.') {
            $("#Email-error").text('Email không hợp lệ');
        }
    }
}

//---------------- Change Status -----------------
user.changeStatusToBlocked = function (id, name) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn <b class="text-primary">Khóa</b> Tài khoản có tên đăng nhập: <b class="text-success">${name}</b>?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không',
                className: 'btn-success'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/user/changeStatusToBlocked/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.userId != null) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                user.showData();
                            });
                        } else {
                            bootbox.alert(`<h5 class="text-danger">${response.data.message} !!!</h5>`);
                        }
                    }
                });
            }
        }
    });
}

user.changeStatusToActive = function (id, name) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn <b class="text-primary">Hoạt động</b> Tài khoản có tên đăng nhập: <b class="text-success">${name}</b>?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không',
                className: 'btn-success'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/user/changeStatusToActive/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.userId != null) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                user.showData();
                            });
                        } else {
                            bootbox.alert(`<h5 class="text-danger">${response.data.message} !!!</h5>`);
                        }
                    }
                });
            }
        }
    });
}
user.delete = function (id, name) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn <b class="text-primary">Xóa</b> Tài khoản có tên đăng nhập: <b class="text-success">${name}</b>?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không',
                className: 'btn-success'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/user/delete/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.userId != null) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                user.showData();
                            });
                        } else {
                            bootbox.alert(`<h5 class="text-danger">${response.data.message} !!!</h5>`);
                        }
                    }
                });
            }
        }
    });
}

user.initRoles = function () {
    $.ajax({
        url: '/role/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            $('#Roles').append(`<option selected for="RoleId" value="">-Chọn-</option>`);
            $.each(response.data, function (i, v) {
                $('#Roles').append(
                    `<option value='${v.roleId}'>${v.roleName}</option>`
                );
            });
        }
    });
}

//---------------- Contact Info -----------------
user.initProvinces = function () {
    $.ajax({
        url: '/contactInfo/getProvinces',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            $('#Province').empty();
            $('#Province').append(`<option selected for="ProvinceId" value="">-Chọn-</option>`);
            $.each(response.data, function (i, v) {
                $('#Province').append(
                    `<option value=${v.provinceId}>${v.provinceName}</option>`
                );
            });
        }
    });
}

user.changeProvince = function (id) {
    user.initDistricts(id);
    $("#Ward").append(`<option selected value="">-Chọn-</option>`);
}

user.initDistricts = function (provinceId, ditrictId) {
    $.ajax({
        url: `/contactInfo/getDistricts/${provinceId}`,
        method: "GET",
        contentType: "json",
        success: function (response) {
            $("#District").empty();
            $("#District").append(`<option selected value="">-Chọn-</option>`);
            $.each(response.data, function (i, v) {
                $("#District").append(`
                    <option value=${v.districtId}>${v.districtName}</option>
                `);
            });
            $('#District').val(ditrictId);
        }
    });
    $("#Ward").empty();
}

user.changeDistrict = function (id) {
    user.initWards(id);
}

user.initWards = function (districtId, wardId) {
    $.ajax({
        url: `/contactInfo/getWards/${districtId}`,
        method: "GET",
        contentType: "json",
        success: function (response) {
            $("#Ward").empty();
            $("#Ward").append(`<option selected value="">-Chọn-</option>`);
            $.each(response.data, function (i, v) {
                $("#Ward").append(`
                    <option value=${v.wardId}>${v.wardName}</option>
                `);
            });
            $('#Ward').val(wardId);
        }
    });
}

user.drawDataTable = function () {
    table = $("#tbUsers").DataTable(
        {
            "language": {
                "sProcessing": "Đang xử lý...",
                "sLengthMenu": "Xem _MENU_ mục",
                "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
                "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
                "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
                "sInfoFiltered": "(được lọc từ _MAX_ mục)",
                "sInfoPostFix": "",
                "sSearch": "Tìm:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "Đầu",
                    "sPrevious": "Trước",
                    "sNext": "Tiếp",
                    "sLast": "Cuối"
                }
            },
            "columnDefs": [
                {
                    "targets": 0,
                    "orderable": false,
                    "width": "20%"
                },
                {
                    "targets": 1,
                    "orderable": false,
                    "width": "18%"
                },
                {
                    "targets": 2,
                    "orderable": false,
                    "width": "10%"
                },
                {
                    "targets": 3,
                    "width": "7%"
                },
                {
                    "targets": 6,
                    "orderable": false,
                    "searchable": false
                },
                {
                    "targets": 7,
                    "orderable": false,
                    "searchable": false,
                    "width": "12%"
                }
            ]
        }
    );
};

user.resetForm = function () {
    $('#addEditUserModal').modal('hide');
    $('#fromAddEditUser').trigger('reset');
    $('#image_upload_preview').attr('src', '/img/none-avatar.png');
    $('#labelFile').text('Chọn file');
    $('#msgResult').hide();
    $('#fromAddEditUser').validate().resetForm();
}

user.openModal = function () {
    user.resetForm();
    $('#modalUserTitle').text('THÊM MỚI TÀI KHOẢN');
    $('#addEditUserModal').modal('show');
}

user.init = function () {
    user.initProvinces();
    user.initRoles();
    user.showData();
}

$(document).ready(function () {
    user.init();
});