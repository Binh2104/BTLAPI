$(document).ready(function () {
    getAllTintuc();
});

function getAllTintuc() {
    $.ajax({
        url: `https://localhost:7269/api/ApiTinTuc`,
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
                url: `https://localhost:7269/api/ApiTinTuc/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
$("#form-tintuc").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_tintuc').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/APITinTuc/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
    $("#MaTinTuc").val("").change()
    $("#MaNV").val("").change()
    $("#avatar").val("").change()
    $("#Mota").val("").change()
    $("#NoiDung").val("").change()
   
}
function InsertTinTuc() {
    var maTT = $("#MaTinTuc").val();
    var maNV = $("#MaNV").val();
    var MoTa = $("#Mota").val();
    var NoiDung = $("#NoiDung").val();

    var formData = new FormData();

    formData.append("maTin", maTT);
    formData.append("maNv", maNV);
    formData.append("anh", $("#avatar")[0].files[0]);
    formData.append("moTa", MoTa);
    formData.append("noiDung", NoiDung);
    

    var url = 'https://localhost:7269/api/APITinTuc/themTT';
    $.ajax({
        url: url,
        method: 'POST',
        processData: false,
        contentType: false,
        data: formData,
        error: function (error) {
            alert("Có lỗi xảy ra");
            getAllTintuc();
        },
        success: function (response) {
            alert("Thêm mới thành công");
            resetInput();
            getAllTintuc(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function UpdateTT() {
    var maTT = $("#MaTinTuc").val();
    var maNV = $("#MaNV").val();

    var MoTa = $("#Mota").val();
    var NoiDung = $("#NoiDung").val();
    var formData = new FormData();

    formData.append("maTin", maTT);
    formData.append("maNv", maNV);

    formData.append("anh", $("#avatar")[0].files[0]);
    formData.append("moTa", MoTa);
    formData.append("noiDung", NoiDung);

    var url = 'https://localhost:7269/api/APITinTuc/capnhatTT';
    $.ajax({
        url: url,
        method: 'PUT',
        processData: false,
        contentType: false,
        data: formData,
        error: function (error) {
            alert("Có lỗi xảy ra");
            getAllTintuc();
        },
        success: function (response) {
            alert("Cập nhật thành công");
            resetInput();
            getAllTintuc(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function updateTTFill(id) {
    var url = 'https://localhost:7269/api/APITinTuc/getById?id=' + id;
    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            alert("Cập nhật không thành công");
        },
        success: function (response) {
            $("#MaTinTuc").val(response.maTin.trim())
            $("#MaNV").val(response.maNv.trim())
            $("#avatar").val(response.anh.trim()).change()
            $("#Mota").val(response.moTa.trim())
            $("#noidung").val(response.noiDung.trim())
            /*$("#avatar").val(response.tenFileAnh.trim()).change()*/
        }
    });
}

function deleteTT(id) {
    var url = 'https://localhost:7269/api/ApiTinTuc?input=' + id;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            /* alert("Xóa không thành công");*/
            getAllTintuc();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllTintuc(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function renderTable(response) {
    const len = response.items.length;
    let table = '';
    for (var i = 0; i < len; ++i) {
        table = table + '<tr>';
        table = table + '<td>' + response.items[i].maTin.trim() + '</td>';
        table = table + '<td>' + response.items[i].maTin.trim() + '</td>';
        table = table + `<td class="py-1">
                    <img src="../../img/AnhTinTuc/${!!response.items[i].anh ? response.items[i].anh.trim() : 'default-avatar.png'}" alt="image" />
                </td>`;    
        table = table + '<td>' + response.items[i].moTa.trim() + '</td>';
         table = table + '<td>' + response.items[i].noiDung.trim() + '</td>';

        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-info btn-rounded btn-icon" onclick="updateTTFill(\'' + response.items[i].maTin.trim() + '\')">Edit</i></button> ' + '</td>';
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick=" deleteTT(\'' + response.items[i].maTin.trim() + '\')">Delete</button> ' + '</td>';
       /* table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="ChiTietTT(\'' + response.items[i].maTin.trim() + '\')">Chi Tiết</button> ' + '</td>';*/
    }
    document.getElementById('tbody-tintuc').innerHTML = table;
}
