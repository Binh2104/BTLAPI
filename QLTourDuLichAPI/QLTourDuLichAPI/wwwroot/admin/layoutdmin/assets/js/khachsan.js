$(document).ready(function () {
    getAllKhachsan();
});

function getAllKhachsan() {
    $.ajax({
        url: `https://localhost:7269/api/APIKhachSan`,
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
                url: `https://localhost:7269/api/APIKhachSan/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
        }
    });
}
$("#form-khachsan").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_khachsan').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/APIKhachSan/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
    $("#MaKS").val("").change()
    $("#TenKS").val("").change()
    $("#DiaChi").val("").change()
    $("#sdt").val("").change()
    $("#XHKS").val("").change()
    $("#Anh").val("").change()
}
function InsertKS() {
    var maDd = $("#MaKS").val();
    var tendd = $("#TenKS").val();
    var mien = $("#DiaChi").val();
    var mota = $("#sdt").val();
    var mota = $("#XHKS").val();
    var formData = new FormData();

    formData.append("maKs", maDd);
    formData.append("tenKs", tendd);
    formData.append("diaChi", mien);
    formData.append("sdt", mota);
    formData.append("xepHangKs", mota);
    formData.append("tenFileAnh", $("#Anh")[0].files[0]);

    var url = 'https://localhost:7269/api/APIKhachSan/themKS';
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
            getAllKhachsan(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}
function UpdateKS() {
    var MaKS = $("#MaKS").val();
    var TenKS = $("#TenKS").val();
    var DiaChi = $("#DiaChi").val();
    var sdt = $("#sdt").val();
    var XHKS = $("#XHKS").val();
    var formData = new FormData();

    formData.append("maKs", MaKS);
    formData.append("tenKs", TenKS);
    formData.append("diaChi", DiaChi);
    formData.append("sdt", sdt);
    formData.append("xepHangKs", XHKS);
    formData.append("tenFileAnh", $("#Anh")[0].files[0]);

    var url = 'https://localhost:7269/api/APIKhachSan/capnhatKS';
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
            getAllKhachsan(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function updateKSFill(id) {
    var url = 'https://localhost:7269/api/APIKhachSan/getById?id=' + id;
    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            alert("Cập nhật không thành công");
        },
        success: function (response) {
            $("#MaKS").val(response.maKs.trim())
            $("#TenKS").val(response.tenKs.trim()).change()
            $("#DiaChi").val(response.diaChi.trim()).change()
            $("#sdt").val(response.sdt.trim()).change()
            $("#XHKS").val(response.xepHangKs).change()
            $("#Anh").val(response.tenFileAnh.trim()).change()


        }
    });
}

function deleteKS(id) {
    var url = 'https://localhost:7269/api/APIKhachSan?input=' + id;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            /* alert("Xóa không thành công");*/
            getAllKhachsan();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllKhachsan(); //Gọi đến hàm lấy dữ liệu lên bảng
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
        table = table + '<td>' + response.items[i].maKs.trim() + '</td>';
        table = table + '<td>' + response.items[i].tenKs.trim() + '</td>';
        table = table + '<td>' + response.items[i].diaChi.trim() + '</td>';
        table = table + '<td>' + response.items[i].sdt.trim() + '</td>';
        table = table + '<td>' + response.items[i].xepHangKs + '</td>';
        table = table + `<td class="py-1">
                    <img src="../../img/KhachSan/${!!response.items[i].tenFileAnh ? response.items[i].tenFileAnh.trim() : 'default-Anh.png'}" alt="image" />
                </td>`;

        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-info btn-rounded btn-icon" onclick="updateKSFill(\'' + response.items[i].maKs.trim() + '\')">Edit</i></button> ' + '</td>';
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteKS(\'' + response.items[i].maKs.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-khachsan').innerHTML = table;
}