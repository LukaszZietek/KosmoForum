var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/category/GetAllCategories",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `<img src="data:image/jpg;base64,${data}" style='width: 50px; height:50px;' />`;
                    
                },
                "width": "10%"
            },
            {
                "data": "title",
                "width": "40%"
            },
            {
                "data": "description",
                "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                    <a href="/category/Upsert/${data}"
                    class = 'btn btn-success text-white'
                    style = 'cursor:pointer;'> <i class='far fa-edit'></i></a>
                    &nbsp;
                        <a onclick=Delete("/category/Delete/${data}")
                        class = 'btn btn-danger text-white'
                    style = 'cursor:pointer;'> <i class = 'far fa-trash-alt'></i></a>
                    </div>
                `;
                },
                "width": "30%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Jesteś pewny że chcesz wykonać tą operacje?",
        text: "Nie będzie możliwe odnowienie danych",
        icon: "warning",
        confirmButtonText: "Tak",
        showDenyButton: true,
        denyButtonText: "Nie",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

