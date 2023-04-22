$(document).ready(function () {
    getAllDiemThamQuan();
});

function getAllDiemThamQuan() {
    $.ajax({
        url: `https://localhost:7269/api/APIDiemThamQuan`,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            var count = parseInt(response.totalCount);
            const pageNumber = 1;
            const pageSize = 3;
            $.ajax({
                url: `https://localhost:7269/api/APIDiemThamQuan/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
$("#form-diemthamquan").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_diemthamquan').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 3;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/APIDiemThamQuan/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
    $("#MaDd").val("").change()
    $("#tendd").val("").change()
    $("#mien").val("").change()
    $("#mota").val("").change()
    $("#avatar").val("").change()
}
function InsertDiemThamQuan() {
    var maDd = $("#MaDd").val();
    var tendd = $("#tendd").val();
    var mien = $("#mien").val();
    var mota = $("#mota").val();
    var formData = new FormData();

    formData.append("maDd", maDd);
    formData.append("tendiadiem", tendd);
    formData.append("mien", mien);
    formData.append("moTa", mota);
    formData.append("tenFileAnh", $("#avatar")[0].files[0]);

    var url = 'https://localhost:7269/api/APIDiemThamQuan/themDTQ';
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
            getAllDiemThamQuan(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}
function UpdateDTQ() {
    var maDd = $("#MaDd").val();
    var tendd = $("#tendd").val();
    var mien = $("#mien").val();
    var mota = $("#mota").val();
    var formData = new FormData();

    formData.append("maDd", maDd);
    formData.append("tendiadiem", tendd);
    formData.append("mien", mien);
    formData.append("moTa", mota);
    formData.append("tenFileAnh", $("#avatar")[0].files[0]);

    var url = 'https://localhost:7269/api/APIDiemThamQuan/capnhatDTQ';
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
            getAllDiemThamQuan(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function updateDTQFill(id) {
    var url = 'https://localhost:7269/api/ApiDiemThamQuan/getById?id=' + id;
    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            alert("Cập nhật không thành công");
        },
        success: function (response) {
            $("#MaDd").val(response.maDd.trim())
            $("#tendd").val(response.tendiadiem.trim()).change()
            $("#mien").val(response.mien.trim()).change()
            $("#mota").val(response.moTa.trim()).change()
            $("#avatar").val(response.tenFileAnh.trim()).change()
        }
    });
}

function deleteDTQ(id) {
    var url = 'https://localhost:7269/api/ApiDiemThamQuan?input=' + id;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
           /* alert("Xóa không thành công");*/
            getAllDiemThamQuan();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllDiemThamQuan(); //Gọi đến hàm lấy dữ liệu lên bảng
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
        table = table + '<td>' + response.items[i].maDd.trim() + '</td>';
        table = table + '<td>' + response.items[i].tendiadiem.trim() + '</td>';
        table = table + '<td>' + response.items[i].mien.trim() + '</td>';
        table = table + `<td class="py-1">
                    <img src="../../img/anhDLTC/${!!response.items[i].tenFileAnh ? response.items[i].tenFileAnh.trim() : 'default-avatar.png'}" alt="image" />
                </td>`;
       /* table = table + '<td>' + response.items[i].moTa.trim() + '</td>';*/

        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-info btn-rounded btn-icon" onclick="updateDTQFill(\'' + response.items[i].maDd.trim() + '\')">Edit</i></button> ' + '</td>';
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteDTQ(\'' + response.items[i].maDd.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-diemthamquan').innerHTML = table;
}