﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var inputOption = document.getElementById("OptionOfData").value;
    var urlLink;
    if (inputOption == "index") { //Opcja 1, zwraca wszystkie dostepne posty w serwisie
        urlLink = "/forumpost/GetAllForumPosts";
    }
    else if (inputOption == "category") { // Opcja2, zwraca wszystkie dostepne posty w danej kategorii
        urlLink = "/forumpost/GetAllForumPostsInCategory?categoryid=" + document.getElementById("forum").value;
    }
    else if (inputOption == "userposts") { // Opcja 3, zwraca wszystkie dostepne posty stworzone przez danego uzytkownika
        urlLink = "/user/GetMyForumPosts";
    } else {
        urlLink = "/forumpost/GetAllForumPosts";
    }

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": urlLink,
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
                "render": function (data, type, row, meta) {
                    var value = row['id'];
                    return `<h6><a href="/forumpost/readforumpost/${value}" class="text-primary align-middle">${data}</a></h6>`;
                },
                "width": "20%"
            },
            {
                "data": "date",
                "render": function (data) {
                    var showData = data.substring(8, 10) + "-" + data.substring(5, 7) + "-" + data.substring(0, 4);
                    return `${showData}`;
                },
                "width": "30%"
            },
            {
                "data": "userId",
                "render": function(data, type, row, meta) {
                    var value = CutUserName(row['user']['username'],10);
                    return `${value}`;
                },
                "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    var booleanValue = IfBelongToUser(data);
                    if (booleanValue)
                    {

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
                    }
                    return ``;
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

function IfBelongToUser(id) {
    var urlLink = "/forumpost/ifauthorized/" + id;
    var status = false;

    $.ajax({
        url: urlLink,
        async: false,
        success: function(data) {
            status = data;
        }
    });

    return status;
}

function CutUserName(userName, expectedLength) {
    if (userName.length > expectedLength) {
        return userName.substring(0, expectedLength) + "...";
    }
    return userName;
}

