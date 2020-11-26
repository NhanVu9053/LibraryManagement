$(document).ready(function () {
    $("#tbCategory").dataTable(
        {
            "columnDefs": [
                {
                    "targets": 5,
                    "orderable": false,
                    "searchable": false
                }
            ]
        }
    );
});