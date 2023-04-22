$(document).ready(function () {
    getAllTaikhoan();
});

function getAllTaikhoan() {
    $.ajax({
        url: `https://localhost:7269/api/apitaikhoan`,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            var count = parseInt(response.totalCount);
            const pageNumber = 1;
            const pageSize = 5;
            $.ajax({
                url: `https://localhost:7269/api/apitaikhoan/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
                method: 'GET',
                contentType: 'json',
                dataType: 'json',
                error: function (response) {
                    console.log("error");
                },
                success: function (response) {
                    renderTable(response);
                    renderPagination(Math.ceil(count / pageSize), pageNumber);
                },
                fail: function (response) {
                    console.log("fail");
                }
            });
        },
    })
}
$("#form-taikhoan").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_taikhoan').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/apitaikhoan/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            renderTable(response);
        },
        fail: function (response) {
            console.log("fail");
        }
    });
}

function resetInput() {
    $("#userName").val("").change()
    $("#loai").val("").change()
    $("#password").val("").change()
    $("#confirmPassword").val("").change()
}
function InsertTK() {
    var UserName = $("#userName").val();
    var Loai = $("#loai").val();
    var Password = $("#password").val();
    var ConfirmPassword = $("#confirmPassword").val();

    var formData = new FormData();
    formData.append("userName", UserName);
    formData.append("loai", Loai);
    formData.append("password", Password);
    formData.append("confirmPassword", ConfirmPassword);

    var url = 'https://localhost:7269/api/apitaikhoan/themTK';
    $.ajax({
        url: url,
        method: 'POST',
        processData: false,
        contentType: false,
        data: formData,
        error: function (error) {
            alert("Có lỗi xảy ra");
        },
        success: function (response) {
            alert("Thêm mới thành công");
            resetInput();
            getAllTaikhoan(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function deleteTK(id) {
    var url = 'https://localhost:7269/api/Apitaikhoan?input=' + id;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            /* alert("Xóa không thành công");*/
            getAllTaikhoan();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllTaikhoan(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function renderTable(response) {
    const len = response.items.length;
    let table = '';
    let cls = "table-success";
    for (var i = 0; i < len; ++i) {
        if (i % 2 == 0) {
            cls = "table-primary";
        }
        else {
            cls = "table-success";
        }
        table = table + '<tr class="' + cls + '">';
        table = table + '<td>' + response.items[i].userName.trim() + '</td>';
        table = table + `<td>${response.items[i].loai == 1 ? 'Admin': 'Khách'}</td>`;
       /* table = table + '<td>' + response.items[i].loai + '</td>';*/
        table = table + '<td>' + response.items[i].password.trim() + '</td>';
        table = table + '<td>' + response.items[i].confirmPassword.trim() + '</td>';
        
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteTK(\'' + response.items[i].userName.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-taikhoan').innerHTML = table;
}