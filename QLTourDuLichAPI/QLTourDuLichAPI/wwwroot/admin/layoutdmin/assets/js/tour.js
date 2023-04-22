$(document).ready(function () {
    getAllTour();
});

function getAllTour() {
    $.ajax({
        url: `https://localhost:7269/api/APITour`,
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
                url: `https://localhost:7269/api/APITour/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
$("#form-tour").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_Tour').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/APITour/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
    $("#MaTour").val("").change()
    $("#TenTour").val("").change()
    $("#GiaCho").val("").change()
    $("#XepHangTour").val("").change()
    $("#avartar").val("").change()
    $("#SoNgayDL").val("").change()
    $("#DXP").val("").change()
}
function InsertTour() {
    var matour = $("#MaTour").val();
    var tentour = $("#TenTour").val();
    var giacho = $("#GiaCho").val();
    var xh = $("#XepHangTour").val();
    var sndl = $("#SoNgayDL").val();
    var dxp = $("#DXP").val();

    var formData = new FormData();

    formData.append("maTour", matour);
    formData.append("tenTour", tentour);
    formData.append("giaCho", giacho);
    formData.append("xepHangTour", xh);
    formData.append("soNgayDl", sndl);
    formData.append("diemXuatPhat", dxp);
    formData.append("anh", $("#avatar")[0].files[0]);

    var url = 'https://localhost:7269/api/APITour/themTour';
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
            getAllTour(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}
function UpdateTour() {
    var matour = $("#MaTour").val();
    var tentour = $("#TenTour").val();
    var giacho = $("#GiaCho").val();
    var xh = $("#XepHangTour").val();
    var sndl = $("#SoNgayDL").val();
    var dxp = $("#DXP").val();

    var formData = new FormData();

    formData.append("maTour", matour);
    formData.append("tenTour", tentour);
    formData.append("giaCho", giacho);
    formData.append("xepHangTour", xh);
    formData.append("soNgayDl", sndl);
    formData.append("diemXuatPhat", dxp);
    formData.append("anh", $("#avatar")[0].files[0]);

    var url = 'https://localhost:7269/api/APITour/capnhatTour';
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
            getAllTour(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function updateTourFill(id) {
    var url = 'https://localhost:7269/api/ApiTour/getById?id=' + id;
    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            alert("Cập nhật không thành công");
        },
        success: function (response) {
            $("#MaTour").val(response.maTour.trim())
            $("#TenTour").val(response.tenTour.trim()).change()
            $("#GiaCho").val(response.giaCho).change()
            $("#XepHangTour").val(response.xepHangTour).change()
            $("#avatar").val(response.anh.trim()).change()
            $("#SoNgayDL").val(response.soNgayDl).change()
            $("#DXP").val(response.diemXuatPhat.trim()).change()
        }
    });
}

function deleteTour(id) {
    var url = 'https://localhost:7269/api/ApiTour?input=' + id;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
             /*alert("Xóa không thành công");*/
            getAllTour();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllTour(); //Gọi đến hàm lấy dữ liệu lên bảng
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
        table = table + '<tr class="'+cls+'">';
        table = table + '<td> ' + response.items[i].maTour.trim() + '</td>';
        table = table + '<td>  <a style="font-weight:bold; text-align:center; color:black; text-decoration:none" href="/home/chitiettour?matour=' + response.items[i].maTour.trim()+ '">' + response.items[i].tenTour.trim() + '</a></td>';         
        table = table + `<td class="py-1">
                    <img src="../../img/anhdaidien/${!!response.items[i].anh ? response.items[i].anh.trim() : 'default-avatar.png'}" alt="image" />
                </td>`;
        table = table + '<td>' + response.items[i].xepHangTour + '</td>';
        table = table + '<td>' + response.items[i].giaCho + '</td>';
        table = table + '<td>' + response.items[i].soNgayDl + '</td>';
        table = table + '<td>' + response.items[i].diemXuatPhat + '</td>';
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-info btn-rounded btn-icon" onclick="updateTourFill(\'' + response.items[i].maTour.trim() + '\')">Edit</i></button> ' + '</td>';
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteTour(\'' + response.items[i].maTour.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-Tour').innerHTML = table;
}