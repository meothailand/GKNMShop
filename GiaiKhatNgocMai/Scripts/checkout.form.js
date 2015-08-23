var checkoutForm = {
    init:function(){
        $('h3.show-hide-form').parent('fieldset.toHidden').children(':not(h3)').hide();        
    },
	setEventToControls: function () {
		//for show hide forms
		$('h3.show-hide-form').unbind('click').click(function () {
			var frmId = $(this).attr('rel');
			var toHideEls = $('#' + frmId).children().children(':not(h3)');
			var toShowEls = $(this).parent().children(':not(h3)');
			var errEl = $('div.error');
			$(toHideEls).fadeOut(1000, function () {
				$(toHideEls).hide();
				$(toShowEls).show('slow');
				errEl.hide();
			});
			checkoutForm.resetForm(frmId);
			return false;
		});
		//for required input change
		$('input.required').unbind('change').change(function () {
			if (checkoutForm.checkEmptyStr($(this).val())) {
				$(this).fadeOut(500, function () {
					$(this).addClass('input_validation_error');
					$(this).fadeIn(200);
				});
			} else {
				if ($(this).hasClass('input_validation_error'))
					$(this).removeClass('input_validation_error');
			}
			return false;
		});
		//for button submit
		$('input[type=button]').unbind('click').click(function () {
			var btnId = $(this).attr('id');
			var errEl = $('div.error');
			var isValid = true;
			errEl.children('ol').html('');
			if (btnId == 'login_button') {
				var emailEl = $('#customer_email');
				var passwordEl = $('#customer_password');
				if (!checkoutForm.validateEmail(emailEl.val())) {
					isValid = false;
					checkoutForm.setErrorInput(emailEl);
					errEl.children('ol').append('<li>Vui lòng nhập địa chỉ email hợp lệ</li>');
					errEl.show();
				}
				if (checkoutForm.checkEmptyStr(passwordEl.val())) {
					isValid = false;
					checkoutForm.setErrorInput(passwordEl);
					errEl.children('ol').append('<li>Vui lòng nhập password</li>');
					errEl.show();
				}
				if (isValid == true) $('#login_form').submit();
				return false;
			} else {
				var nameEl = $('#customer_name');
				var phoneEl = $('#customer_phone');
				var newemailEl = $('#email_address');

				if (checkoutForm.checkEmptyStr(nameEl.val())) {
					isValid = false;
					checkoutForm.setErrorInput(nameEl);
					errEl.children('ol').append('<li>Vui lòng nhập tên khách hàng</li>');
					errEl.show();
				}
				if (!checkoutForm.validatePhone(phoneEl.val())) {
					isValid = false;
					checkoutForm.setErrorInput(phoneEl);
					errEl.children('ol').append('<li>Vui lòng nhập số điện thoại liên lạc hợp lệ</li>');
					errEl.show();
				}
				if (!checkoutForm.checkEmptyStr(newemailEl.val()) && !checkoutForm.validateEmail(newemailEl.val())) {
					isValid = false;
					checkoutForm.setErrorInput(newemailEl);
					errEl.children('ol').append('<li>Vui lòng nhập địa chỉ email hợp lệ</li>');
					errEl.show();
				}
				if (btnId == 'submit_create_button' && !checkoutForm.validateEmail(newemailEl.val())) {
					isValid = false;
					checkoutForm.setErrorInput(newemailEl);
					errEl.children('ol').append('<li>Bạn cần nhập email hợp lệ để tạo tài khoản</li>');
					errEl.show();
				}
				if (isValid == true) $('#create-account_form').submit();
				return false;
			}
		})
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
	},
	validatePhone: function (str) {
		var phonePattern = /[0-9]{8,11}/;
		return phonePattern.test(str);
	},
	setErrorInput: function ($el) {
		$el.fadeOut(500, function () {
			$el.addClass('input_validation_error');
			$el.fadeIn(100);
		})
	},
	resetForm: function (id) {
		document.getElementById(id).reset();
		var $inputs = $('#' + id + ' > fieldset > p > input');
		for(var i = 0; i < $inputs.length; i++){
			if ($($inputs[i]).hasClass('input_validation_error')) $($inputs[i]).removeClass('input_validation_error')
		}
		return false;
	}
};

$(document).ready(function () {
    checkoutForm.init();
	checkoutForm.setEventToControls();
})