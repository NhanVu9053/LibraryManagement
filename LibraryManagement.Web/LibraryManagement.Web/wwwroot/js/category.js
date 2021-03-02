

var category = {} || category;
var table = $('#tbCategory').DataTable();

category.showData = function () {
    $.ajax({
        url: '/category/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            table.clear().destroy();
            $('#tbCategory>tbody').empty();
             $.each(response.data, function (i, v) {
                $('#tbCategory>tbody').append(
                    `<tr>
                        <td>${v.categoryId}</td>
                        <td>${v.categoryName}</td>
                        <td>${v.createdDateStr}</td>
                        <td>${v.createdByName}</td>
                        <td>${v.modifiedDateStr}</td>
                        <td>${v.modifiedByName}</td>
                        <td>
                             <a href="javascript:;" class="text-warning  ml-2" onclick="category.edit(${v.categoryId})"><i class='fas fa-edit'></i></a>
                             <a href="javascript:;" class='text-danger ml-2' onclick="category.delete(${v.categoryId},'${v.categoryName}')"><i class='fas fa-trash'></i></a>
                        </td>
                    </tr>`
                );
             });
            category.drawDataTable();
        }
    });
}

category.delete = function (categoryId, categoryName) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
        message: `Bạn có muốn <b class="text-primary">Xóa</b> Thể loại sách: <b class="text-success">${categoryName}</b>?`,
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
                    url: `/category/delete/${categoryId}`,
                    method: 'PATCH',
                    dataType: 'JSON',
                    contentType: 'application/json',
                    success: function (response) {
                        if (response.data.categoryId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditCategoryModal').modal('hide');
                                category.showData();
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


category.save = function () {

    if ($('#fromAddEditCategory').valid()) {
        var saveObj = {};
        saveObj.categoryId = parseInt($('#CategoryId').val());
        saveObj.categoryName = $('#CategoryName').val(); 
        $.ajax({
            url: '/category/save',
            method: 'POST',
            dataType: 'JSON',
            contentType: 'application/json',
            data: JSON.stringify(saveObj),
            success: function (response) {
                if (response.data.categoryId > 0) {
                    $('#addEditCategoryModal').modal('hide');
                    bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                        category.showData();
                        category.resetForm();
                    });
                } else {
                    $('#msgResult').text(`${response.data.message}`);
                    $('#msgResult').show();
                }
            }
        });
    }
}

category.edit = function (id) {
    $.ajax({
        url: `category/get/${id}`,
        method: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (reponse) {
            category.resetForm();
            $('#CategoryId').val(reponse.data.categoryId);
            $('#CategoryName').val(reponse.data.categoryName);
            $('#modalCategoryTitle').text('CẬP NHẬT THỂ LOẠI SÁCH');
            $('#addEditCategoryModal').modal('show');
        }
    });

};

category.resetForm = function () {
    $('#addEditCategoryModal').modal('hide');
    $('#fromAddEditCategory').trigger('reset');
    $('#msgResult').hide();
    $('#fromAddEditCategory').validate().resetForm();
}

category.openModal = function () {
    category.resetForm();
    $('#modalCategoryTitle').text('THÊM MỚI THỂ LOẠI SÁCH');
    $('#addEditCategoryModal').modal('show');
}

category.drawDataTable = function () {
    table = $("#tbCategory").DataTable(
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
                        "orderable": false,
                        "searchable": false,
                        "width": "14%"
                    },
                    {
                        "targets": 3,
                        "orderable": false,
                        "searchable": false
                    },
                    {
                        "targets": 5,
                        "orderable": false,
                        "searchable": false
                    },
                    {
                        "targets": 6,
                        "orderable": false,
                        "searchable": false
                    }
                ],
                "order": [[0, 'desc']]

            }
    );
}

category.init =  function () {
     category.showData();
}

$(document).ready(function () {
    category.init();
});

