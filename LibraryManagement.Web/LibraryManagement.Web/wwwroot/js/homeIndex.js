function initCategory() {
    $.ajax({
        url: `/book/getsCategory`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            $.each(response.data, function (i, v) {
                $('#categories').append(
                    `<li><a href="/Home/GetBookByCategory/${v.categoryId}"><img alt="" src="~/Assets/tmart/images/icons/thum10.png"> ${v.categoryName}</a></li>`
                );
            });
        }
    });
}

function initTopBookLoan() {
    $.ajax({
        url: `/book/topLoan`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            $.each(response.data, function (i, v) {
                $('#topLoanBook').append(
                    `<div class="col-md-4 single__pro col-lg-4 cat--1 col-sm-4 col-xs-12">
                        <div class="product__details">
                            <div class="product__inner">
                                <a href="/Home/ViewProduct/${v.bookId}">
                                    <img src="/img/${v.imagePath}" alt="blog images" />
                                </a>
                                <div class="product__price">
                                    <span style="text-align: center; font-size: 1.5em; margin-left: 140px;">${v.statusName}</span>
                                </div>
                            </div>
                        </div>
                    </div>`
                );
            });
        }
    });
}
function initTopNewBook() {
    $.ajax({
        url: `/book/topNew`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            $.each(response.data, function (i, v) {
                $('#topNewBook').append(
                    `<div class="col-md-4 single__pro col-lg-4 cat--1 col-sm-4 col-xs-12">
                        <div class="product__details">
                            <div class="product__inner">
                                <a href="/Home/ViewProduct/${v.bookId}">
                                    <img src="/img/${v.imagePath}" alt="blog images" />
                                </a>
                                <div class="product__price">
                                    <span style="text-align: center; font-size: 1.5em; margin-left: 140px;">${v.statusName}</span>
                                </div>
                            </div>
                        </div>
                    </div>`
                );
            });
        }
    });
}
function initRandomBook() {
    $.ajax({
        url: `/book/topNew`,
        method: 'GET',
        dataType: 'JSON',
        contentType: 'application/json',
        success: function (response) {
            $.each(response.data, function (i, v) {
                $('#randomBook').append(
                    `<div class="col-md-4 single__pro col-lg-4 cat--1 col-sm-4 col-xs-12">
                        <div class="product__details">
                            <div class="product__inner">
                                <a href="/Home/ViewProduct/${v.bookId}">
                                    <img src="/img/${v.imagePath}" alt="blog images" />
                                </a>
                                <div class="product__price">
                                    <span style="text-align: center; font-size: 1.5em; margin-left: 140px;">${v.statusName}</span>
                                </div>
                            </div>
                        </div>
                    </div>`
                );
            });
        }
    });
}
$(document).ready(function () {
    initCategory();
    initTopBookLoan();
    initTopNewBook();
    initRandomBook();
});