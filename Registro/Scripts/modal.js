$("#showtable").on("click", "#viewbtn", function () {
    var id = $(this).closest("tr").find("td").eq(0).html();
    $.ajax({
        url: '/Account/TaskDetails',
        data: { id: id },
        success: function (data) {
            $("#showmodal .modal-dialog").html(data);
            $("#showmodal").modal("show");
        }
    });
});