﻿//GetForm
+function ($) {
    "use strict";
    var DF = {
        url: '',
        data: null,
        resetForm: false,
        TinyMCE: true,
        done: function () { },
        fail: function () { },
        always: function () { }
    }
    $.fn.GetForm = function (op) {
        var $this = $(this);
        $this.off('click').on('click', function (e) {
            $.extend(DF, op);
            getForm({ element: e, url: DF.url, data: DF.data, form: formEdit, modal: ModalEdit, TinyMCE: DF.TinyMCE, done: DF.done, fail: DF.fail, always: DF.always });
            if (DF.resetForm) $this.trigger('reset');
        })
    };
}(jQuery);
function getForm(obj) {
    if (obj.element !== undefined) obj.element.preventDefault();
    $.get(obj.url, obj.data, function (d) {
        $(document).find(obj.form).append(d).find(obj.modal).modal('show');
        for (var i in obj.data)
            $(obj.modal).append('<input type="hidden" name="' + i + '" value="' + obj.data[i] + '" />');
    }).done(function (d) {
        if (typeof obj.done == 'function') obj.done();
        obj.TinyMCE ? getTinyMCE() : null;
    }).fail(function (d) {
        if (typeof obj.fail == 'function') obj.fail();
    }).always(function (d) {
        if (typeof obj.always == 'function') obj.always();
    })
}

//FormPost
+function ($) {
    "use strict";
    //contentType
    //1. 'application/x-www-form-urlencoded; charset=utf-8' / FormData
    //2. 'application/x-www-form-urlencoded; charset=utf-8' / serialize
    //3. 'application/json; charset=utf-8' / JSON.stringify()
    var DF = {
        type: 'POST',
        contentType: 1,
        url: '',
        id: null,
        arrayData: null,
        data: null,
        resetForm: true,
        done: function () { },
        fail: function () { },
        always: function () { }
    }
    $.fn.FormPost = function (op) {
        var $this = $(this);
        $this.attr('action', DF.url);
        $this.off('submit').on('submit', function (e) {
            $.extend(DF, op);
            e.preventDefault();
            var contentType = false;
            var dataForm = new FormData(this);
            var processData = false;
            if (DF.arrayData !== null) dataForm.appendArray(DF.arrayData);
            if (DF.contentType === 2) {
                contentType = 'application/x-www-form-urlencoded; charset=utf-8';
                dataForm = $this.serialize();
                processData = true;
            } else if (DF.contentType === 3) {
                contentType = 'application/json; charset=utf-8';
                dataForm = JSON.stringify(DF.data);
                processData = true;
            }
            if ($this.valid())
                $.ajax({
                    type: DF.type,
                    dataType: 'json',
                    contentType: contentType,
                    processData: processData,
                    url: DF.id === null ? DF.url : DF.url + '/' + $(DF.id).val(),
                    data: DF.data === null ? dataForm : $.extend({}, DF.data, { __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val() }),
                    success: function (d) {
                        if (d.success) {
                            if (DF.resetForm) $this.trigger('reset');
                            $('.has-success').removeClass('has-success');
                            $('.form-control-success').removeClass('form-control-success');
                            $($TMAlert).TMAlert({ type: "success", message: TMLanguage(d.success) });
                        }
                        else if (d.danger)
                            $($TMAlert).TMAlert({ type: "danger", message: TMLanguage(d.danger) });
                    }
                }).done(function (d) {
                    if (typeof DF.done == 'function') DF.done();
                }).fail(function (d) {
                    if (typeof DF.fail == 'function') DF.fail();
                }).always(function (d) {
                    if (typeof DF.always == 'function') DF.always();
                })
            else
                console.log("Submit Error");
        })
    }
}(jQuery);

//FormGet
+function ($) {
    "use strict";
    var DF = {
        url: '',
        id: null,
        arrayData: null,
        data: null,
        resetForm: false,
        done: function () { },
        fail: function () { },
        always: function () { }
    }
    $.fn.FormGet = function (op) {
        var $this = $(this);
        $this.off('click').on('click', function (e) {
            $.extend(DF, op);
            formGet({ element: e, url: DF.url, data: DF.data });
        })
    };
}(jQuery);

function formGet(obj) {
    if (obj.element !== undefined) obj.element.preventDefault();
    $.get(obj.url, $.extend({}, obj.data, { __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val() }), function (d) {
        if (d.success)
            $(document).find(obj.form).TMLoadFormData(d.data.result ? { data: $.extend({}, d.data.result.obj, d.data.result) } : { data: d.data })
        else if (d.danger)
            $($TMAlert).TMAlert({ type: "danger", message: TMLanguage(d.danger) });
    }).done(function (d) {
        if (typeof obj.done == 'function') obj.done();
    }).fail(function (d) {
        if (typeof obj.fail == 'function') obj.fail();
    }).always(function (d) {
        if (typeof obj.always == 'function') obj.always();
    })
};
//TMLoadFormData
+function ($) {
    "use strict";
    var DF = {
        attr: 'name',
        attrs: 'tm-form',
        data: null,
        tinymce: false,
        file: '#fileInput',
        imgWidth: 200,
        imgHeight: 200
    };

    $.fn.TMLoadFormData = function (op) {
        $.extend(DF, op);
        return $(this).each(function () {
            if (DF.data == null) return false;
            DF.data = ConvertKeysToLowerCase(DF.data);
            var t = [];
            $(this).find('[' + DF.attr + ']').each(function () {
                var $this = $(this);
                var attrVal = $this.attr(DF.attr).toLowerCase();
                t.push({ tag: $this, type: this.type })
                if (DF.data.hasOwnProperty(attrVal)) {
                    if ($this.is('input:hidden')) {//this.type === 'hidden'
                        $this.attr('value', DF.data[attrVal])
                    }
                    else if ($this.is('input:radio')) {
                        if ($this.val() == DF.data[attrVal]) {
                            $this.attr('checked', 'checked');
                            if ($this.parent().parent().hasClass('btn-group')) $this.parent().addClass('active');
                        }
                        else {
                            $this.removeAttr('checked');
                            if ($this.parent().parent().hasClass('btn-group')) $this.parent().removeClass('active');
                        }
                    }
                    else if ($this.is('input:checkbox')) {//$this.is('input:checkbox')
                        if ($this.val() == DF.data[attrVal]) {
                            $this.attr('checked', 'checked');
                            if ($this.parent().parent().hasClass('btn-group')) $this.parent().addClass('active');
                        }
                        else {
                            $this.removeAttr('checked');
                            if ($this.parent().parent().hasClass('btn-group')) $this.parent().removeClass('active');
                        }
                    }
                    else if ($this.is('textarea')) {
                        $this.html(DF.data[attrVal])
                        //if (DF.tinymce) {
                        //    tinyMCE.activeEditor.setContent($('<div/>').html(DF.data[attrVal]).text());
                        //}
                    }
                    else {
                        $this.attr('value', DF.data[attrVal])
                    }
                    //$this.removeAttr(DF.attr);
                }
                var reg = /\[\]/g;
                var regVal = reg.exec(attrVal);
                if (regVal !== null) {
                    attrVal = attrVal.replace(reg, '');
                    if (DF.data.hasOwnProperty(attrVal)) {
                        if ($this.is('input:checkbox')) {
                            if ($.inArray($this.val(), DF.data[attrVal].trim(',').split(',')) > -1) {
                                $this.attr('checked', 'checked');
                                if ($this.parent().parent().hasClass('btn-group')) $this.parent().addClass('active');
                            }
                        }
                    }
                }
            });
            $('[' + DF.attrs + ']').each(function () {
                var $this = $(this);
                var attrVal = $this.attr(DF.attrs).toLowerCase();
                if ($this.is('img')) {
                    if (DF.data.hasOwnProperty(attrVal) && DF.data[attrVal] != null) {
                        var imgUrl = Host + '/' + DF.data[attrVal].replace(/\\/g, '/');
                        //$this.attr('tm-img-url', imgUrl).append('<img src="' + imgUrl + '">');
                        $this.attr('src', imgUrl);
                        $('#fileInput').TMReadImage({ w: DF.imgWidth, h: DF.imgHeight, loadForm: true });
                    } else {
                        $('#fileInput').TMReadImage({ w: DF.imgWidth, h: DF.imgHeight });
                    }
                }
                else if ($this.is('label')) {
                    var reg = /\[\d\]/g;
                    var regVal = reg.exec(attrVal);
                    if (regVal !== null) {
                        var arrVal = DF.data[attrVal.replace(reg, '')].trim(',').split(',');
                        var i = regVal[0].replace(/\[|\]/g, '');
                        if (arrVal.length > i)
                            $this.html(arrVal[i])
                        else
                            $this.html('N/A')
                    } else if (DF.data.hasOwnProperty(attrVal) && DF.data[attrVal] != null) {
                        if ($this.is('[moment-fomat]'))
                            $this.html(moment(DF.data[attrVal]).format($this.attr('moment-fomat')))
                        else
                            $this.html(DF.data[attrVal])
                    } else
                        $this.html('N/A')
                }
            });
        });
    };
}(jQuery);