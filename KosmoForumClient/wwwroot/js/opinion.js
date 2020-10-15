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
                    return `<a href="/forumpost/readforumpost/${value}">${data.substring(0,35) + " ..."}</a>`;
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


