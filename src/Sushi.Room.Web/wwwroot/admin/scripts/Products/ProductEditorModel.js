var ProductEditorModel = (function() {
    function init() {
        initBootstrapSwitch();
        initNumericInput();
        initTinymce('#Description');
        initTinymce('#DescriptionEng');
        $('#product-image').change(function (){
            var file = $(this)[0].files[0];
            if (file){
                $('#ImageName').val(file.name);
                globals.readFileAsBase64(this).then(function (base64){
                    if (base64){
                        $('#ImageBase64').val(base64.split(',')[1]);
                        $('#product-image-preview').removeClass('hidden').attr('src', base64);
                    }
                });
            }
        });
    }

    function initBootstrapSwitch() {
        $(".checkbox-bootstrap-switch").bootstrapSwitch();
    }
    
    function initNumericInput() {
        $('#Price, #DiscountPercent').numericInput({ allowFloat: true });
    }
    
    function initTinymce(selector) {
        tinymce.init({
            selector: selector,
            height: 250,
            menubar: false,
            plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime media table paste code help wordcount'
            ],
            toolbar: 'undo redo | formatselect | ' +
                'bold italic backcolor | alignleft aligncenter ' +
                'alignright alignjustify | bullist numlist outdent indent | ' +
                'removeformat | help',
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
        });
    }

    return {
        init: init
    }
})();