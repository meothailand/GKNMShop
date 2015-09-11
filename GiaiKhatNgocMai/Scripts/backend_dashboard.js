var dashboardJs =
{
    init: function () {
        $('a.more').unbind('click').click(function () {
            var rel = $(this).attr('rel');
            var colorClass = $(this).attr('data-color');
            var requestUrl = backendBaseUrl + 'dashboard/';
            if (rel === 'new_orders') requestUrl += 'getunconfirmedorders';
            if (rel === 'today_delivery_orders') requestUrl += 'gettodaydeliverorders';
            if (rel === 'upcoming_orders') requestUrl += 'getupcomingorders';
            dashboardJs.ajaxGetOrders(requestUrl, colorClass);
            return false;
        });
        return false;
    },
    ajaxGetOrders: function (url, colorClass) {
        var loadingEl = $('div#ajax_loading');
        loadingEl.removeClass('hidden');
        $('div#orders_table').remove();
        $.ajax({
            url: url,
            type: 'GET'
        }).success(function (data) {
            $(data).insertAfter(loadingEl);
            $('div#orders_table').children('div').children('div.portlet').addClass(colorClass);
        }).error(function () {
            $('div#ajax_loading_error').removeClass('hidden');
        }).always(function () {
            loadingEl.addClass('hidden');
        });
    },
    updateOrderCount: function() {
        $.ajax({
            url: backendBaseUrl + 'dashboard/getordercountinmonth'
        }).success(function (data) {
            var value = '';
            $('div.number').each(function () {
                switch ($(this).attr('rel')) {
                    case 'new_orders':
                        $(this).fadeOut(200, function () {
                            $(this).text(data.toConfirmOrder).fadeIn(200);
                        }); 
                        break;
                    case 'today_delivery_orders':
                        $(this).fadeOut(200, function () {
                            $(this).text(data.todayDeliveryOrder).fadeIn(200);
                        });
                        break;
                    case 'upcoming_orders':
                        $(this).fadeOut(200, function () {
                            $(this).text(data.upcomingOrder).fadeIn(200);
                        });
                        break;
                    case 'total_orders':
                        $(this).fadeOut(200, function () {
                            $(this).text(data.totalOrder).fadeIn(200);
                        });
                        break;
                    default: break;
                }                
            });
            return false;
        }).error(function () {

        });
    }
};


jQuery(document).ready(function () {
    dashboardJs.init();
    var updateTimer = setInterval(function () { dashboardJs.updateOrderCount(); }, 60000);
});