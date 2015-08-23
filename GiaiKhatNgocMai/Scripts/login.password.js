var loginJs = {
    init: function () {
        $('#reset_password_button').unbind('click').click(function () {
            $('#login_form').fadeOut(300, function () {
                $('#reset_password_form').fadeIn(300);
            });
            return false;
        });

        $('#login_button').unbind('click').click(function () {
            $('#reset_password_form').fadeOut(300, function () {
                $('#login_form').fadeIn(300);
            });
            return false;
        });
        $('#customerEmail').unbind('change').change(function () {
            if (loginJs.validateEmail($(this).val())) {
                if ($(this).hasClass('input_validation_error')) $(this).removeClass('input_validation_error');
                $('#error_list').parent().hide();
                return false;
            }
            $(this).addClass('input_validation_error');
            return false;
        });
        $('#submit_reset').unbind('click').click(function () {
            var email = $('#customerEmail').val();
            var errEl = $('#error_list');
            var resultEl = $('#reset_message');
            var processEl = $('p.processing');
            var toHideEls = $('#reset_password_form fieldset p:not(.processing)');

            errEl.html('');
            if (email.replace(/\s+/gi) == '') {
                $('#customerEmail').addClass('input_validation_error').attr('placeholder', 'Nhập địa chỉ email');
                return false;
            } else if(loginJs.validateEmail(email) === false){
                $('#customerEmail').addClass('input_validation_error');
                errEl.append('<li>Địa chỉ email không hợp lệ</li>');
                errEl.parent('div.error').show();
                return false;
            }
            errEl.parent('div.error').hide();
            toHideEls.hide();
            processEl.show();
            $.ajax({
                url: appBaseUrl + 'khach-hang/mat-khau',
                data: { email: email },
                type: "POST"
            }).success(function (data) {
                processEl.hide();
                if (data.isfound === true) {
                    $('#reset_message').fadeIn(500, function () {
                        if (data.issent) {
                            $(this).addClass('info_success');
                            if ($(this).hasClass('info_failure')) $(this).removeClass('info_failure');
                            if ($(this).hasClass('info_warning')) $(this).removeClass('info_warning');                            
                        } else {
                            $(this).addClass('info_warning');
                            if ($(this).hasClass('info_failure')) $(this).removeClass('info_failure');
                            if ($(this).hasClass('info_success')) $(this).removeClass('info_success');
                        }
                    });
                } else {
                    toHideEls.show();
                    $('#reset_message').fadeIn(500, function () {                       
                            if ($(this).hasClass('info_warning')) $(this).removeClass('info_warning');
                            if ($(this).hasClass('info_success')) $(this).removeClass('info_success');
                            $(this).addClass('info_failure');
                    });
                }
                $('#reset_message').text(data.message);
            }).error(function (jqXHR) {
                errEl.html('');
                errEl.append('<li>Không thực hiện được yêu cầu.' +
                             'Vui lòng refresh và thử lại hoặc liên hệ chúng tôi qua hotline để được hỗ trợ.</li>');
                errEl.parent('div.error').show();
                processEl.hide();
                toHideEls.show();
            });
            return false;
        });
    },
    checkEmptyStr: function (str) {
        result = str.replace(/\s+/gi, '');
        if (result == '') {
            return true;
        }
        return false;
    },
    validateEmail: function (str) {
        var mailPattern = /^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i;
        return mailPattern.test(str);
    }
}



$(document).ready(function () {
    loginJs.init();
})