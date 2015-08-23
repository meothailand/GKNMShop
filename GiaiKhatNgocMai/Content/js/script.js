$(function(){
   $('#setCurrency').jqTransform({imgPath:'jqtransformplugin/img/'});
});
$(function(){
	$("#tmbannerblock2 a").hover(function(){
		$(this).find("img").stop().animate({top:"-5px"}, "normal", "easeOutBack") 
	}, function(){
		$(this).find("img").stop().animate({top:"0px"}, "normal")
	});
});

