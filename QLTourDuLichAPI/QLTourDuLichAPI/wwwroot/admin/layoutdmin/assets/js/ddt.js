$(document).ready(function () {
    getAllDDT();
});

function getAllDDT() {
    $.ajax({
        url: `https://localhost:7269/api/apiDDT`,
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
                url: `https://localhost:7269/api/apiDDT/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
$("#form-DDT").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_DDT').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/apiDDT/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
function Insert() {
    var maDd = $("#MaDTQ").val();
    var matour = $("#MaTour").val();
    var vta = $("#ViTriAnh").val();

    var formData = new FormData();

    formData.append("maDd", maDd);
    formData.append("maTour", matour);
    formData.append("viTriAnh", vta);

    var url = 'https://localhost:7269/api/APIDDT/themDDT';
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
            getAllDDT(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

/*function UpdateDDT() {
    var maDd = $("#MaDTQ").val();
    var matour = $("#MaTour").val();
    var vta = $("#ViTriAnh").val();

    var formData = new FormData();

    formData.append("maDd", maDd);
    formData.append("maTour", matour);
    formData.append("viTriAnh", vta);

    var url = 'https://localhost:7269/api/APIDDT/capnhatDDT';
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
            getAllDDT(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}*/

/*function updateDDTFill(id1, id2) {
    var url = 'https://localhost:7269/api/ApiDDT/getById?id1=' + id1+'&id2='+id2;
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
            $("#MaDTQ").val(response.maDd.trim()).change()
            $("#ViTriAnh").val(response.viTriAnh.trim()).change()
        }
    });
}*/
function deleteDDT(id1, id2) {
    var url = 'https://localhost:7269/api/ApiDDT/xoa?matour='+id1+'&madd='+id2;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
             /*alert("Xóa không thành công");*/
            getAllDDT();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllDDT(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}
function resetInput() {
    $("#MaTour").val("").change()
    $("#MaDTQ").val("").change()
    $("#ViTriAnh").val("").change()
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
        table = table + '<td> <a style="font-weight:bold; text-align:center; color:black; text-decoration:none" href="/home/chitiettour?matour=' + response.items[i].maTour.trim() + '">' + response.items[i].tenTour.trim() + '</td>';
        table = table + '<td>' + response.items[i].tendiadiem.trim() + '</td>';    
        table = table + `<td>${!!response.items[i].viTriAnh.trim() ? response.items[i].viTriAnh.trim() : 'NOT!'}</td>`;
      /*  table = table + '<td>' + ' <button type="button" class="btn btn-gradient-info btn-rounded btn-icon" onclick="updateDDTFill(\'' + response.items[i].maTour.trim() + '\',' + '\'' + response.items[i].maDd.trim() + '\')">Edit</button> ' + '</td>';*/
        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteDDT(\'' + response.items[i].maTour.trim() + '\',' +'\'' + response.items[i].maDd.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-DDT').innerHTML = table;
}