var book = {} || book;
book.delete = function (id) {
    bootbox.confirm({
        title: "Cảnh báo",
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
                    success: function () {
                            window.location.href = "/Book/Index";
                    }
                });
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
            console.log(response.data);
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
    console.log(formData);
    $.ajax({
        url: '/book/save',
        method: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log(response);
            if (response.data.bookId > 0) {
                window.location.href = `/Book/Details/${response.data.bookId}`;
            }
            else {
                $('#msgResult').text(`${response.data.message}`);
                $('#msgResult').show();
            }
        }
    });
}

book.resetForm = function () {
    $('.close').on('click', function () {
        $('#addEditBookModal').modal('hide');
        $('#fromAddEditBook').trigger('reset');
    });
    $('#closeModal').on('click', function () {
        $('#addEditBookModal').modal('hide');
        $('#fromAddEditBook').trigger('reset');
    });
    $('#image_upload_preview').attr('src', '/img/none-imgbook.png');
    $('#addEditBookModal').validate().resetForm();
    $('#labelBook').text('Chọn file');
    $('#msgResult').hide();
}


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



book.openModal = function () {
    book.resetForm();
    $('#modalBookTitle').text('THÊM MỚI SÁCH');
    $('#addEditBookModal').modal('show');
}
book.init = function () {
    book.initCategoryies();
}

$(document).ready(function () {
    book.init();
});

book.changeStatusToPending = function (id) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
        message: `Bạn có muốn <b class="text-primary">Tạm dừng mượn</b> Sách có ID <b class="text-success">${id}</b>?`,
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
                    url: `/Book/ChangeStatusToPending/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (data) {
                        console.log(data);
                        if (data) {
                            window.location.href = `/Book/Index`;
                        }
                    }
                });
            }
        }
    });
}

book.changeStatusToStochking = function (id) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
        message: `Bạn có muốn <b class="text-primary">Cho mượn</b> Sách có ID <b class="text-success">${id}</b>?`,
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
                    url: `/Book/ChangeStatusToStochking/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (data) {
                        console.log(data);
                        if (data) {
                            window.location.href = `/Book/Index`;
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
$(document).ready(function () {
    $("#tbBooks").dataTable(
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
});