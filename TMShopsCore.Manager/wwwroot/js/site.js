//Declares
var $TMAlert = '#TMAlert',
    $isTooltip = '.isTooltip',
    $tinymce = '.tinymce',
    ModalCreate = '#ModalCreate',
    ModalEdit = '#ModalEdit',
    formCreate = '#FormCreate',
    formEdit = '#FormEdit',
    DateNow = new Date();
var $area = Host + '/',
    $areaSegment = $area + Segment[0] + '/';

$(function () {
    TMLanguageTitle('TMShopsCore');
    $($isTooltip).tooltip({ animation: false });
});
function removeBackdrop(edit) {
    $(edit).on('hidden.bs.modal', function (e) {
        $('.modal-backdrop').remove();
        $(edit).remove();
    })
};

//Global Function
$('.profile').GetForm({ url: 'Users/PartialProfile' });
$('.changePassword').GetForm({ url: 'Users/PartialChangePassword' });
$('.userSetting').GetForm({ url: 'Users/PartialUserSetting' });
//$('.profile').off('click').on('click', function (e) {
//    getEdit(e, $area + 'Users/PartialProfile', $area + 'api/UsersAPI/Profile', null)
//})
//$('.changePassword').off('click').on('click', function (e) {
//    getEdit(e, $area + 'Users/PartialChangePassword')
//})
//$('.userSetting').off('click').on('click', function (e) {
//    getEdit(e, $area + 'Users/PartialUserSetting')
//})

//TinyMCE
function getTinyMCE() {
    tinymce.init({
        selector: $tinymce,
        mode: 'specific_textareas',//'textareas'
        //theme: 'advanced',
        //force_br_newlines: false,
        //force_p_newlines: false,
        //forced_root_block: '',
        encoding: 'xml',
        entity_encoding: "raw",
        //convert_urls: false,
        theme: 'modern',
        //width: 500,
        //height: 300,
        plugins: [
            'advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker',
            'searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking',
            'save table contextmenu directionality emoticons template paste textcolor'
        ],
        //content_css: 'css/content.css',
        toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons',
        style_formats: [
            { title: 'Bold text', inline: 'b' },
            { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
            { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
            { title: 'Example 1', inline: 'span', classes: 'example1' },
            { title: 'Example 2', inline: 'span', classes: 'example2' },
            { title: 'Table styles' },
            { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        ],
        setup: function (editor) {
            editor.on('change', function () {
                tinymce.triggerSave();
            });
        }
    });
}
//
//$.fn.TMCheckBox('.chkall', '.chkitem', '.btn-chk');