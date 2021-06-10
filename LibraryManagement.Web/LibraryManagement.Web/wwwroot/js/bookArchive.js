var bookArchive = {} || bookArchive;
var table = $('#tbBookArchive').DataTable();
var pageCurrent = 0;

bookArchive.showData = function () {
    $.ajax({
        url: '/bookArchive/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            table.clear().destroy();
            $('#tbBookArchive>tbody').empty();
            $.each(response.data, function (i, v) {
                $('#tbBookArchive>tbody').append(
                    ` <tr>
                        <td>${v.bookArchiveId}</td>
                        <td>${v.bookId}</td>
                        <td class="text-left">
                           ${v.bookName}
                        </td>
                        <td>${v.quantity}</td>
                        <td>${v.quantityRemain}</td>
                        <td>
                            <a href="javascript:;" onclick="bookArchive.details(${v.bookArchiveId})" class="text-primary ml-2" title="Chi tiết"><i class="fas fa-eye"></i></a>
                            <a href="javascript:;" class="text-success  ml-2" onclick="bookArchive.edit(${v.bookArchiveId},true,'${v.bookName}')" title="Tăng SL"><i class="fa fa-plus-circle" aria-hidden="true"></i></a>
                            <a href="javascript:;" class="text-warning  ml-2" onclick="bookArchive.edit(${v.bookArchiveId},false,'${v.bookName}')" title="Giảm SL"><i class="fa fa-minus-circle" aria-hidden="true"></i></a>
                            <a href="javascript:;" class="text-danger ml-2" onclick="bookArchive.delete(${v.bookArchiveId},'${v.bookName}')" title="Xóa"><i class='fas fa-trash'></i></a>
                        </td>
                    </tr>`
                );
            });
            bookArchive.drawDataTable();
        }
    });
}


bookArchive.details = function (bookArchiveId) {
    $('#dataModal').empty();
    $.ajax({
        url: `/bookArchive/get/${bookArchiveId}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            if (response.data.bookArchiveId > 0) {
                $('#dataModalTitle').text('THÔNG TIN KHO SÁCH');
                $('#dataModal').append(
                    `<h5 class="text-info m-2 text-center">Kho sách: ${response.data.bookName}</h5>
                    <div class="row justify-content-center col-xl-12">
                        <div class="col-xl-6 col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <p><b>Mã Kho sách:</b> ${response.data.bookArchiveId}</p>
                                    <p><b>Mã Sách:</b> ${response.data.bookId}</p>
                                    <p><b>Tên sách:</b> ${response.data.bookName}</p>
                                    <p><b>Thể loại:</b> ${response.data.categoryName}</p>
                                    <p><b>Tổng số lượng:</b> ${response.data.quantity}</p>
                                    <p><b>Số lượng còn:</b> ${response.data.quantityRemain}</p>
                                    <p><b>Ngày tạo:</b> ${response.data.createdDateStr}</p>
                                    <p><b>Người tạo:</b> ${response.data.createdByName}</p>
                                    <p><b>Ngày cập nhật:</b> ${response.data.modifiedDateStr}</p>
                                    <p><b>Người cập nhật:</b> ${response.data.modifiedByName}</p>
                                </div>
                            </div>
                        </div>
                            <div class="col-xl-6 col-md-12 text-center">
                                <img src="/img/${response.data.imagePath}" class="m-5 img_book_details"/>
                            </div>
                    </div> `
                );
                $('#detailsData').modal('show');
            }
            else {
                bootbox.alert(`<h5 class="text-danger">Kho lưu trữ này không tồn tại !!!</h5>`)
            }
        }
    });
}

bookArchive.delete = function (bookArchiveId, bookName) {
    pageCurrent = table.page.info().page;
    bootbox.confirm({
        title: '<h2 class="text-danger">Thông báo</h2>',
        message: `Bạn có muốn xóa <b class="text-primary">Kho sách</b> lưu sách <b class="text-success"> ${bookName}</b> này không?`,
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
                    url: `/bookArchive/delete/${bookArchiveId}`,
                    method: 'PATCH',
                    dataType: 'JSON',
                    contentType: 'application/json',
                    success: function (response) {
                        if (response.data.bookArchive > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#editBookArchiveModal').modal('hide');
                                bookArchive.showData();
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

bookArchive.edit = function (id, isPlus, bookName) {
    bookArchive.openModal();
    $('#bookName').text(`Tên sách: ${bookName}`);
    $('#BookArchiveId').val(id);
    $('#IsPlus').val(isPlus);
    if (isPlus) {
        $('#modalBookArchiveTitle').text('CẬP NHẬT(TĂNG) SỐ LƯỢNG KHO');
    } else {
        $('#modalBookArchiveTitle').text('CẬP NHẬT(GIẢM) SỐ LƯỢNG KHO');
    }
};

bookArchive.save = function () {
    pageCurrent = table.page.info().page;
    $('#msgResult').hide();
    if ($('#fromEditbookArchive').valid()) {
        var saveObj = {};
        saveObj.bookArchiveId = parseInt($('#BookArchiveId').val());
        saveObj.isPlus = ('true' === $('#IsPlus').val());
        saveObj.value = parseInt($('#Value').val());
        if (saveObj.bookArchiveId > 0) {
            $.ajax({
                url: '/bookarchive/save',
                method: 'PATCH',
                dataType: 'JSON',
                contentType: 'application/json',
                data: JSON.stringify(saveObj),
                success: function (response) {
                    if (response.data.bookArchiveId > 0) {
                        $('#editBookArchiveModal').modal('hide');
                        bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                            bookArchive.showData();
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
}


bookArchive.resetForm = function () {
    $('#editBookArchiveModal').modal('hide');
    $('#fromEditbookArchive').trigger('reset');
    $('#msgResult').hide();
    $('#fromEditbookArchive').validate().resetForm();
}

bookArchive.openModal = function () {

    bookArchive.resetForm();
    $('#modalBookArchiveTitle').text('CẬP NHẬT SỐ LƯỢNG KHO');
    $('#editBookArchiveModal').modal('show');
}


bookArchive.drawDataTable = function () {
    table = $("#tbBookArchive").DataTable(
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
                    "width" :"45%"

                },
                {
                    "targets": 5,
                    "orderable": false,
                    "searchable": false
                }
            ],
            "order": [[0, 'desc']]
        }
    );
    table.page(pageCurrent).draw(false);
}
bookArchive.init = function () {
    bookArchive.showData();
}

$(document).ready(function () {
    bookArchive.init();
});
