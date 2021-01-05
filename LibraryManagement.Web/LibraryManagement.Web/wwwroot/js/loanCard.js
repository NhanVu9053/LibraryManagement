var loanCard = {} || loanCard;
var table = $('#tbLoanCard').DataTable();

loanCard.showData = function () {
    $.ajax({
        url: '/loanCard/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
            table.clear().destroy();
            $('#tbLoanCard>tbody').empty();
            $.each(response.data, function (i, v) {
                if (v.statusName != 'Deleted') {
                    var actions = "";
                    switch (v.statusId) {
                        case 4:
                            {
                                actions = `<a href='javascript:void(0)' class='text-danger ml-2' onclick='loanCard.delete(${v.loanCardId})' title='Xóa'><i class='fas fa-trash'></i></a>`;
                                break;
                            }
                        default:
                            {

                                actions = `<a href="javascript:void(0)" class="text-warning ml-2" onclick="loanCard.edit(${v.loanCardId})" title="Cập nhật"><i class="fas fa-edit"></i></a>`
                                actions += `<a href='javascript: void(0)' class='text-dark ml-2' onclick='loanCard.extendLoanCard(${v.loanCardId})' title='Gia hạn'><i class='fa fa-plus-circle'></i></a>`;
                                actions += `<a href='javascript: void(0)' class='text-success ml-2' onclick='loanCard.changeStatusToCompleted(${v.loanCardId})' title='Hoàn thành'><i class='fas fa-check-circle'></i></a>`;
                                break;
                            }
                    }
                    $('#tbLoanCard>tbody').append(
                        `<tr>
                            <td>${v.loanCardId}</td>
                            <td>${v.loanOfDateStr}</td>
                            <td>${v.returnOfDateStr}</td>
                            <td>${v.studentId}</td>
                            <td>${v.books}</td>
                            <td class="text-center">
                                <span class="${(v.statusId == 1? "btn btn-primary": (v.statusId == 2? "btn btn-warning" : (v.statusId == 3? "btn btn-danger" : (v.statusId == 3? "btn btn-success" : "btn btn-info"))))}"
                                      style="width: 110px; height: 40px;">
                                    ${v.statusName}
                                </span>
                            </td>
                            <td>
                                <a href="javascript:void(0)"  onclick="loanCard.details(${v.loanCardId})" class="text-primary ml-2" title="Chi tiết"><i class="fas fa-eye"></i></a>
                                ${actions}
                            </td>
                        </tr>`
                    );
                }
            });
            loanCard.drawDataTable();
        }
    });
}

loanCard.details = function (loanCardId) {
    $('#dataModal').empty();
    $.ajax({
        url: `/loanCard/get/${loanCardId}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            if (response.data.loanCard.loanCardId > 0) {
                $('#dataModalTitle').text('THÔNG TIN THẺ MƯỢN');
                var dataBook = '';
                $.each(response.data.bookList, function (i, v) {
                    dataBook += `<div class="row">
                                    <div class="col-sm-6 text-break">
                                        <br />
                                        <p><b>Tên sách:</b> ${v.bookName}</p>
                                        <p><b>Tác giả:</b>${v.author}</p>
                                        <p><b>Thể loại:</b> ${v.categoryName}</p>
                                    </div>
                                    <div class="col-sm-6 text-break text-center">
                                        <img src="/img/${v.imagePath}" class="img_book" />
                                    </div>
                                </div>
                                <hr />`;
                })
                $('#dataModal').append(
                    `<div class="row justify-content-center col-xl-12">
                        <div class="col-xl-6 col-md-12">
                            <div class="card">
                                <div class="card-body text-break">
                                    <p><b>Mã ID: </b> ${response.data.loanCard.loanCardId}</p>
                                    <p><b>Ngày mượn: </b> ${response.data.loanCard.loanOfDateStr}</p>
                                    <p><b>Ngày trả: </b> ${response.data.loanCard.returnOfDateStr}</p>
                                    <p><b>Mã học sinh: </b> ${response.data.loanCard.studentId}</p>
                                    <p><b>Tên học sinh: </b> ${response.data.loanCard.studentName}</p>
                                    <p><b>Lớp: </b> ${response.data.loanCard.courseName}</p>
                                    <p><b>Ngày tạo: </b> ${response.data.loanCard.createdDateStr}</p>
                                    <p><b>Người tạo: </b> ${response.data.loanCard.createdByName}</p>
                                    <p><b>Ngày sửa: </b> ${response.data.loanCard.modifiedDateStr}</p>
                                    <p><b>Người sửa: </b> ${response.data.loanCard.modifiedByName}</p>
                                    <p><b>Trạng thái: </b> ${response.data.loanCard.statusName}</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-6 col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="text-primary text-center">DANH SÁCH MƯỢN</h4>
                                    ${dataBook}
                                </div>
                            </div>
                        </div>
                    </div>`
                );
                $('#detailsData').modal('show');
            }
            else {
                bootbox.alert(`<h5 class="text-danger">Thẻ mượn này không tồn tại !!!</h5>`)
            }
        }
    });
}

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
            $('#inputCheckStudent').hide();
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
                    if (response.data.loanCardId > 0) {
                        bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                            $('#addEditLoanCardModal').modal('hide');
                            loanCard.showData();
                        });
                    }
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
                        bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                            $('#addEditLoanCardModal').modal('hide');
                            loanCard.showData();
                        });
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
    if (id > 0) {
        $.ajax({
            url: `/LoanCard/DeleteCartBook/${id}`,
            method: 'PATCH',
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (response) {
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
            $('#msgdataBook').hide();
            if (response.data != null) {
                $('#listBooks').empty();
                $.each(response.data, function (i, v) {
                    $('#listBooks').append(
                        ` <div class=" row col-12">
                            <div class="col-sm-6">
                                <p class="text-break"><b>Tên sách:</b> ${v.bookName}</p>
                                <p class="text-break"><b>Tác giả:</b> ${v.author}</p>
                                 <p class="text-break"><b>Thể loại:</b> ${v.categoryName}</p>
                            </div>
                            <div class="col-4">
                                <img src="/img/${v.imagePath}" class="img_book"/>                             
                            </div>
                            <div class="col-2">
                                <a href="javascript:void(0)" onclick="loanCard.deleteBook(${v.bookId})" title="Xóa"><i class="fa fa-times-circle text-danger" style="width: 40px; height: 40px; margin-top: 30px; margin-left: 15px"></i></a>
                            </div>
                        </div>
                            <div class="col-10">
                               <hr/>
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
                    $('#imgStudent').attr('src', `/img/${response.data.avatarPath}`);
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
loanCard.delete = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
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
                    success: function (response) {
                        if (response.data.loanCardId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditLoanCardModal').modal('hide');
                                loanCard.showData();
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

loanCard.changeStatusToCompleted = function (id) {
    bootbox.confirm({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
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
                    success: function (response) {
                        if (response.data.loanCardId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditLoanCardModal').modal('hide');
                                loanCard.showData();
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

loanCard.extendLoanCard = function (id) {
    bootbox.prompt({
        title: '<h4 class="text-danger">THÔNG BÁO</h4>',
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
                    success: function (response) {
                        if (response.data.loanCardId > 0) {
                            bootbox.alert(`<h5 class="text-success">${response.data.message} !!!</h5>`, function () {
                                $('#addEditLoanCardModal').modal('hide');
                                loanCard.showData();
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
    $('#inputCheckStudent').show();
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

loanCard.drawDataTable = function () {
    table = $("#tbLoanCard").DataTable(
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
};

loanCard.init = function () {
    loanCard.showData();
}

$(document).ready(function () {
    loanCard.init();
});