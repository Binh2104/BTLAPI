$(document).ready(function () {
    GetAllKhachHang();
});

function GetAllKhachHang() {
    $.ajax({
        url: `https://localhost:7269/api/apikhachhang`,
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
                url: `https://localhost:7269/api/apikhachhang/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
$("#form-khachhang").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_khachhang').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/apikhachhang/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
    $("#MaKH").val("").change()
    $("#TenKh").val("").change()
    $("#gioitinh").val("").change()
    $("#sdt").val("").change()
    $("#diachi").val("").change()
    $("#UserName").val("").change()
    $("#Email").val("").change()
}
function InsertKH() {
    var maNv = $("#MaKH").val();
    var tennv = $("#TenKh").val();
    var gioitinh = $("#gioitinh").val();
    var sodienthoai = $("#sdt").val();
    var diachi = $("#diachi").val();
    var UserName = $("#UserName").val();
    var Email = $("#Email").val();

    var formData = new FormData();
    formData.append("maKh", maNv);
    formData.append("tenKh", tennv);
    formData.append("gioiTinh", gioitinh);
    formData.append("sdt", sodienthoai);
    formData.append("diaChi", diachi);
    formData.append("userName", UserName);
    formData.append("email", Email);



    var url = 'https://localhost:7269/api/ApiKhachHang/themKH';
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
            GetAllKhachHang(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}
function UpdateKH() {
    var maKh = $("#MaKH").val();
    var tenkh = $("#TenKh").val();
    var gioitinh = $("#gioitinh").val();
    var sodienthoai = $("#sdt").val();
    var diachi = $("#diachi").val();
    var UserName = $("#UserName").val();
    var Email = $("#Email").val();

    var formData = new FormData();
    formData.append("maKh", maKh);
    formData.append("tenKh", tenkh);
    formData.append("gioiTinh", gioitinh);
    formData.append("sdt", sodienthoai);
    formData.append("diaChi", diachi);
    formData.append("userName", UserName);
    formData.append("email", Email);


    var url = 'https://localhost:7269/api/apikhachhang/capnhatKH';
    $.ajax({
        url: url,
        method: 'PUT',
        processData: false,
        contentType: false,
        data: formData,
        error: function (error) {
            alert("Có lỗi xảy ra");
        },
        success: function (response) {
            alert("Cập nhật thành công");
            resetInput();
            GetAllKhachHang(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function updateKHFill(id) {
    var url = 'https://localhost:7269/api/ApiKhachHang/getById?id=' + id;
    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            alert("Cập nhật không thành công");
        },
        success: function (response) {

            $("#MaKH").val(response.maKh.trim()).change()
            $("#TenKh").val(response.tenKh.trim()).change()
            $("#gioitinh").val(response.gioiTinh.trim()).change()
            $("#sdt").val(response.sdt.trim()).change()
            $("#diachi").val(response.diaChi.trim()).change()
            $("#UserName").val(response.userName.trim()).change()
            $("#Email").val(response.email.trim()).change()
        }

    });
}

function deleteKH(id) {
    var url = 'https://localhost:7269/api/ApiKhachHang?input=' + id;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            /* alert("Xóa không thành công");*/
            GetAllKhachHang();
        },
        success: function (response) {
            alert("Xóa thành công");
            GetAllKhachHang(); //Gọi đến hàm lấy dữ liệu lên bảng
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
        table = table + '<td>' + response.items[i].maKh.trim() + '</td>';
        table = table + '<td>' + response.items[i].tenKh.trim() + '</td>';
        table = table + '<td>' + response.items[i].gioiTinh.trim() + '</td>';
        table = table + '<td>' + response.items[i].sdt.trim() + '</td>';
        table = table + '<td>' + response.items[i].diaChi.trim() + '</td>';
        table = table + '<td>' + response.items[i].userName.trim() + '</td>';
        table = table + '<td>' + response.items[i].email.trim() + '</td>';

        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-info btn-rounded btn-icon" onclick="updateKHFill(\'' + response.items[i].maKh.trim() + '\')">Edit</i></button> ' + '</td>';
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteKH(\'' + response.items[i].maKh.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-khachhang').innerHTML = table;
}