$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/pages/GetAll' },
        scrollX: true,
        language: {
            "decimal": "",
            "emptyTable": "داده ای موجود نیست",
            "info": "نمایش _START_ تا _END_ از _TOTAL_ مورد",
            "infoEmpty": "نمایش 0 تا 0 از 0 لیست",
            "infoFiltered": "(فیلتر از _MAX_ کل )",
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
                data: 'objPage.pageId', "render": function (data) {
                    return `<p style="text-align:center;">${data}</p>
                    <div class="w-75 btn-group" role="group">
                           <a href="/admin/pages/edit?id=${data}" class="btn btn-primary mx-2">
                           <i class="bi bi-pencil-square"></i>
                           <a href="/admin/pages/delete?id=${data}" class="btn btn-danger mx-2">
                           <i class="bi bi-trash-fill"></i>

                           <a href="/Customer/Home/Property?pageId=${data}" class="btn btn-info mx-2">
                           <i class="bi bi-eye-fill"></i>
                    </div>`
                }, "width": "5%"
            },
            {
                data: 'objPage.imageUrl',
                "render": function (data) {
                    if (data) {
                        data = `<img src="${data}" width="100" height="100" style="object-fit: cover;object-position: 50% 50%;" />`
                    }
                    else {
                        data = `<img src="/images/logo.jpg" width="100" height="100" style="object-fit: cover;object-position: 50% 50%;" />`

                    }
                    return data



                }
            , "width": "1%" },
            { data: 'objPage.title', "width": "1%" },
            { data: 'objPage.price', "width": "1%" },
            { data: 'objPage.dimensions', "width": "1%" },
            { data: 'objPage.thickness', "width": "1%" },
            { data: 'objPage.mass', "width": "1%" },
            { data: 'objPage.colorCode', "width": "1%" },
            {
                data: 'objPage.customerNumber', "render": function (data) {
                    if (data == "") {
                        data = `<i class="bi bi-telephone"></i>`;
                    } else {
                        data = `<i class="bi bi-telephone-fill"></i> ${data}`;
                    }
                    return data
                }, "width": "5%" },
            {
                data: 'objPage.isActive', "render": function (data) {
                    if (data) {
                        data = `<i class="bi bi-eye"></i> فعال`;
                    } else {
                        data = `<i class="bi bi-eye-slash"></i> غیرفعال`;
                    }
                    return data
                }, "width": "5%"
            },
            {
                data: 'objPage.isFeatured', "render": function (data) {
                    if (data) {
                        data = `<i class="bi bi-star-fill"></i> ویژه`;
                    } else {
                        data = `<i class="bi bi-star"></i> عادی`;
                    }
                    return data
                }, "width": "5%"
            },
            { data: 'objPage.date', "width": "10%"}



        ]
    });
}
