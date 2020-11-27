var category = {} || category;


category.showData = function () {
    $.ajax({
        url: '/category/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            $('#tbCategory>tbody').empty();
            $.each(response.data, function (i, v) {
                $('#tbCategory>tbody').append(
                    `<tr>
                        <td>${v.categoryId}</td>
                        <td>${v.categoryName}</td>
                        <td>${v.createdDate}</td>
                        <td>${v.createdBy}</td>
                        <td>${v.statusName}</td>
                        <td>${v.modifiedDate}</td>
                        <td>${v.modifiedBy}</td>
                        <td>
                             <a href="javascript:;" class="text-warning  ml-2" onclick="category.edit(${v.categoryId})"><i class='fas fa-edit'></i></a>
                             <a href="javascript:;" class='text-danger ml-2' onclick="category.delete(${v.categoryId},'${v.categoryName}')"><i class='fas fa-trash'></i></a>
                        </td>
                    </tr>`
                );
            });
        }
    });
}

category.initStatus = function () {
    $.ajax({
        url: '/category/status/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            $('#Status').empty();
            $.each(response.data, function (i, v) {
                $('#Status').append(
                    `<option value=${v.id}>${v.name}</option>`
                );
            });
        }
    });
}

category.delete = function (categoryId, categoryName) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Warning</h2>',
        message: `Do you want to <b class="text-primary">Delete</b>  <b class="text-success">${categoryName}</b> danh mục?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> No'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Yes'
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
                        bootbox.alert(`<h4 class="alert alert-danger">${response.data.message} !!!</h4>`);
                        if (response.data.categoryId > 0) {
                            $('#addEditCategoryModal').modal('hide');
                            category.showData();
                        }
                    }
                });
            }
        }
    });
}


category.openModal = function () {
    
    document.getElementById('msg').style.display = 'none';
    $('#addEditCategoryModal').modal('show');
}

category.tables = function () {
    $("#tbCategory").dataTable(
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
                    "targets": 3,
                    "orderable": false
                },
                {
                    "targets": 4,
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
                }
            ]
        }
    );
};
category.init = function () {
    category.showData();
    category.initStatus();
}

$(document).ready(function () {
    category.init();
});

