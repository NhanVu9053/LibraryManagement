var book = {} || book;
book.delete = function (id) {
    bootbox.confirm({
        title: "Cảnh báo",
        message: "Bạn có muốn xóa cuốn sách này không?",
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
    //book.openModal();
    //$('#Image').rules('remove', 'required')
    //$('#Image').attr('data-rule-required', 'false');
    //$('#Image').removeAttr('required');
    $.ajax({
        url: `/book/get/${id}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            book.resetForm();
            console.log(response.data);
            //$('#fromAddEditBook').trigger('reset');
            $('#BookId').val(response.data.bookId);
            $('#BookName').val(response.data.bookName);
            $('#Categoryies').val(response.data.categoryId);
            $('#Author').val(response.data.author);
            var dobString = response.data.dop.toString();
            $('#Dop').val(dobString.slice(0,10));
            $('#PublishCompany').val(response.data.publishCompany);
            $('#Description').val(response.data.description);
            $('#Page').val(response.data.page);
            $('#Quanity').val(response.data.quanity);
            $('#Status').val(response.data.statusId);
            $('#ImagePath').val(response.data.imagePath);
            $('#image_upload_preview').attr('src',`/img/${response.data.imagePath}`);
            $('#addEditBookModal').find('.modal-title').text('Update Book');
            $("#Image").removeAttr('data-rule-required');
            $("#Image").removeAttr('data-msg-required');
            $('#addEditBookModal').modal('show');
            //document.getElementById('msg').style.display = 'none';
        }
    });
}

book.save = function () {
    //var checkBookId = parseInt($('#BookId').val());
    //console.log(checkBookId);
    //if (checkBookId > 0) {
    //    $('#Image').attr('data-rule-required', 'false');
    //    $("#Image").removeAttr('data-rule-required');
    //    $("#Image").removeAttr('data-msg-required');     
    //    console.log(checkBookId);
    //};
    if ($('#fromAddEditBook').valid()) {
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
        formData.append("quanity", parseInt($('#Quanity').val()));
        formData.append("statusId", parseInt($('#Status').val()));
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
                bootbox.alert(`<h4 class="alert alert-danger">${response.data.message} !!!</h4>`, () => {
                    if (response.data.bookId > 0) {
                        $('#addEditBookModal').modal('hide');
                        window.location.href = "/Book/Index";
                    }
                });
            }
        });
    }
}

book.resetForm = function () {
    $('.close').on('click', function () {
        $('#addEditBookModal').modal('hide');
        $('#fromAddEditBook').trigger('reset');
        window.location.href = "/Book/Index";
    });
    $('#closeModal').on('click', function () {
        $('#addEditBookModal').modal('hide');
        $('#fromAddEditBook').trigger('reset');
        window.location.href = "/Book/Index";
    });
    $('#image_upload_preview').attr('src', '/img/none-imgbook.png');

    //var classError = document.getElementsByClassName('error');
    //var count = 0;
    //for (let i = 0; i < classError.length; i++) {
    //    let item = classError[i];
    //    if (count % 2) {
    //        item.style.display = 'none';
    //    } else {
    //        item.classList.remove('error');
    //        i--;
    //    }
    //    count++;
    //}
}


book.initCategoryies = function () {
    $.ajax({
        url: '/category/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            //console.log(response);
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
book.initStatus = function () {
    $.ajax({
        url: '/book/status/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            //console.log(response);
            $('#Status').empty();
            $('#Status').append(`<option selected for="StatusId" value="">-Chọn-</option>`);
            $.each(response.data, function (i, v) {
                $('#Status').append(
                    `<option value=${v.statusId}>${v.statusName}</option>`
                );
            });
        }
    });
}



book.openModal = function () {
    book.resetForm();
    //document.getElementById('msg').style.display = 'none';
    $('#addEditBookModal').modal('show');
    //book.resetForm();
}
book.init = function () {
    //module.showData();
    book.initStatus();
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

//book.checkStatusBookIsOver = function (id) {
//    $.ajax({
//        url: `/Book/CheckStatusBookIsOver/${id}`,
//        method: "PATCH",
//        contentType: 'JSON',
//        success: function () {
//                window.location.href = `/Book/Index`;
//        }
//    });
//}

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