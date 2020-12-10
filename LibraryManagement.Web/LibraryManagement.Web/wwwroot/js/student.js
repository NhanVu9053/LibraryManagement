var student = {} || student;
var table = $('#tbStudents').DataTable();

student.showData = function () {
    $.ajax({
        url: '/student/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            table.clear().destroy();
            $('#tbStudents>tbody').empty();
            $.each(response.data, function (i, v) {
                if (v.statusName != 'Deleted') {
                    var actions = "";
                    switch (v.statusId) {
                        case 1:
                            {
                                actions = `<a href='javascript: void(0)' class='text-dark ml-2' onclick='student.changeStatusToBlocked(${v.studentId})' title='Khóa'><i class='fa fa-ban'></i></a>`;
                                actions += `<a href='javascript: void(0)' class='text-danger ml-2' onclick='student.delete(${v.studentId})' title='Xóa'><i class='fas fa-trash'></i></a>`;
                                break;
                            }
                        case 3:
                            {
                                actions = `<a href='javascript: void(0)' class='text-success ml-2' onclick='student.changeStatusToActive(${v.studentId})' title='Hoạt dộng'><i class='far fa-play-circle'></i></a>`;
                                actions += `<a href='javascript: void(0)' class='text-danger ml-2' onclick='student.delete(${v.studentId})' title='Xóa'><i class='fas fa-trash'></i></a>`;
                                break;
                            }
                    }
                    $('#tbStudents>tbody').append(
                     `<tr>
                        <td>${v.studentId}</td>
                        <td>${v.studentName}</td>
                        <td>${v.courseName}</td>
                        <td>${(v.gender == true ? "Nam" : "Nữ")}</td>
                        <td>${v.phoneNumber}</td>
                        <td>${v.email}</td>
                        <td class="text-center">
                            <span class="${(v.statusId == 1 ? "btn btn-primary" : (v.statusId == 2 ? "btn btn-success" : (v.statusId == 3? "btn btn-danger" : "btn btn-info")))}" style="width: 100px; height: 40px;">
                                ${v.statusName}
                            </span>
                        </td>
                        <td class="text-center">
                            <img src="/img/${v.avatarPath}" style="width: 60px; height: 70px;" />
                        </td>
                        <td>
                            <a href="javascript:void(0)"  onclick="student.details(${v.studentId})" class="text-primary ml-2" title="Chi tiết"><i class="fas fa-eye"></i></a>
                            <a href="javascript:void(0)" class="text-warning ml-2" onclick="student.edit(${v.studentId})" title="Cập nhật"><i class="fas fa-edit"></i></a>
                            ${actions}
                        </td>
                    </tr>`
                    );
                }
            });
            student.drawDataTable();
        }
    });
}

student.details = function (studentId) {
    $('#dataModal').empty();
    $.ajax({
        url: `/student/get/${studentId}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            if (response.data.studentId > 0) {
                $('#dataModalTitle').text('THÔNG TIN HỌC SINH');
                $('#dataModal').append(
                    `<h5 class="text-info text-center m-2">Học sinh: ${response.data.studentName}</h5>
                    <br />
                    <div class="row justify-content-center col-xl-12">
                        <div class="col-xl-6 col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <p><b>StudentId:</b> ${response.data.studentId}</p>
                                    <p><b>Tên lớp:</b> ${response.data.courseName}</p>
                                    <p><b>Giới tính:</b> ${(response.data.gender == true ? "Nam" : "Nữ")}</p>
                                    <p><b>Ngày sinh:</b> ${response.data.dobStr}</p>
                                    <p><b>Số điện thoại:</b> ${response.data.phoneNumber}</p>
                                    <p><b>Email:</b> ${response.data.email}</p>
                                    <p><b>Địa chỉ:</b>${response.data.addressStr}</p>
                                    <p><b>Trạng thái:</b> ${response.data.statusName}</p>
                                    <p><b>Ngày tạo:</b> ${response.data.createdDateStr}</p>
                                    <p><b>Người tạo:</b> ${response.data.createdBy}</p>
                                    <p><b>Ngày cập nhật:</b> ${response.data.modifiedDateStr}</p>
                                    <p><b>Người cập nhật:</b> ${response.data.modifiedBy}</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-6 col-md-12">
                            <img src="/img/${response.data.avatarPath}" class="m-2" style="width: 400px; height: 450px;" />
                        </div>
                    </div>`
                );
                $('#detailsData').modal('show');
            }
            else {
                bootbox.alert(`<h5 class="text-danger">Học sinh này không tồn tại !!!</h5>`)
            }
        }
    });
}
//---------------- Edit -----------------
student.edit = function (id) {
    $.ajax({
        url: `/student/get/${id}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            student.resetForm();
            student.initDistricts(response.data.provinceId, response.data.districtId);
            student.initWards(response.data.districtId, response.data.wardId);
            $('#StudentId').val(response.data.studentId);
            $('#AvatarPath').val(response.data.avatarPath);
            $('#StudentName').val(response.data.studentName);
            $('#CourseName').val(response.data.courseName);
            $('#Gender').val(`${response.data.gender}`);
            var dobString = response.data.dob.toString();
            $('#Dob').val(dobString.slice(0, 10));
            $('#Email').val(response.data.email);
            $('#PhoneNumber').val(response.data.phoneNumber);
            $('#Province').val(response.data.provinceId);
            $('#District').val(response.data.districtId);
            $('#Ward').val(response.data.wardId);
            console.log(response.data.provinceId, response.data.districtId, response.data.wardId);
            $('#Address').val(response.data.address);
            $('#image_upload_preview').attr('src', `/img/${response.data.avatarPath}`);
            $('#modalStudentTitle').text('CẬP NHẬT SÁCH');
            $('#addEditStudentModal').modal('show');
        }
    });
}
//---------------- Save -----------------
student.checkSave = function () {
    var vnf_regex = /((09|03|07|08|05)+([0-9]{8})\b)/g;
    var mobile = $('#PhoneNumber').val();
    $("#msgPhone").hide();
    if ($('#fromAddEditStudent').valid()) {
        if (vnf_regex.test(mobile) == false) {
            $("#msgPhone").text('Số điện thoại không đúng định dạng');
            $("#msgPhone").show();
        }
        else {
            student.save();
        }
    }
}

student.save = function () {
    if ($('#fromAddEditStudent').valid()) {
        var formData = new FormData();
        formData.append("studentId", parseInt($('#StudentId').val()));
        formData.append("studentName", $('#StudentName').val());
        formData.append("courseName", $('#CourseName').val());
        formData.append("gender", $('#Gender').val());
        formData.append("dob", $('#Dob').val());
        formData.append("email", $('#Email').val());
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
        console.log(formData);
        $.ajax({
            url: '/student/save',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                    if (response.data.studentId > 0) {
                        bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                            $('#addEditStudentModal').modal('hide');
                            student.showData();
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
//---------------- Contact Info -----------------
student.initProvinces = function () {
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

student.changeProvince = function (id) {
    student.initDistricts(id);
    $("#Ward").append(`<option selected value="">-Chọn-</option>`);
}

student.initDistricts = function (provinceId , ditrictId) {
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

student.changeDistrict = function (id) {
    student.initWards(id);
}

student.initWards = function (districtId, wardId) {
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


student.delete = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn xóa <b class="text-primary">Học sinh</b> có <b class="text-success">ID: ${id}</b> này không?`,
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
                    url: `/Student/Delete/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.studentId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                student.showData();
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

student.changeStatusToBlocked = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn <b class="text-primary">Khóa</b> Học sinh có ID <b class="text-success">${id}</b>?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/Student/ChangeStatusToBlocked/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.studentId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                student.showData();
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

student.changeStatusToActive = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn chuyển trạng thái <b class="text-primary">Hoạt động</b> Học sinh có ID <b class="text-success">${id}</b>?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/Student/ChangeStatusToActive/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.studentId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                student.showData();
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

student.drawDataTable = function () {
    table = $("#tbStudents").DataTable(
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
                    "targets": 1,
                    "orderable": false
                },
                {
                    "targets": 2,
                    "orderable": false
                },
                {
                    "targets": 3,
                    "orderable": false
                },
                {
                    "targets": 4,
                    "orderable": false
                },
                {
                    "targets": 5,
                    "orderable": false
                },
                {
                    "targets": 6,
                    "orderable": false
                },
                {
                    "targets": 7,
                    "orderable": false,
                    "searchable": false
                },
                {
                    "targets": 8,
                    "orderable": false,
                    "searchable": false
                }
            ],
            "order": [[0, 'desc']]
        }
    );
};

student.resetForm = function () {
    $('#addEditStudentModal').modal('hide');
    $('#fromAddEditStudent').trigger('reset');
    $('#image_upload_preview').attr('src', '/img/none-avatar.png');
    $('#labelFile').text('Chọn file');
    $('#msgResult').hide();
    $('#fromAddEditStudent').validate().resetForm();
}

student.openModal = function () {
    student.resetForm();
    $('#modalStudentTitle').text('THÊM MỚI HỌC SINH');
    $('#addEditStudentModal').modal('show');
}

student.init = function () {
    student.initProvinces();
    student.showData();
}

$(document).ready(function () {
    student.init();
});