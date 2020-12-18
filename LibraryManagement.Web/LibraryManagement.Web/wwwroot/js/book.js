var book = {} || book;
var table = $('#tbBooks').DataTable();

book.showData = function () {
    $.ajax({
        url: '/book/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            table.clear().destroy();
            $('#tbBooks>tbody').empty();
            $.each(response.data, function (i, v) {
                if (v.statusName != 'Deleted') {
                    var actions = "";
                    switch (v.statusId) {
                        case 1:
                            {
                                actions = `<a href="javascript: void(0)" class="text-dark ml-2" onclick="book.changeStatusToPending(${v.bookId})" title="Tạm dừng"><i class="far fa-stop-circle"></i></a>`;
                                break;
                            }
                        case 3:
                            {
                                actions = `<a href="javascript: void(0)" class="text-success ml-2" onclick="book.changeStatusToStochking(${v.bookId})" title="Cho mượn"><i class="far fa-play-circle"></i></a>`;
                                actions += `<a href="javascript: void(0)" class="text-danger ml-2" onclick="book.delete(${v.bookId})" title="Xóa"><i class="fas fa-trash"></i></a>`;
                                break;
                            }
                    }
                    $('#tbBooks>tbody').append(
                        ` <tr>
                        <td>${v.bookId}</td>
                        <td>${v.bookName}</td>
                        <td>${v.categoryName}</td>
                        <td>${v.dopStr}</td>
                        <td class="text-center">
                            <span class="${(v.statusId == 1? 'btn btn-primary': (v.statusId == 2? 'btn btn-danger' : (v.statusId == 3? 'btn btn-warning' : 'btn btn-info')))}" style="width: 100px; height: 40px;">
                               ${v.statusName}
                            </span>
                        </td>
                        <td class="text-center">
                            <img src="/img/${v.imagePath}" style="width: 60px; height: 70px;" />
                        </td>
                        <td>
                            <a href="javascript:;" onclick="book.details(${v.bookId})" class="text-primary ml-2" title="Chi tiết"><i class="fas fa-eye"></i></a>
                            <a href="javascript:void(0)" class="text-warning ml-2" onclick="book.edit(${v.bookId})" title="Cập nhật"><i class="fas fa-edit"></i></a>
                            ${actions}
                        </td>
                    </tr>`
                    );
                }
            });
            book.drawDataTable();
        }
    });
}

book.details = function (bookId) {
    $('#dataModal').empty();
    $.ajax({
        url: `/book/get/${bookId}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            if (response.data.bookId > 0) {
                $('#dataModalTitle').text('THÔNG TIN SÁCH');
                $('#dataModal').append(
                    `<h5 class="text-info m-2 text-center">Tên sách: ${response.data.bookName}</h5>
                    <br />
                    <div class="row justify-content-center col-xl-12">
                        <div class="col-xl-6 col-md-12">
                            <div class="card">
                                <div class="card-body">
                                <p><b>Mã sách:</b> ${response.data.bookId}</p>
                                <p><b>Thể loại:</b> ${response.data.categoryName}</p>
                                <p><b>Tác giả:</b> ${response.data.author}</p>
                                <p><b>Ngày xuất bản:</b> ${response.data.dopStr}</p>
                                <p><b>Nhà xuất bản:</b> ${response.data.publishCompany}</p>
                                <p><b>Tóm tắt nội dung:</b> ${response.data.description}</p>
                                <p><b>Số trang:</b> ${response.data.page}</p>
                                <p><b>Tổng số lượng sách:</b> ${response.data.quantity}</p>
                                <p><b>Số sách còn trong kho:</b> ${response.data.quanityRemain}</p>
                                <p><b>Trạng thái:</b> ${response.data.statusName}</p>
                                <p><b>Ngày tạo:</b> ${response.data.createdDateStr}</p>
                                <p><b>Người tạo:</b> ${response.data.createdByName}</p>
                                <p><b>Ngày cập nhật:</b> ${response.data.modifiedDateStr}</p>
                                <p><b>Người cập nhật:</b> ${response.data.modifiedByName}</p>
                                </div>
                            </div>
                        </div>
                            <div class="col-xl-6 col-md-12 text-center">
                                <img src="/img/${response.data.imagePath}" class="m-2" style="width: 400px; height: 450px;"/>
                            </div>
                    </div> `
                );
                $('#detailsData').modal('show');
            }
            else {
                bootbox.alert(`<h5 class="text-danger">Sách này không tồn tại !!!</h5>`)
            }
        }
    });
}

book.edit = function (id) {
    $.ajax({
        url: `/book/get/${id}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            book.resetForm();
            $('#BookId').val(response.data.bookId);
            $('#BookName').val(response.data.bookName);
            $('#Categoryies').val(response.data.categoryId);
            $('#Author').val(response.data.author);
            var dopString = response.data.dop.toString();
            $('#Dop').val(dopString.slice(0,10));
            $('#PublishCompany').val(response.data.publishCompany);
            $('#Description').val(response.data.description);
            $('#Page').val(response.data.page);
            $('#ImagePath').val(response.data.imagePath);
            $('#image_upload_preview').attr('src',`/img/${response.data.imagePath}`);
            $('#modalBookTitle').text('CẬP NHẬT SÁCH');
            $('#QuantityBook').hide();
            $('#addEditBookModal').modal('show');
        }
    });
}

book.checkSave = function () {
    var bookId = parseInt($('#BookId').val());
    var checkImgFile = $('#Image').val();
    $("#msgImg").hide();
    if ($('#fromAddEditBook').valid()) {
        if (bookId > 0) {
            book.save();
        } else if (bookId == 0 && checkImgFile == '') {
            $("#msgImg").text('Hình ảnh sách không được để trống');
            $("#msgImg").show();
        } else {
            book.save();
        }
    }
}
book.save = function () {
    var formData = new FormData();
    formData.append("bookId", parseInt($('#BookId').val()));
    formData.append('image', $('#Image')[0].files[0]);
    formData.append("bookName", $('#BookName').val());
    formData.append("categoryId", parseInt($('#Categoryies').val()));
    formData.append("author", $('#Author').val());
    formData.append("dop", $('#Dop').val());
    formData.append("publishCompany", $('#PublishCompany').val());
    formData.append("description", $('#Description').val());
    formData.append("page", parseInt($('#Page').val()));
    formData.append("quantity", parseInt($('#Quantity').val()));
    formData.append("imagePath", $('#ImagePath').val());
    $.ajax({
        url: '/book/save',
        method: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.data.bookId > 0) {
                bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                    $('#addEditBookModal').modal('hide');
                    book.showData();
                });
            }
            else {
                $('#msgResult').text(`${response.data.message}`);
                $('#msgResult').show();
            }
        }
    });
}


book.delete = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn xóa <b class="text-primary">Sách</b> có <b class="text-success">ID: ${id}</b> này không?`,
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
                    url: `/Book/Delete/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.bookId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                book.showData();
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

book.changeStatusToPending = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn <b class="text-primary">Tạm dừng mượn</b> Sách có ID <b class="text-success">${id}</b>?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không',
                className: 'btn-success'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có',
                className: 'btn-warning'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/Book/ChangeStatusToPending/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.bookId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                book.showData();
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

book.changeStatusToStochking = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
        message: `Bạn có muốn <b class="text-primary">Cho mượn</b> Sách có ID <b class="text-success">${id}</b>?`,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Không',
                className: 'btn-success'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Có',
                className: 'btn-warning'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: `/Book/ChangeStatusToStochking/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (response) {
                        if (response.data.bookId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditBookModal').modal('hide');
                                book.showData();
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
book.changeImage = function () {
    $("#msgImg").empty();
    $("#msgImg").hide();
}

book.resetForm = function () {
    $('#addEditBookModal').modal('hide');
    $('#fromAddEditBook').trigger('reset');
    $('#image_upload_preview').attr('src', '/img/none-imgbook.png');
    $('#addEditBookModal').validate().resetForm();
    $('#labelBook').text('Chọn file');
    $('#msgResult').hide();
}


book.openModal = function () {
    book.resetForm();
    $('#modalBookTitle').text('THÊM MỚI SÁCH');
    $('#addEditBookModal').modal('show');
}

book.drawDataTable = function () {
    table = $("#tbBooks").DataTable(
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
};


book.initCategoryies = function () {
    $.ajax({
        url: '/category/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            $('#Categoryies').empty();
            $('#Categoryies').append(`<option selected for="CategoryId" value="">-Chọn-</option>`);
            $.each(response.data, function (i, v) {
                $('#Categoryies').append(
                    `<option value=${v.categoryId}>${v.categoryName}</option>`
                );
            });
        }
    });
}

book.init = function () {
    book.initCategoryies();
    book.showData();
}

$(document).ready(function () {
    book.init();
});