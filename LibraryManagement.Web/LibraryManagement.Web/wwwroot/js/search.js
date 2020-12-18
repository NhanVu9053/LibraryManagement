searchjs = function (string) {
    alert(string);
    $.ajax({
        url: `home/search/${string}`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
          
        }
    });
}