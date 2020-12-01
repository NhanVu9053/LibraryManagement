

var bookArchive = {} || bookArchive;


bookArchive.showData = function () {
    $.ajax({
        url: '/bookArchive/gets',
        method: 'GET',
        dataType: 'JSON',
        success: function (response) {
           
            $('#tbbookArchive>tbody').empty();
            $.each(response.data, function (i, v) {
                $('#tbbookArchive>tbody').append(
                    `<tr>
                        <td>${v.bookArchiveId}</td>
                        <td>${v.bookId}</td>
                        <td>${v.bookName}</td>
                        <td>${v.categoryName}</td>
                        <td>${v.quantity}</td>
                        <td>${v.quantityRemain}</td>                        
                        <td>${v.modifiedDateStr}</td>
                        <td>${v.modifiedBy}</td>
                        <td>${v.statusName}</td>
                        <td>
                             <a href="javascript:;" class="text-warning  ml-2" onclick="bookArchive.edit(${v.bookArchiveId},${true})"><i class="fa fa-plus-circle" aria-hidden="true"></i></a>
                             <a href="javascript:;" class="text-warning  ml-2" onclick="bookArchive.edit(${v.bookArchiveId},${false})"><i class="fa fa-minus-circle" aria-hidden="true"></i></a>
                             <a href="javascript:;" class="text-danger ml-2" onclick="bookArchive.delete(${v.bookArchiveId})"><i class='fas fa-trash'></i></a>
                        </td>
                    </tr>`
                );
            });
        }
    });
}

bookArchive.initStatus = function () {
    $.ajax({
        url: '/bookArchive/status/gets',
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

bookArchive.delete = function (bookArchiveId) {
    bootbox.confirm({
        title: '<h2 class="text-danger">Warning</h2>',
        message: `Do you want to <b class="text-primary">Delete</b>  <b class="text-success"></b> `,
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
                    url: `/bookArchive/delete/${bookArchiveId}`,
                    method: 'PATCH',
                    dataType: 'JSON',
                    contentType: 'application/json',
                    success: function (response) {

                        bootbox.alert(`<h4 class="alert alert-danger">${response.data.message} !!!</h4>`);
                        if (response.data.bookArchiveId > 0) {
                            $('#addEditbookArchiveModal').modal('hide');
                            bookArchive.showData();
                        }
                    }
                });
            }
        }
    });
}


bookArchive.save = function () {

    if ($('#fromAddEditbookArchive').valid()) {
        var saveObj = {};
        var xx = $('#bookArchiveId').val();

        var bookArchiveId = parseInt(xx);
        saveObj.BookArchiveId = bookArchiveId;
        
        saveObj.Value = parseInt($('#value').val());       
        saveObj.StatusId = parseInt($('#Status').val());
        $.ajax({
            url: '/bookarchive/save',
            method: 'POST',
            dataType: 'JSON',
            contentType: 'application/json',
            data: JSON.stringify(saveObj),
            success: function (response) {
                bootbox.alert(`<h2 class="text-success">${response.data.message}</h2>`);
                if (response.data.bookArchiveId > 0) {
                    $('#addEditbookArchiveModal').modal('hide');
                    bookArchive.showData();
                }
            }
        });
    }
}

bookArchive.edit = function (id,isPlus) {
    $.ajax({
        url: `get/${id}`,
        method: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (reponse) {
            console.log(reponse);
            $('#bookArchiveId').val(reponse.data.bookArchiveId);
            $('#value').val(reponse.data.value);
            
            //$('#CategoryName').val(reponse.data.categoryName);
            $('#Status').val(reponse.data.status);
            $('#addEditbookArchiveModal').find('.modal-title').text('Update bookArchive');
            $('#addEditbookArchiveModal').modal('show');
        }
    });

};

bookArchive.openModal = function () {

    document.getElementById('msg').style.display = 'none';
    $('#addEditbookArchiveModal').modal('show');
}


bookArchive.init = function () {
    bookArchive.showData();
    bookArchive.initStatus();
}

$(document).ready(function () {
    bookArchive.init();
});

