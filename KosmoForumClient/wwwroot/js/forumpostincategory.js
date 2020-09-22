var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var urllink = "/forumpost/GetAllForumPostsInCategory?categoryid=" + document.getElementById("forum").value;
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": urllink,
            "type": "GET",
            "datatype": "json"
        },
        "language": {
            "emptyTable": "Brak dostępnych postów",
            "lengthMenu": "",
            "paginate": {
                "first": "First",
                "last": "Last",
                "next": "Następna",
                "previous": "Poprzednia"
            },
            "search": "Szukaj",
            "info": ""
        },
        "ordering": false,
        "columns": [
            {
                "data": "title",
                "width": "10%"
            },
            {
                "data": "date",
                "width": "40%"
            },
            {
                "data": "userId",
                "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                    <a href="/forumpost/Upsert/${data}"
                    class = 'btn btn-success text-white'
                    style = 'cursor:pointer;'> <i class='far fa-edit'></i></a>
                    &nbsp;
                        <a onclick=Delete("/forumpost/Delete/${data}")
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
    swal({
        title: "Jesteś pewny, że chcesz wykonać tą operacje?",
        text: "Nie będzie możliwe odnowienie danych",
        icon: "warning",
        buttons: true,
        buttons: ["Nie", "Tak"],
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
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

