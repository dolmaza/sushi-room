var globals = (function () {
    var handleBootstrapSwitch = function () {
        if (!$().bootstrapSwitch) {
            return;
        }
        $('.make-switch').bootstrapSwitch();
    };

    var initNumericInput = function () {
        $('.apply-numeric').numericInput();
        $('.apply-numeric-allow-float').numericInput({ allowFloat: true });
    };

    var readFileAsBase64 = function ($this) {
        return new Promise(function (resolve, reject) {
            if ($this.files && $this.files[0]) {
                var fileReader = new FileReader();
                fileReader.onload = function (e) {
                    if (resolve) {
                        resolve(e.target.result);
                    }
                };

                fileReader.onerror = function (errorText) {
                    reject(errorText);
                };

                fileReader.readAsDataURL($this.files[0]);
            }
        });
    }

    return {
        texts: {
            abort: 'უკაცრავად დაფიქსირდა შეცდომა',
            success: 'ოპერაცია დასრულდა წარმატებით',
            pleaseSelectRolesAndPermissions: 'გთხოვთ მონიშნოთ როლი და უფლებები',
            pleaseSelectACar: 'გთხოვთ აირჩიოთ მანქანა და სცადოთ თავიდან'
        },

        handleBootstrapSwitch: handleBootstrapSwitch,
        initNumericInput: initNumericInput,
        readFileAsBase64: readFileAsBase64,
        showAjaxLoader: true
    };
})();

$(function () {
    globals.handleBootstrapSwitch();
    globals.initNumericInput();


    $(document).ajaxStart(function () {
        if (globals.showAjaxLoader) {
            App.blockUI({ animate: true });
        }
    });

    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        var item = this;
        //TO handle ajax abort error
        if (jqxhr.status === 0 || jqxhr.readyState === 0)
            return;

        //Unauthorized request
        if (jqxhr.status === 401) {
            window.location = "/login";
            return;
        }
    });

    $(document).ajaxComplete(function (event, jqxhr, settings, thrownError) {

        var item = jqxhr.getResponseHeader("X-Responded-JSON");

        if (!$.isEmptyObject(item)) {
            if (JSON.parse(item).status === 401) {
                window.location = "/login";
                return;
            }
        }

        App.unblockUI();
        globals.showAjaxLoader = true;
    });

});