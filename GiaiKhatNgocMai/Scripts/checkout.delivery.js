var deliveryForm = {
    init: function () {
        //bind delivery button 
        $('a#delivery_form_btn').unbind('click').click(function () {
            if ($(this).hasClass('button_secondary')) {
                $(this).removeClass('button_secondary');
            }
            $('#nondelivery_form').fadeOut(300, function () {
                $('a#nondelivery_form_btn').addClass('button_secondary');
                $('#delivery_form').fadeIn(300);
            });
        });
        $('a#nondelivery_form_btn').unbind('click').click(function () {
            if ($(this).hasClass('button_secondary')) {
                $(this).removeClass('button_secondary');
            }
            $('#delivery_form').fadeOut(300, function () {               
                $('a#delivery_form_btn').addClass('button_secondary');
                $('#nondelivery_form').fadeIn(300);
            });
        });
        //bind on change event for shipment district select
        $('#shipment_district').unbind('change').change(function () {
            var feeEl = $('#shipmentfee');
            var spanEl = $('#fee_threshold');
            var totalEl = $('#order_total');
            var selected = $(this).val();
            if (deliveryForm.isEmptyStr(selected)) {
                deliveryForm.showHideErrorInput($(this), true);
                return false;
            }
            deliveryForm.showHideErrorInput($(this), false);
            var threshold = $('#shipfeethreshold > option[value="' + selected + '"]').text();
            if(Number(totalEl.val()) >= Number(threshold)){
                feeEl.css('text-decoration', 'line-through');
                spanEl.text('Miễn tính ship với đơn hàng từ ' + threshold + ' đ');
                spanEl.fadeIn(300);
            } else {
                feeEl.css('text-decoration', 'none');
                spanEl.fadeOut(300);
            }
            feeEl.val($('#shipfee > option[value="' + selected + '"]').text());
        });
        //bind change event for required inputs
        $('input.shipment_input').unbind('change').change(function () {
            if (deliveryForm.isEmptyStr($(this).val())) {
                deliveryForm.showHideErrorInput($(this), true);
                return false;
            }
            deliveryForm.showHideErrorInput($(this), false);
        });
        //bind click event for submit button
        $('#submitsave').unbind('click').click(function () {
            var frmEl = $('#delivery_form');
            var distEl = $('#shipment_district');
            var addressEl = $('#shipment_address');
            var shipDateEl = $('#shipment_date');
            var isValid = true;
            
            if (deliveryForm.isEmptyStr(distEl.children('option:selected').val())) {
                isValid = false;
                deliveryForm.showHideErrorInput(distEl, true);
            }
            if (deliveryForm.isEmptyStr(addressEl.val())) {
                isValid = false;
                deliveryForm.showHideErrorInput(addressEl, true);
            }
            if (deliveryForm.isEmptyStr(shipDateEl.val())) {
                isValid = false;
                deliveryForm.showHideErrorInput(shipDateEl, true);
            }

            if (isValid) {
                frmEl.submit();
            }
        });
        $('#submitsave_non').unbind('click').click(function () {
            $('#nondelivery_form').submit();
        });
        return false;
    },
    showHideErrorInput: function ($el, isError) {
        if (isError) {
            $el.fadeOut(500, function () {
                $el.addClass('input_validation_error');
                $el.fadeIn(100);
            })
        } else {
            if ($el.hasClass('input_validation_error'))
                $el.removeClass('input_validation_error');
        }
    },
    isEmptyStr: function (str) {
        var result = str.replace(/\s+/gi, '');
        if (result == '') return true;
        return false;
    }
}

$(document).ready(function () {
    deliveryForm.init();
})