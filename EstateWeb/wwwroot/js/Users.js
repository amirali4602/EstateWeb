$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/home/getusers' },
        scrollX: true,
        "columns": [
            {
                data: 'userName', "render": function (data) {
                    return `
                    <div class="btn-group" role="group">
                           <a href="/admin/home/edit?userName=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div>`
                }, "width": "1%"
            },
            {
                data: 'profilePic',
                "render": function (data) {
                    if (data == undefined) {
                        data = "/images/logo.jpg";
                    }
                    return `<img src="${data}" width="100" height="100" style="object-fit: cover;object-position: 50% 50%;" />`
                }
                , "width": "1%"
            },
            {
                data: 'role',
                "render": function (data) {
                    if (data == undefined) {
                        data = "مشتری";
                    }
                    return `${data}`
                }, "width": "1%" },
            { data: 'userName', "width": "1%" },
            {
                data: 'name',
                "render": function (data) {
                    if (data == undefined || data == "0") {
                        data = "_";
                    }
                    return `${data}`
                }
                        , "width": "1%"
                },

        ]
    });
}
