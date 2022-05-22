function changeLabel() {
    var elem = document.getElementById('file-label')
    elem.style.backgroundColor = 'green'
    elem.textContent = 'Change'
}

function rowClicked(i) {
    // Funcion para detectar el indice de la tabla que se presiono
    console.log(i)

    $.ajax({
        type: 'GET',
        url: '/Account/TaskDetails',
        data: { index: i },
        success: function (data) {
            $('body').html(data)
        }
    });
}

function modalLoad(i) {
    $.ajax({
        url: '/Account/TaskDetails',
        data: { index: i },
        success: function (data) {
            $("#showmodal .modal-dialog").html(data);
            $("#showmodal").modal("show");
        }
    });
}