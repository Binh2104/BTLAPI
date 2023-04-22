$(document).ready(function () {
    getAllKST();
});

function getAllKST() {
    $.ajax({
        url: `https://localhost:7269/api/apiKST`,
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
                url: `https://localhost:7269/api/apiKST/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
$("#form-KST").submit(function (e) {
    e.preventDefault();
})
function renderPagination(totalPages, currentPage) {
    let pagination = '';
    for (let i = 1; i <= totalPages; i++) {
        pagination += `<button class="btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'}" onclick="setPage(${i})">${i}</button> `;
    }
    document.getElementById('pagination_KST').innerHTML = pagination;
}

function setPage(pageNumber) {
    const pageSize = 5;
    document.getElementById('page-number').innerHTML = pageNumber;
    $.ajax({
        url: `https://localhost:7269/api/apiKST/getPagination?pageSize=${pageSize}&pagenumber=${pageNumber}`,
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
function InsertKST() {
    var maKs = $("#MaKS").val();
    var matour = $("#MaTour").val();
    var vta = $("#ViTriAnh").val();

    var formData = new FormData();

    formData.append("maKs", maKs);
    formData.append("maTour", matour);
    formData.append("viTriAnh", vta);

    var url = 'https://localhost:7269/api/APIKST/themKST';
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
            getAllKST(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}

function deleteKST(id1, id2) {
    var url = 'https://localhost:7269/api/ApiKST?xoa?matour='+id1+'&maks='+id2;
    $.ajax({
        url: url,
        method: 'DELETE',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            /* alert("Xóa không thành công");*/
            getAllKST();
        },
        success: function (response) {
            alert("Xóa thành công");
            getAllKST(); //Gọi đến hàm lấy dữ liệu lên bảng
        }
    });
}
function resetInput() {
    $("#MaTour").val("").change()
    $("#MaKS").val("").change()
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
        table = table + '<td>' + response.items[i].tenKs.trim() + '</td>';    
        /*table = table + `<td>${ !!response.items[i].viTriAnh.trim() ? response.items[i].viTriAnh.trim() : 'NOT!' }</td>`;*/

        table = table + '<td>' + ' <button type="button" class="btn btn-gradient-danger btn-rounded btn-icon" onclick="deleteKST(\'' + response.items[i].maTour.trim() + '\',' + '\'' + response.items[i].maKs.trim() + '\')">Delete</button> ' + '</td>';
    }
    document.getElementById('tbody-KST').innerHTML = table;
}