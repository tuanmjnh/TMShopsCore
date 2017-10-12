removeBackdrop(ModalCreate);
$(formCreate).off('submit').on('submit', function (e) {
    e.preventDefault();
    ////form encoded data
    var contentType = 'application/x-www-form-urlencoded; charset=utf-8';
    var data = new FormData(this);//$(this).serialize();
    //var dfImg = $('.tm-read-img').children('img').attr('src');
    ////JSON data
    //var contentType = 'application/json; charset=utf-8';
    //var data = {
    //    FirstName: 'Andrew',
    //    LastName: 'Lock',
    //    Age: 31
    //}
    if ($(this).valid())
        $.ajax({
            type: 'POST',
            dataType: 'json',
            contentType: false,//contentType,
            processData: false,
            url: $url,//Host + '/cms/api/UsersAPI',
            data: data,
            success: function (d) {
                if (d.success) {
                    $(formCreate).trigger('reset');
                    $('.has-success').removeClass('has-success');
                    $('.form-control-success').removeClass('form-control-success');
                    tinyMCE.activeEditor.setContent('');
                    $($TMAlert).TMAlert({ type: "success", message: TMLanguage(d.success) });
                    $($table).bootstrapTable('refresh');
                    //$('.tm-read-img').children('img').attr('src', dfImg);
                    //$('#TMAlertModal').TMAlert({ type: "success", message: TMLanguage(d.success) });
                }
                else if (d.danger)
                    $($TMAlert).TMAlert({ type: "danger", message: TMLanguage(d.danger) });
                //$('#TMAlertModal').TMAlert({ type: "danger", message: TMLanguage(d.danger) });
            }
        })
    else
        console.log("Submit Error");
})