var toastrNotification = {
    notificationType: null,
    message: null,

    types: {
        success: 'success',
        error: 'warning',
        warning: 'error',
        info: 'info'
    },

    options: {
        closeButton: false,
        debug: false,
        newestOnTop: false,
        progressBar: false,
        positionClass: 'toast-top-right',
        preventDuplicates: false,
        onclick: null,
        showDuration: '300',
        hideDuration: '1000',
        timeOut: '5000',
        extendedTimeOut: '1000',
        showEasing: 'swing',
        hideEasing: 'linear',
        showMethod: 'fadeIn',
        hideMethod: 'fadeOut'
    },

    positionClasses: {
        topRight: 'toast-top-right',
        bottomRight: 'toast-bottom-right',
        bottomLeft: 'toast-bottom-left',
        topLeft: 'toast-top-left',
        topFullWidth: 'toast-top-full-width',
        bottomFullWidth: 'toast-bottom-full-width',
        topCenter: 'toast-top-center',
        bottomCenter: 'toast-bottom-center'
    },

    init: function (message, options) {
        toastrNotification.notificationType = toastrNotification.types.success;
        toastrNotification.message = message ? message : null;

        if (options) {
            options = typeof (options) == 'object' ? options : JSON.stringify(options);

            toastrNotification.notificationType = options.notificationType ? options.notificationType : toastrNotification.types.success;

            toastrNotification.options.closeButton = options.closeButton == undefined || options.closeButton == null ? toastrNotification.options.closeButton : options.closeButton;
            toastrNotification.options.closeButton = options.debug == undefined || options.debug == null ? toastrNotification.options.debug : options.closeButton;
            toastrNotification.options.newestOnTop = options.newestOnTop == undefined || options.newestOnTop == null ? toastrNotification.options.newestOnTop : options.newestOnTop;
            toastrNotification.options.progressBar = options.progressBar == undefined || options.progressBar == null ? toastrNotification.options.progressBar : options.progressBar;
            toastrNotification.options.positionClass = options.positionClass ? options.positionClass : toastrNotification.options.positionClass;
            toastrNotification.options.preventDuplicates = options.preventDuplicates == undefined || options.preventDuplicates == null ? toastrNotification.options.preventDuplicates : options.preventDuplicates;
            toastrNotification.options.onclick = options.onclick ? options.onclick : toastrNotification.options.onclick;
            toastrNotification.options.showDuration = options.showDuration ? options.showDuration : toastrNotification.options.showDuration;
            toastrNotification.options.hideDuration = options.hideDuration ? options.hideDuration : toastrNotification.options.hideDuration;
            toastrNotification.options.timeOut = options.timeOut ? options.timeOut : toastrNotification.options.timeOut;
            toastrNotification.options.extendedTimeOut = options.extendedTimeOut ? options.extendedTimeOut : toastrNotification.options.extendedTimeOut;
            toastrNotification.options.showEasing = options.showEasing ? options.showEasing : toastrNotification.options.showEasing;
            toastrNotification.options.hideEasing = options.hideEasing ? options.hideEasing : toastrNotification.options.hideEasing;
            toastrNotification.options.showMethod = options.showMethod ? options.showMethod : toastrNotification.options.showMethod;
            toastrNotification.options.hideMethod = options.hideMethod ? options.hideMethod : toastrNotification.options.hideMethod;
            
        }

        return toastrNotification;
    },

    showMessage: function () {

        toastr.options = toastrNotification.options;
        switch (toastrNotification.notificationType) {
            case toastrNotification.types.success:
                toastr.success(toastrNotification.message);
                break;

            case toastrNotification.types.error:
                toastr.error(toastrNotification.message);
                break;

            case toastrNotification.types.warning:
                toastr.warning(toastrNotification.message);
                break;

            case toastrNotification.types.info:
                toastr.info(toastrNotification.message);
                break;

            default:
        }
    }
}