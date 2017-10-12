(function ($) {
    //Custom select
    $.validator.addMethod("data-val-select", function (value, element, param) { if (value == 0) return false; else return true; }, function (param, element) { return param; });
    $.validator.unobtrusive.adapters.add("data-val-select");

    //Custom checkbox
    $.validator.addMethod("data-val-checkbox", function (value, element, param) {
        var a = $(element).attr('name');
        var b = $('form').find('[data-val-checkbox]').filter('[name="' + a + '"]');
        var rs = false;
        b.each(function (i, e) { if ($(e).prop('checked') == true) { rs = true; } })
        return rs;
    }, function (param, element) { return param; });
    $.validator.unobtrusive.adapters.add("data-val-checkbox");

    //parseDynamicContent
    $.validator.unobtrusive.parseDynamicContent = function (selector) {
        //use the normal unobstrusive.parse method
        $.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = $(selector).first().closest('form');

        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        var validator = form.validate();

        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {
            if (validator.settings.rules[elname] == undefined) {
                var args = {};
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //edit:use quoted strings for the name selector
                $("[name='" + elname + "']").rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {
                    if (validator.settings.rules[elname][rulename] == undefined) {
                        var args = {};
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        //edit:use quoted strings for the name selector
                        $("[name='" + elname + "']").rules("add", args);
                    }
                });
            }
        });
    }
})($);
//Add Bootstrap CSS
$(function () {
    $('[data-val="true"]').on('keyup blur', function () {
        var $this = $(this);
        setTimeout(function () {
            if ($this.hasClass('input-validation-error')) {
                $this.removeClass('form-control-success').addClass('form-control-danger');
                $this.parents('.form-group').removeClass('has-success').addClass('has-danger');
            }
            else if ($this.hasClass('valid')) {
                $this.removeClass('form-control-danger').addClass('form-control-success');
                $this.parents('.form-group').removeClass('has-danger').addClass('has-success');
            }
        }, 1)
    })
});
//jQuery.validator.unobtrusive.parseElement($('#input')[0], false);
//$.validator.unobtrusive.parseDynamicContent($('#input')[0]);