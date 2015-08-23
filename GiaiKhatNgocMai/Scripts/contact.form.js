var contactForm = {
	init: function () {
		$(':input.required').unbind('change').change(function () {
			return contactForm.validateForm($(this));
		});

		$(':input[type=button]#submit').unbind('click').click(function () {
			var validForm = true;
			var processEl = $('#processing');
			var btn = $(this);
			var errEl = $('div.error');
			$(':input.required').each(function () {
				validForm = validForm && contactForm.validateForm($(this));
			});

			if (validForm) {
				processEl.show();
				btn.hide();
				errEl.hide();
				$('#sent_message').hide();
				$.ajax({
					url: appBaseUrl + 'ajax/sendmessage',
					type: 'POST',
					data: { message: $('#message').val(), name: $('#name').val(), email: $('#email').val() }
				}).success(function (data) {
					if (data.sent == true) {
						$(':input.required').each(function () {
							$(this).val('');
						});
						$('#sent_message').fadeIn(5000, function () {
							$(this).fadeOut(300);
						});
					} else {
						errEl.children('ol').html('<li>' + data.error + '</li>');
						errEl.show();
					}
				}).error(function () {
					errEl.children('ol').html('<li>Không thể gửi tin cho hệ thống. Vui lòng refresh trang và thử lại hay liên hệ hotline của chúng tôi.</li>');
					errEl.show();
				}).always(function () {
					processEl.hide();
					btn.show();
				});
			}
		});
	},
	validateForm: function($el){
		var id = $el.attr('id');
		if (contactForm.isStringEmpty($el.val())) {
			contactForm.showHideError($el, true, 'Vui lòng điền thông tin.');
			return false;
		} else if (id === 'email' && !contactForm.validateEmail($el.val())) {
			contactForm.showHideError($el, true, 'Địa chỉ email không hợp lệ.');
			return false;
		} else if (id === 'message' && $el.val().trim().length < 50) {
			contactForm.showHideError($el, true, 'Nội dung tin nhắn phải từ 50 ký tự trở lên.');
			return false;
		}
		contactForm.showHideError($el, false, '');
		return true;	
	},
	validateEmail:function(email){
		var mailPattern = /^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i;
		return mailPattern.test(email);
	},
	isStringEmpty: function(str){
		var result = str.replace(/\s+/gi, '');
		if (result == '') return true;
		return false;
	},
	showHideError: function ($el, iserror, errortext) {
	    var elId = $el.attr('id');
		if (iserror) {
			$el.addClass('input_validation_error');
			$el.siblings('span.error[rel='+ elId +']').text(errortext);
	    }
	    else
		{
			if ($el.hasClass('input_validation_error')) $el.removeClass('input_validation_error');
			$el.siblings('span.error[rel=' + elId + ']').text('');
		}
	}
};

$(document).ready(function () {
	contactForm.init();
});