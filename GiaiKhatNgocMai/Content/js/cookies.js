(function($) {
$(function() {

	function createCookie(name,value,days) {
		if (days) {
			var date = new Date();
			date.setTime(date.getTime()+(days*24*60*60*1000));
			var expires = "; expires="+date.toGMTString();
		}
		else var expires = "";
		document.cookie = name+"="+value+expires+"; path=/";
	}
	function readCookie(name) {
		var nameEQ = name + "=";
		var ca = document.cookie.split(';');
		for(var i=0;i < ca.length;i++) {
			var c = ca[i];
			while (c.charAt(0)==' ') c = c.substring(1,c.length);
			if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
		}
		return null;
	}
	function eraseCookie(name) {
		createCookie(name,"",-1);
	}

	$('ul.product_view').each(function(i) {
		var cookie = readCookie('tabCookie'+i);
		if (cookie) $(this).find('li').eq(cookie).addClass('current').siblings().removeClass('current')
			.parents('#center_column').find('.box').hide().eq(cookie).show();
	})

	$('ul.product_view').delegate('li:not(.current)', 'click', function() {
		$(this).addClass('current').siblings().removeClass('current')
			.parents('#center_column').find('.box').hide().eq($(this).index()).show();
		var ulIndex = $('ul.product_view').index($(this).parents('ul.product_view'));
		eraseCookie('tabCookie'+ulIndex);
		createCookie('tabCookie'+ulIndex, $(this).index(), 365);
	})

	$('ul.user_view').each(function (i) {
	    var cookie = readCookie('tabCookie' + i);
	    if (cookie)
	    {
	        $(this).find('li').eq(cookie).addClass('current').siblings().removeClass('current')
                .parents('#center_column').find('.box').hide().eq(cookie).show();
	        $('span#user_description').text($(this).find('li.current').attr('title'));
	    }
	})

	$('ul.user_view').delegate('li:not(.current)', 'click', function () {
	    $(this).addClass('current').siblings().removeClass('current')
			.parents('#center_column').find('.box').hide().eq($(this).index()).show();
	    $('span#user_description').text($(this).attr('title'));
	    var ulIndex = $('ul.user_view').index($(this).parents('ul.user_view'));
	    eraseCookie('tabCookie' + ulIndex);
	    createCookie('tabCookie' + ulIndex, $(this).index(), 365);
	})

})
})(jQuery)