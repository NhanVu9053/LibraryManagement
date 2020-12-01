var student = {} || student;
student.delete = function (id) {
    bootbox.confirm({
        title: "Cảnh báo",
        message: "Bạn có muốn xóa Học sinh này không?",
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
student.initProvinces = function () {
    $.ajax({
        url: '/contactInfo/getProvinces',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            //console.log(response);
            $('#Provinces').empty();
            $('#Provinces').append(`<option selected for="ProvinceId" value="">-Chọn-</option>`);
            $.each(response.data, function (i, v) {
                $('#Provinces').append(
                    `<option value=${v.provinceId}>${v.provinceName}</option>`
                );
            });
        }
    });
}
student.openModal = function () {
    //book.resetForm();
    $('#modalStudentTitle').text('THÊM MỚI HỌC SINH');
    $('#addEditStudentModal').modal('show');
}
student.init = function () {
    //module.showData();
    student.initProvinces();
    student.initDistricts();
    student.initWards();
}

$(document).ready(function () {
    student.init();
});
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