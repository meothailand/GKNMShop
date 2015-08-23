var accountJs = {
    init: function () {
        //bind on change event for required text input
        $('input[type=text].required').unbind('change').change(function () {
            if(accountJs.isEmptyString($(this).val())){
                accountJs.showHideError($(this), true);
            } else {
                if ($(this).attr('id') === 'customerPhone' && !accountJs.isPhoneNumber($(this).val()))
                {
                    accountJs.showHideError($(this), true);
                }
                else if ($(this).attr('id') === 'customerEmail' && !accountJs.isEmailFormat($(this).val()))
                {
                    accountJs.showHideError($(this), true);
                }
                else if ($(this).attr('id') === 'customerPassword') {
                    accountJs.showHideError($(this), true);
                    alert($(this).val().length);
                }
                else
                {
                    accountJs.showHideError($(this), false);
                }
            }
            return false;
        });
        $('input[type=password].required').unbind('change').change(function () {
            if (accountJs.isEmptyString($(this).val())) {
                accountJs.showHideError($(this), true);
            } else {
                if ($(this).attr('id') === 'customerPassword' && $(this).val().length < 6) {
                    accountJs.showHideError($(this), true);
                } else {
                    accountJs.showHideError($(this), false);
                }

                if ($(this).attr('id') === 'confirmPassword' && $(this).val() != $('#customerPassword').val())
                {
                    accountJs.showHideError($(this), true);
                } else {
                    accountJs.showHideError($(this), false);
                }
            }
            return false;
        });
        //bind on change event for select option
        $('#shipDistrict').unbind('change').change(function () {
            if (accountJs.isEmptyString($(this).children(':selected').val())) {
                accountJs.showHideError($(this), true);
            } else {
                accountJs.showHideError($(this), false);
            }
            return false;
        });
        //bind click event for submit button
        $('#submit_button').unbind('click').click(function () {
            var errListEl = $('#error_list');
            errListEl.html('');
            var validatedOk = true;
            var emptyErrMsg = "Vui lòng điền các thông tin: ";
            $('.required').each(function () {
                if (accountJs.isEmptyString($(this).val())) {
                    emptyErrMsg += ($(this).attr('title') + ', ');
                    accountJs.showHideError($(this), true);
                    validatedOk = false;
                }
            });

            if (validatedOk === false) errListEl.append('<li>' + emptyErrMsg + '</li>');

            if (!accountJs.isPhoneNumber($('#customerPhone').val())) {
                accountJs.showHideError($('#customerPhone'), true);
                errListEl.append('<li>Số điện thoại không đúng</li>');
                validatedOk = false;
            }

            if (!accountJs.isEmailFormat($('#customerEmail').val())) {
                accountJs.showHideError($('#customerEmail'), true);
                errListEl.append('<li>Địa chỉ email không đúng</li>');
                validatedOk = false;
            }

            if ($('#customerPassword').val().length < 6) {
                accountJs.showHideError($('#customerPassword'), true);
                errListEl.append('<li>Mật khẩu phải dài ít nhất 6 ký tự</li>');
                validatedOk = false;
            }

            if ($('#customerPassword').val() != $('#confirmPassword').val()) {
                accountJs.showHideError($('#customerPassword'), true);
                accountJs.showHideError($('#confirmPassword'), true);
                errListEl.append('<li>Mật khẩu và xác nhận mật khẩu không giống nhau</li>');
                validatedOk = false;
            }
           
            if (validatedOk) {
                $('#create-account_form').submit();
            } else {
                errListEl.parent().show();
            };
            return false;
        });
    },
    isEmptyString: function (str) {
        var result = str.replace(/\s+/gi, '');
        if (result == '') return true;
        return false;
    },
    isPhoneNumber: function(str){
        var phonePattern = /[0-9]{8,11}$/i;
        return phonePattern.test(str);
    },
    isEmailFormat: function (emailAddress) {
        var mailPattern = /^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i;
        return mailPattern.test(emailAddress);
    },
    showHideError: function(el, isErr){
        if(isErr){
            el.fadeOut(300, function(){
                if (!el.hasClass("input_validation_error")) el.addClass("input_validation_error");
                el.fadeIn(300);                
            });
        }else if(el.hasClass("input_validation_error")){
            el.removeClass("input_validation_error");
        }
    }
}

$(document).ready(function () {
    accountJs.init();
})