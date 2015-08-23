var customerJs = {
    init: function () {
        customerJs.bindClickEventOrderLink();

        $('img.page_navigation').unbind('click').click(function () {
            if ($(this).attr('disabled') != 'disabled') {
                $('img.page_navigation').attr('disabled', 'disabled');
                var page = $(this).attr('id').replace('page_','');
                page = $(this).hasClass('next') ? (Number(page) + 1) : (Number(page) - 1);
                if (page <= 0) { $(this).css('visibility', 'hidden'); return false; }
                customerJs.loadOrdersByPage(page);
            }
            return false;
        });

        $(':input.required').unbind('change').change(function () {
            var id = $(this).attr('id');
            var id_origin = '#' + id + '_origin';
            if (id == 'customerPhone' && !customerJs.isValidPhoneNo($(this).val()))
            {
                customerJs.showHideFieldError($(this), true, 'Số điện thoại không hợp lệ');
                return false;
            }
            if (id == 'customerAddress' && $(this).val().length < 10) {
                customerJs.showHideFieldError($(this), true, 'Địa chỉ không hợp lệ.')
                return false;
            }
            if (customerJs.isStringEmpty($(this).val())) {
                customerJs.showHideFieldError($(this), true, 'Không thể bỏ trống dòng này.');
                return false;
            }
            customerJs.showHideFieldError($(this), false, '');
            if ($(this).val() != $(id).val()) {
                $(this).addClass('have_change');
            } else {
                if ($(this).hasClass('have_change')) $(this).removeClass('have_change');
            }
            return false;
        });

        $(':input[type=reset]').unbind('click').click(function () {
            var frmId = $(this).attr('rel');
            document.getElementById(frmId.replace('#', '')).reset();

            $(frmId).children().children().children(':input.have_change').removeClass('have_change');
            $(frmId).children().children().children(':input.input_validation_error').removeClass('input_validation_error');
            $(frmId).children().children().children('span.error').hide();
        });

        $(':input[type=button]').unbind('click').click(function () {
            var frmId = $(this).attr('rel');            
            var isValid = true;
            var email = $('#customerEmail').val();
            if (frmId === '#user_security_form')
            {
                $(frmId).children().children().children(':input').each(function () {
                    if (customerJs.isStringEmpty($(this).val())) {
                        customerJs.showHideFieldError($(this), true, 'Không thể bỏ trống dòng này.');
                        isValid = false;
                    }
                });
                if ($('#newPassword').val() != $('#confirmPassword').val()) {
                    customerJs.showHideFieldError($('#newPassword'), true, 'Mật khẩu mới và xác nhận mật khẩu không giống nhau');
                    customerJs.showHideFieldError($('#confirmPassword'), true, '');
                    isValid = false;
                }
                if ($('#newPassword').val().length < 6) {
                    customerJs.showHideFieldError($('#newPassword'), true, 'Mật khẩu phải có ít nhất 6 ký tự.');
                    isValid = false;
                }
                if (isValid) {
                    var data = { email: email, current: $('#currentPassword').val(), newp: $('#newPassword').val(), cfmp: $('#confirmPassword').val() };
                    var link = appBaseUrl + 'ajax/resetpassword';
                    var successCallback = function () {
                        $(frmId).children().children().children(':input[type=password]').each(function () {
                            $(this).val('');
                            if ($(this).hasClass("have_change")) $(this).removeClass('have_change');
                        });
                    };
                    customerJs.updateUser(data, link, successCallback);
                }
            } else if (frmId === '#user_info_form')
            {
                if ($(frmId).children().children().children(':input.has_change').length > 0) return false;
                $(frmId).children().children().children(':input:not(:disabled, :hidden)').each(function () {
                    if (customerJs.isStringEmpty($(this).val())) {
                        customerJs.showHideFieldError($(this), true, 'Không thể bỏ trống dòng này.');
                        isValid = false;
                    } else if ($(this).attr('Id') == 'customerPhone' && !customerJs.isValidPhoneNo($(this).val())) {
                        customerJs.showHideFieldError($(this), true, 'Số điện thoại không hợp lệ.');
                        isValid = false;
                    }
                });
                if (isValid) {
                    var data = {
                        name: $('#customerName').val(),
                        phone: $('#customerPhone').val(),
                        address: $('#customerAddress').val(),
                        district: $('#customerDistrict').val(),
                        receiveInfo: $('#registerNews').is(':checked')
                    };
                    var link = appBaseUrl + 'ajax/updatecustomer';
                    var onSuccessFunc = function () {
                        $(frmId).children().children().children(':input').each(function () {
                            if ($(this).hasClass('have_change')) $(this).removeClass('have_change');
                            $('#' + $(this).attr('id') + '_origin').val($(this).val());
                        });
                    };
                    customerJs.updateUser(data, link, onSuccessFunc);
                }
            }
            return false;
        });
    },
    bindClickEventOrderLink: function () {
        $('a.ajax_order_link').unbind('click').click(function () {
            if ($(this).attr('disabled') != 'disabled') {
                $('a.ajax_order_link').attr('disabled', 'disabled');
                var id = $(this).attr('id').replace('order_no_', '');
                customerJs.loadOrder(id, $(this).attr('rel'));
            }
            return false;
        });
        return false;
    },
    loadOrdersByPage: function (page) {
        var processingEl = $('#orders_table > table.std > tbody > tr:first');
        var tBodyEl = $('#orders_table > table.std > tbody');
        var errEl = $('div.user_tab').siblings('div.error');
        errEl.hide();
        tBodyEl.children('tr').fadeOut(500, function () {
            processingEl.fadeOut(300);
        });

        $.ajax({
            url: appBaseUrl + "ajax/get-customer-orders/" + page,
            type: "GET"
        }).success(function (data) {
            if (data.loggedIn) {
                tBodyEl.html(data.order_table);
                customerJs.bindClickEventOrderLink();
                $('#orders_table > table.std > tfoot > tr > td:first').text('Trang ' + data.page_info.Current + ' - Hiển thị ' + data.page_info.Count +
                  ' trong số ' + data.page_info.TotalItems + ' đơn hàng');
                $('img.page_navigation.prev').attr('id', 'page_' + data.page_info.Current);
                $('img.page_navigation.next').attr('id', 'page_' + data.page_info.Current);
                if (data.page_info.Current > 1)
                {
                    $('img.page_navigation.prev').css('visibility', 'visible');
                } else
                {
                    $('img.page_navigation.prev').css('visibility', 'hidden');
                }
                if (data.page_info.Current < data.page_info.TotalPage)
                {
                    $('img.page_navigation.next').css('visibility', 'visible');
                }
                else
                {
                    $('img.page_navigation.next').css('visibility', 'hidden');
                }
            } else {
                setTimeout(function () {
                    window.location = appBaseUrl + data.link;
                }, 3000);
            }
        }).error(function () {
            errEl.children('ol').html('<li>Xin lỗi bạn! Có sự cố xảy ra trong quá trình xử lý. Vui lòng refresh trang và thử lại hoặc liên hệ hotline để chúng tôi hỗ trợ.</li>');
            errEl.show("slow");
        }).always(function () {
            $('img.page_navigation').removeAttr('disabled', 'disabled');
        });
        return false;
    },
    loadOrder: function (id, orderNo) {
        var processingEl = $('div.order_details > p.processing');
        var errEl = processingEl.siblings('div.error');
        var orderEl = $('div#order_details');
        processingEl.show();
        orderEl.hide();
        errEl.hide();
        $.ajax({
            url: appBaseUrl + 'ajax/get-order-for-customer/'+ id + "/" + orderNo,
            type: 'GET'
        }).success(function (data) {
            if (data.loggedIn === true) {
                if (data.found) {
                    orderEl.html(data.order_detail);
                    orderEl.fadeIn(1000);
                } else {
                    errEl.children('ol').html('<li>Không tìm thấy đơn đặt hàng mã ' + orderNo + '. Vui lòng refresh trang và thử lại.</li>');
                    errEl.show("slow");
                }
            } else {
                setTimeout(function () {                    
                    window.location = appBaseUrl + data.link;
                }, 3000);                
            }            
        }).error(function () {
            errEl.children('ol').html('<li>Xin lỗi bạn! Có sự cố xảy ra trong quá trình xử lý. Vui lòng refresh trang và thử lại hoặc liên hệ hotline để chúng tôi hỗ trợ.</li>');
            errEl.show("slow");
        }).always(function () {
            processingEl.hide();
            $('a.ajax_order_link').removeAttr('disabled');
        });
    },
    updateUser: function (data, link, successFunc) {
        var successEl = $('div#message > p.info_success');
        var processingEl = $('div#message > p.processing');
        var errEl = $('#user_view_user_info').siblings('div.error');
        processingEl.show();
        errEl.hide();
        successEl.hide();
        $.ajax({
            url: link,
            data: data,
            type: "POST"
        }).success(function (data) {
            processingEl.fadeOut(300);
            if (data.isLoggedIn === false) {
                errEl.text('Phiên làm việc đã hết. Chúng tôi sẽ chuyển tiếp qua màn hình đăng nhập trong giây lát...');
                errEl.show(300).delay(5000).hide("slow", function () {
                    window.location = data.link;
                })
            } else if (data.found === false) {
                errEl.text('Sai mật khẩu hoặc dữ liệu không tồn tại. Vui lòng refresh lại trang và thử lại.');
                errEl.show();
            } else if (data.updated === false) {
                errEl.text('Lỗi: ' + data.error);
                errEl.show();
            } else {
                //update successfully
                successEl.html("Cập nhật thành công.");
                successEl.fadeIn(300).delay(5000).fadeOut(300);
                if (successFunc) successFunc();
            }           
        }).error(function () {
            processingEl.fadeOut(300);
            errEl.text('Lỗi trong quá trình xử lý. Vui lòng refresh lại trang và thử lại hoặc liên hệ hotline để được hỗ trợ.');
            errEl.show();
        });
    },
    isStringEmpty:function(str){
        var result = str.replace(/\s+/gi, '');
        if (result === '') return true;
        return false;
    },
    isValidPhoneNo: function (str) {
        var phonePattern = /[0-9]{8,11}$/i;
        return phonePattern.test(str);
    },
    showHideFieldError: function ($el, isError, errorText) {
        if (isError) {
            $el.fadeOut(300, function () {
                if ($el.hasClass('have_change')) $(this).removeClass('have_change');

                $el.addClass("input_validation_error");
                $el.fadeIn(300);
                $el.siblings('span.error').text(errorText).show();
            });
        } else if ($el.hasClass("input_validation_error")) {
            $el.removeClass("input_validation_error");
            $el.siblings('span.error').hide();
        }
    }
}

$(document).ready(function () {
    customerJs.init();
});