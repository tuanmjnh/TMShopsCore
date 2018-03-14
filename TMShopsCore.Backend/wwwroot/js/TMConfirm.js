﻿+function ($) {
    "use strict";
    var DF = {
        action: 'click',
        type: 'df',//lg,sm
        txtTitle: 'Yêu cầu xác nhận!',
        txtContent:'Bạn có chắc chắn muốn thực hiện?',
        labelCancel: 'Hủy bỏ',
        labelOk: 'Xác nhận',
        fade: false,
        backdrop: true,
        keyboard: true,
        resetForm: false,
        modalShow: function () { },
        modalHidden: function () { },
        modalOk: function () { },
        modalCancel: function () { }
    }
    $.fn.TMConfirm = function (op) {
        var $this = $(this);
        $this.off('click').on('click', function (e) {
            $.extend(DF, op);
            //
            var html = '<div class="modal ' + (DF.fade ? 'fade' : '') + '" id="TMConfirm" tabindex="-1" role="dialog" data-backdrop="' + (DF.backdrop ? 'true' : 'false') + '" data-keyboard="' + (DF.keyboard ? 'true' : 'false') + '">\
                        <div class="modal-dialog modal-'+ DF.type + '" role="document">\
                            <div class="modal-content">';
            html += '<div class="modal-header">\
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>\
                    <h4 class="modal-title">'+ DF.txtTitle + '</h4>\
                </div>';
            html += '<div class="modal-body">' + DF.txtContent + '</div>';
            html += '<div class="modal-footer">\
                    <button type="button" class="btn btn-primary" data-dismiss="modal">'+ DF.labelOk + '</button>\
                    <button type="button" class="btn btn-default" data-dismiss="modal">'+ DF.labelCancel + '</button>\
                </div>';
            html += '</div></div></div>';
            //
            $('body').append(html);
            var modal = $(document).find('#TMConfirm');
            //
            $(modal).modal('show');
            //
            $(modal).find('.btn.btn-primary').on('click', function () {
                if (typeof DF.modalOk == 'function') DF.modalOk();
            });
            //
            $(modal).find('.btn.btn-default').on('click', function () {
                if (typeof DF.modalCancel == 'function') DF.modalCancel();
            });
            //
            $(modal).on('show.bs.modal', function (e) {
                if (typeof DF.modalShow == 'function') DF.modalShow();
            });
            //
            $(modal).on('hidden.bs.modal', function (e) {
                modal.remove();
                if (typeof DF.modalHidden == 'function') DF.modalHidden();
            });
            if (DF.resetForm) $this.trigger('reset');
        })
    };
}(jQuery);