var student = {} || student;
student.delete = function (id) {
    bootbox.confirm({
        title: "Cảnh báo",
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
                    success: function () {
                        window.location.href = "/Student/Index";
                    }
                });
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
            console.log(response.data);
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
            //$('#Status').val(response.data.statusId);
            if (response.data.avatarPath = "none-avatar.png") {
                response.data.avatarPath = "none-avatar.png?v=aAV3uOswN-3pO2CQXqL8QINyLxyvHnj8hSbQVotuF2w";
            }
            $('#image_upload_preview').attr('src', `/img/${response.data.avatarPath}`);
            $('#modalStudentTitle').text('CẬP NHẬT SÁCH');
            $('#addEditStudentModal').modal('show');
        }
    });
}
//---------------- Save -----------------
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
        //formData.append("statusId", parseInt($('#Status').val()));
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
                        window.location.href = `/Student/Details/${response.data.studentId}`;
                    } else {
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
            //console.log(response);
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
//---------------------------------------------------

//student.initStatus = function () {
//    $.ajax({
//        url: '/student/status/gets',
//        method: 'GET',
//        dataType: 'JSON',
//        success: function (response) {
//            $('#Status').empty();
//            $('#Status').append(`<option selected for="StatusId" value="">-Chọn-</option>`);
//            $.each(response.data, function (i, v) {
//                $('#Status').append(
//                    `<option value=${v.statusId}>${v.statusName}</option>`
//                );
//            });
//        }
//    });
//}

student.resetForm = function () {
    $('.close').on('click', function () {
        $('#addEditStudentModal').modal('hide');
        $('#fromAddEditStudent').trigger('reset');
    });
    $('#closeModal').on('click', function () {
        $('#addEditStudentModal').modal('hide');
        $('#fromAddEditStudent').trigger('reset');
    });
    $('#image_upload_preview').attr('src', '/img/none-avatar.png?v=aAV3uOswN-3pO2CQXqL8QINyLxyvHnj8hSbQVotuF2w');
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
    //student.initStatus();
}

$(document).ready(function () {
    student.init();
});

student.changeStatusToBlocked = function (id) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
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
                    success: function (data) {
                        console.log(data);
                        if (data) {
                            window.location.href = `/Student/Index`;
                        }
                    }
                });
            }
        }
    });
}

student.changeStatusToActive = function (id) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
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
                    success: function (data) {
                        console.log(data);
                        if (data) {
                            window.location.href = `/Student/Index`;
                        }
                    }
                });
            }
        }
    });
}

$(document).ready(function () {
    $("#tbStudents").dataTable(
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
});