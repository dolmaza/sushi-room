var CategoryEditorModel = (function() {
    function init() {
        initBootstrapSwitch();
    }

    function initBootstrapSwitch() {
        $(".checkbox-bootstrap-switch").bootstrapSwitch();
    }

    return {
        init: init
    }
})()