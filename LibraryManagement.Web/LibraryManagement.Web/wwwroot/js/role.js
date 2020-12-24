var role = {} || role;
var table = $('#tbRoles').DataTable();

role.showData = function () {
    $.ajax({
        url: '/role/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            table.clear().destroy();
            $('#tbRoles>tbody').empty();
            $.each(response.data, function (i, v) {
                $('#tbRoles>tbody').append(
                        `<tr>
                        <td>${v.roleId}</td>
                        <td>${v.roleName}</td>
                        <td>
                            <a href="javascript:void(0)"  onclick="role.edit('${v.roleId}')" class="text-warning ml-2" title="Cập nhật"><i class="fas fa-edit"></i></a>
                            <a href="javascript:void(0)" class="text-danger ml-2" onclick="role.delete('${v.roleId}','${v.roleName}')" title="Xóa"><i class="fas fa-trash"></i></a>
                        </td>
                    </tr>`
                    );
            });
            role.drawDataTable();
        }
    });
}

role.edit = function (id) {
    $.ajax({
        url: `/role/get/${id}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            role.resetForm();
            $('#RoleId').val(response.data.roleId);
            $('#RoleName').val(response.data.roleName);
            $('#modalRoleTitle').text('CẬP NHẬT ROLE');
            $('#addEditRoleModal').modal('show');
        }
    });
}

role.save = function () {
    $('#msgResult').hide();
    if ($('#fromAddEditRole').valid()) {
        var saveObj = {};
        saveObj.roleId = $('#RoleId').val();
        saveObj.roleName = $('#RoleName').val();
        var nameUrl = 'create';
        var nameMethod = 'POST';
        if (saveObj.roleId != '') {
            nameUrl = 'edit';
            nameMethod = 'PATCH';
        }
        $.ajax({
            url: `/role/${nameUrl}`,
            method: `${nameMethod}`,
            dataType: 'JSON',
            contentType: 'application/json',
            data: JSON.stringify(saveObj),
            success: function (response) {
                if (response.data.roleName != null) {
                    $('#addEditRoleModal').modal('hide');
                    bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                        role.showData();
                    });
                }
                else {
                    $('#msgResult').text(`${response.data.message}`);
                    $('#msgResult').show();
                }
            }
        });
    }
}

role.delete = function (roleId, roleName) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
        message: `Bạn có muốn <b class="text-primary">Xóa</b> Role: <b class="text-success">${roleName}</b>?`,
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
                    url: `/role/delete/${roleId}`,
                    method: 'DELETE',
                    dataType: 'JSON',
                    contentType: 'application/json',
                    success: function (response) {
                        if (response.data.roleName != null) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditRoleModal').modal('hide');
                                role.showData();
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

role.resetForm = function () {
    $('#addEditRoleModal').modal('hide');
    $('#fromAddEditRole').trigger('reset');
    $('#msgResult').hide();
    $('#fromAddEditRole').validate().resetForm();
}

role.openModal = function () {
    role.resetForm();
    $('#modalRoleTitle').text('THÊM MỚI ROLE');
    $('#addEditRoleModal').modal('show');
}

role.drawDataTable = function () {
    table = $("#tbRoles").DataTable(
        {
            "ordering": false,
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
                    "width": "40%"
                },
                {
                    "targets": 1,
                    "width": "40%"
                },
                {
                    "targets": 2,
                    "orderable": false,
                    "searchable": false
                }
            ]
        }
    );
};

role.init = function () {
    role.showData();
}

$(document).ready(function () {
    role.init();
});