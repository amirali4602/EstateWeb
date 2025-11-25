$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/agents/GetAll' },
        scrollX: true,
        language: {
            "decimal": "",
            "emptyTable": "داده ای موجود نیست",
            "info": "نمایش _START_ تا _END_ از _TOTAL_ مورد",
            "infoEmpty": "نمایش 0 تا 0 از 0 لیست",
            "infoFiltered": "(فیلتر از _MAX_ کل)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "تعداد _MENU_ درحال نمایش",
            "loadingRecords": "در حال بارگزاری ...",
            "processing": "",
            "search": "جستجو: ",
            "zeroRecords": "نتیجه ای یافت نشد",
            "paginate": {
                "first": "اولین",
                "last": "اخرین",
                "next": "بعد",
                "previous": "قبل"
            },
            "aria": {
                "orderable": "نمایش برا اساس این ستون",
                "orderableReverse": "نمایش معکوس بر اساس این ستون"
            }
        },
        "columns": [
            {
                data: 'id', "render": function (data) {
                    return `<p style="text-align:center;">${data}</p>
                    <div class="w-75 btn-group" role="group">
                           <a href="/admin/agents/edit?id=${data}" class="btn btn-primary mx-2">
                           <i class="bi bi-pencil-square"></i>
                           <a href="/admin/agents/delete?id=${data}" class="btn btn-danger mx-2">
                           <i class="bi bi-trash-fill"></i>
                    </div>`
                }, "width": "5%"
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
            { data: 'name', "width": "1%" },
            { data: 'number', "width": "1%" },
            { data: 'description', "width": "15%" }

        ]
    });
}
