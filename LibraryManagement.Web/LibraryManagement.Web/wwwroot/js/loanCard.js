var loanCard = {} || loanCard;

loanCard.edit = function (id) {
    loanCard.openModal();
    $.ajax({
        url: `/loanCard/get/${id}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            $('#loanCard_id').text(response.data.loanCard.loanCardId).show();
            $('#LoanCardId').val(response.data.loanCard.loanCardId);
            $('#editStudentId').val(response.data.loanCard.studentId);
            var loanOfDateStr = response.data.loanCard.loanOfDate.toString();
            $('#LoanOfDate').val(loanOfDateStr.slice(0, 10));
            var returnOfDateStr = response.data.loanCard.returnOfDate.toString();
            $('#ReturnOfDate').val(returnOfDateStr.slice(0, 10));
            $('#Status').val(response.data.loanCard.statusId);
            $('#modalLoanCardTitle').text('CẬP NHẬT THẺ MƯỢN');
            $('#fromEditLoanCard').show();
            $('#checkStudentId').val(response.data.loanCard.studentId);
            loanCard.checkStudent();
            loanCard.drawData();
        }
    });
}

loanCard.save = function () {
    var loanCardId = parseInt($('#loanCard_id').text())
    var studentId = parseInt($('#StudentId').text());
    var checkDataBook = $('#listBooks').is(':empty');
    var saveObj = {};
    if (loanCardId > 0) {
        if ($('#fromEditLoanCard').valid()) {
            saveObj.loanCardId = parseInt($('#loanCard_id').text());
            saveObj.studentId = parseInt($('#editStudentId').val());
            saveObj.loanOfDate = $('#LoanOfDate').val();
            saveObj.returnOfDate = $('#ReturnOfDate').val();
            $.ajax({
                url: '/LoanCard/Save',
                method: "POST",
                contentType: 'application/json',
                data: JSON.stringify(saveObj),
                success: function (response) {
                    if (response.data.loanCardId > 0)
                        window.location.href = `/LoanCard/Details/${response.data.loanCardId}`;
                    else {
                        $('#msgResult').text(`${response.data.message}`);
                        $('#msgResult').show();
                    }
                }
            });
        }
    } else {
        if (studentId > 0 && !checkDataBook) {
            saveObj.studentId = parseInt($('#StudentId').text());
            $.ajax({
                url: `/LoanCard/Save`,
                method: "POST",
                contentType: 'application/json',
                data: JSON.stringify(saveObj),
                success: function (response) {
                    if (response.data.loanCardId > 0) {
                        window.location.href = `/LoanCard/Details/${response.data.loanCardId}`;
                    } else {
                        $('#msgResult').text(`${response.data.message}`);
                        $('#msgResult').show();
                    }
                }
            });

        } else if (studentId > 0) {
            $('#msgResult').text('Bạn chưa có thông tin về Danh sách mượn!');
            $('#msgResult').show();
        } else if (!checkDataBook) {
            $('#msgResult').text('Bạn chưa có thông tin về Học sinh!');
            $('#msgResult').show();
        } else {
            $('#msgResult').text('Bạn chưa có bất cứ thông tin nào để tạo Thẻ mượn!');
            $('#msgResult').show();
        }
    }
}

loanCard.addBook = function () {
    $('#msgResult').hide();
    var bookId = parseInt($('#checkBookId').val());
    if (bookId > 0) {
        $.ajax({
            url: `/LoanCard/AddCartBook/${bookId}`,
            method: 'POST',
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (response) {
                $('#msgdataBook').hide();
                if (response.data != null) {
                    $('#listBooks').empty();
                    $.each(response.data, function (i, v) {
                        $('#listBooks').append(
                            `<div class="col-sm-6">
                                <br/>
                                <p class="text-break"><b>Tên sách:</b> ${v.bookName}</p>
                                <p class="text-break"><b>Tác giả:</b> ${v.author}</p>
                                <p class="text-break"><b>Thể loại:</b> ${v.categoryName}</p>
                                <hr/>
                                <br/>
                            </div>
                            <div class="col-4">
                                <img src="/img/${v.imagePath}" class="img_book" />
                            </div>
                            <div class="col-2">
                                <a href="javascript:void(0)" onclick="loanCard.deleteBook(${v.bookId})" title="Xóa"><i class="fa fa-times-circle text-danger" style="width: 40px; height: 40px; margin-top: 30px;"></i></a>
                            </div>`
                        );
                    });
                }
                if (response.message != null) {
                    $('#msgdataBook').text(response.message);
                    $('#msgdataBook').show();
                }
            }
        });
    }
    else {
        $('#msgdataBook').text("ID sách không tồn tại !");
        $('#msgdataBook').show();
    }
}

loanCard.deleteBook = function (id) {
    console.log(id);
    if (id > 0) {
        $.ajax({
            url: `/LoanCard/DeleteCartBook/${id}`,
            method: 'PATCH',
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (response) {
                console.log(response);
                if (response) {
                    loanCard.drawData();
                }
                else {
                    $('#msgdataBook').text("Thao tác không thực hiện được. Vui lòng thử lại !");
                    $('#msgdataBook').show();
                }
            }
        });
    }
}

loanCard.drawData = function () {
    $('#msgResult').hide();
    $.ajax({
        url: '/LoanCard/DataCartBook',
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            console.log(response);
            $('#msgdataBook').hide();
            if (response.data != null) {
                $('#listBooks').empty();
                $.each(response.data, function (i, v) {
                    $('#listBooks').append(
                        ` <div class="col-sm-6">
                               <br/>
                                <p class="text-break"><b>Tên sách:</b> ${v.bookName}</p>
                                <p class="text-break"><b>Tác giả:</b> ${v.author}</p>
                                 <p class="text-break"><b>Thể loại:</b> ${v.categoryName}</p>
                                <hr/>
                                <br/>
                            </div>
                            <div class="col-4">
                                <img src="/img/${v.imagePath}" class="img_book"/>
                            </div>
                            <div class="col-2">
                                <a href="javascript:void(0)" onclick="loanCard.deleteBook(${v.bookId})" title="Xóa"><i class="fa fa-times-circle text-danger" style="width: 40px; height: 40px; margin-top: 30px;"></i></a>
                            </div>`
                    );
                });
            }
            if (response.message != null) {
                $('#msgdataBook').text(response.message);
                $('#msgdataBook').show();
            }
        }
    });
}

loanCard.checkStudent = function () {
    $('#msgResult').hide();
    if ($('#fromCheckStudent').valid()) {
        var studentId = parseInt($('#checkStudentId').val());
        $.ajax({
            url: `/student/get/${studentId}`,
            method: 'GET',
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (response) {
                console.log(response.data);
                if (response.data != null) {
                    $('#msgInfo').hide();
                    $('#dataInfoStudent').show();
                    $('#StudentId').text(response.data.studentId);
                    $('#StudentName').text(response.data.studentName);
                    $('#CourseName').text(response.data.courseName);
                    var genderName = response.data.gender == true ? 'Nam' : 'Nữ';
                    $('#Gender').text(genderName);
                    $('#Dob').text(response.data.dobStr);
                    $('#PhoneNumber').text(response.data.phoneNumber);
                    $('#Email').text(response.data.email);
                    $('#Address').text(response.data.addressStr);
                    $('#StatusName').text(response.data.statusName);
                }
                else {
                    $('#StudentId').empty();
                    $('#msgInfo').show();
                    $('#msgInfo').text('Không có thông tin về Học sinh!');
                    $('#dataInfoStudent').hide();
                }
               
            }
        });
    }
}

loanCard.checkBook = function () {
    $('#msgResult').hide();
    if ($('#fromCheckBook').valid()) {
        var bookId = parseInt($('#checkBookId').val());
        $.ajax({
            url: `/book/get/${bookId}`,
            method: 'GET',
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (response) {
                $('#dataCheckBook').show();
                if (response.data != null) {
                    $('#msgInfoBook').hide();
                    $('#dataInfoBook').show();
                    $('#BookId').text(response.data.bookId);
                    $('#BookName').text(response.data.bookName);
                    $('#CategoryName').text(response.data.categoryName);
                    $('#Author').text(response.data.author);
                    $('#DopStr').text(response.data.dopStr);
                    $('#StatusNameBook').text(response.data.statusName);
                    $('#ImgBook').attr('src', `/img/${response.data.imagePath}`);
                    $('#dataImg').show();
                }
                else {
                    $('#msgInfoBook').show();
                    $('#dataInfoBook').hide();
                    $('#msgInfoBook').text('Không có thông tin về Sách!');
                    $('#dataImg').hide();
                }

            }
        });
    }
}

loanCard.resetForm = function () {
    $('#fromCheckStudent').trigger('reset');
    $('#fromCheckBook').trigger('reset');
    $('#fromCheckStudent').validate().resetForm();
    $('#fromCheckBook').validate().resetForm();
    $('.infoStudent').empty();
    $('#listBooks').empty();
    $('.infoBook').empty();
    $('#fromEditLoanCard').trigger('reset');
    $('#fromEditLoanCard').validate().resetForm();
    $('#fromEditLoanCard').hide();
    $.ajax({
        url: '/LoanCard/ResetCartBook',
        method: 'PATCH',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
        }
    });
}

loanCard.openModal = function () {
    loanCard.resetForm();
    $('#modalLoanCardTitle').text('THÊM MỚI THẺ MƯỢN');
    $('#dataInfoStudent').hide();
    $('#msgInfo').hide();
    $('#msgInfoBook').hide();
    $('#dataCheckBook').hide();
    $('#dataImg').hide();
    $('#msgdataBook').hide();
    $('#msgResult').hide();
    $('#addEditLoanCardModal').modal('show');
}

loanCard.delete = function (id) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
        message: `Bạn có muốn xóa <b class="text-primary">Thẻ mượn</b> có <b class="text-success">ID: ${id}</b> này không?`,
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
                    url: `/LoanCard/Delete/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function () {
                        window.location.href = "/LoanCard/Index";
                    }
                });
            }
        }
    });
}

loanCard.changeStatusToCompleted = function (id) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Cảnh báo</h2>',
        message: `Bạn có muốn chuyển chuyển trạng thái <b class="text-primary">Hoàn thành</b> Thẻ mượn có <b class="text-success">ID: ${id}</b>?`,
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
                    url: `/LoanCard/ChangeStatusToCompleted/${id}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (data) {
                        console.log(data);
                        if (data) {
                            window.location.href = `/LoanCard/Index`;
                        }
                    }
                });
            }
        }
    });
}

loanCard.extendLoanCard = function (id) {
    bootbox.prompt({
        title: '<h2 class="text-danger">Gia hạn Thẻ mượn</h2>',
        message: `Bạn muốn gia hạn <b class='text-primary'>Thẻ mượn</b> có <b class='text-success'>ID: ${id}</b> thêm bao nhiêu ngày? <br>`,
        inputType: 'radio',
        inputOptions: [{
            text: '3 Ngày',
            value: 3,
        },
        {
            text: '7 Ngày',
            value: 7,
        }],
        callback: function (result) {
            if (result > 0) {
                $.ajax({
                    url: `/LoanCard/ExtendLoanCard/${id}/${result}`,
                    method: "PATCH",
                    contentType: 'JSON',
                    success: function (data) {
                        console.log(data);
                        if (data) {
                            window.location.href = `/LoanCard/Index`;
                        }
                    }
                });
            }  
        }
    });
}

$(document).ready(function () {
    $("#tbLoanCard").dataTable(
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
                    "orderable": false,
                    "searchable": false
                }
            ],
            "order": [[0, 'desc']]
        }
    );
});