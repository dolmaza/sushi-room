var CategoryEditorModel = (function() {
    function init() {
        initBootstrapSwitch();
        
        $('#category-image').change(function (){
            var file = $(this)[0].files[0];
            if (file){
                $('#ImageName').val(file.name);
                globals.readFileAsBase64(this).then(function (base64){
                    if (base64){
                        $('#ImageBase64').val(base64.split(',')[1]);
                        $('#category-image-preview').removeClass('hidden').attr('src', base64);
                    }
                });
            }
        });
    }

    function initBootstrapSwitch() {
        $(".checkbox-bootstrap-switch").bootstrapSwitch();
    }

    return {
        init: init
    }
})()