$(document).ready(function () {
    getAllNhanvien();
});

function getAllNhanvien() {
    $.ajax({
        url: `https://localhost:7269/api/apinhanvien`,
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
                url: `https://localhost:7269/api/apinhanvien/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
$("#form-nhanvien").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_nhanvien').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/apinhanvien/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
    $("#MaNV").val("").change()
    $("#TenNV").val("").change()
    $("#gioitinh").val("").change()
    $("#sdt").val("").change()
    $("#diachi").val("").change()
    $("#UserName").val("").change()
}
function InsertNV() {
    var maNv = $("#MaNV").val();
    var tennv = $("#TenNV").val();
    var gioitinh = $("#gioitinh").val();
    var sodienthoai = $("#sdt").val();
    var diachi = $("#diachi").val();
    var UserName = $("#UserName").val();
    var formData = new FormData();
    formData.append("maNv", maNv);
    formData.append("tenNv", tennv);
    formData.append("gioiTinh", gioitinh);
    formData.append("soDienThoai", sodienthoai);
    formData.append("diaChi", diachi);
    formData.append("userName", UserName);


    var url = 'https://localhost:7269/api/apinhanvien/themNV';
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
            getAllNhanvien(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}
function UpdateNV() {   
    var maNv = $("#MaNV").val();
    var tennv = $("#TenNV").val();
    var gioitinh = $("#gioitinh").val();
    var sodienthoai = $("#sdt").val();
    var diachi = $("#diachi").val();
    var UserName = $("#UserName").val();
    var formData = new FormData();
    formData.append("maNv", maNv);
    formData.append("tenNv", tennv);
    formData.append("gioiTinh", gioitinh);
    formData.append("soDienThoai", sodienthoai);
    formData.append("diaChi", diachi);
    formData.append("userName", UserName);

    var url = 'https://localhost:7269/api/apinhanvien/capnhatNV';
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
            getAllNhanvien(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function updateNVFill(id) {
    var url = 'https://localhost:7269/api/ApiNhanVien/getById?id=' + id;
    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            alert("Cập nhật không thành công");
        },
        success: function (response) {

            $("#MaNV").val(response.maNv.trim())
            $("#TenNV").val(response.tenNv.trim()).change()
            $("#gioitinh").val(response.gioiTinh.trim()).change()
            $("#sdt").val(response.soDienThoai.trim()).change()
            $("#diachi").val(response.diaChi.trim()).change()           
            $("#UserName").val(response.userName.trim()).change()           
        }
       
    });
}

function deleteNV(id) {
    var url = 'https://localhost:7269/api/ApiNhanVien?input=' + id;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            /* alert("Xóa không thành công");*/
            getAllNhanvien();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllNhanvien(); //Gọi đến hàm lấy dữ liệu lên bảng
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
        table = table + '<td>' + response.items[i].maNv.trim() + '</td>';
        table = table + '<td>' + response.items[i].tenNv.trim() + '</td>';
        table = table + '<td>' + response.items[i].gioiTinh.trim() + '</td>';
        table = table + '<td>' + response.items[i].soDienThoai.trim() + '</td>';
        table = table + '<td>' + response.items[i].diaChi.trim() + '</td>';
        table = table + '<td>' + response.items[i].userName.trim() + '</td>';

        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-info btn-rounded btn-icon" onclick="updateNVFill(\'' + response.items[i].maNv.trim() + '\')">Edit</i></button> ' + '</td>';
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteNV(\'' + response.items[i].maNv.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-nhanvien').innerHTML = table;
}