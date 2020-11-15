var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var urlLink = "opinions/getAllOpinionForUser";

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": urlLink,
            "type": "GET",
            "datatype": "json"
        },
        "language": {
            "emptyTable": "Brak dostępnych opinii",
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
                "data": "content",
                "render": function (data, type, row, meta) {
                    var value = row['forumPostId'];
                    return `<h6><a href="/forumpost/readforumpost/${value}" class="text-primary">${data.substring(0,35) + " ..."}</a></h6>`;
                },
                "width": "35%"
            },
            {
                "data": "creationDateTime",
                "render": function (data) {
                    var showData = data.substring(8, 10) + "-" + data.substring(5, 7) + "-" + data.substring(0, 4);
                    return `${showData}`;
                },
                "width": "40%"
            },
            {
                "data": "marks",
                "render": function (data) {
                    var value = parseInt(data);
                    var backData = '';
                    for (i = value; i < 5; i++) {
                        backData +=
                            '<i class="fa fa-star fa-1x" id="star0" style="color: pink" aria-hidden="true"></i>';
                    }
                    for (i = 0; i < value; i++) {
                        backData +=
                            '<i class="fa fa-star-o fa-1x" id="star0" style="color: pink" aria-hidden="true"></i>';
                    }

                    return `${backData}`;
                },
                "width": "20%"
            },
            {
                "data": "id",
                "render": function(data) {
                    return `<input type="hidden" value="${data}" />`;
                },
                "width": "5%"
            }
        ]
    });
}


