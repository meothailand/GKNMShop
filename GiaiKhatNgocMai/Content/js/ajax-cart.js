/*
* 2015 GiaikhatNgocMai shop online
*
*  @author meothailand
*  @copyright  2015 meothailand.com
*  @license    http://opensource.org/licenses/afl-3.0.php  Academic Free License (AFL 3.0)
*/


//JS Object : update the cart by ajax actions
var ajaxCart = {
    addItemUrl: appBaseUrl + "cart/add",
    updateItemUrl: appBaseUrl + "cart/update",
    deleteItemUrl: appBaseUrl + "cart/remove",
    //override every button in the page in relation to the cart
    overrideButtonsInThePage: function () {
        //for every 'add' buttons...
        $('.ajax_add_to_cart_button').unbind('click').click(function () {
            var idProduct = $(this).attr('rel').replace('ajax_id_product_', '');
            if ($(this).attr('disabled') != 'disabled') {
                $('.ajax_add_to_cart_button').attr('disabled', 'disabled');
                ajaxCart.add(ajaxCart.addItemUrl, idProduct, 1, null, null, null);
            }                
            return false;
        });
        //for product page 'add' button...
        $('#buy_block p#add_to_cart a').unbind('click').click(function () {
            $(this).attr('disabled', 'disabled');
            var prodId = $('#product_page_product_id').val();
            var qty = $('#quantity_wanted').val();
            if (isNaN(qty) || qty <= 0) {
                $(this).removeAttr('disabled');
                return false;
            }
            ajaxCart.add(ajaxCart.updateItemUrl, prodId, qty, null, null, null, function () {
                $('#buy_block p#add_to_cart a').removeAttr('disabled');
            });
            return false;
        });
        //for quantity change in product page...
        $('#quantity_wanted').unbind('change').change(function () {
            if (isNaN($(this).val()) || $(this).val() <= 0) {
                $(this).fadeOut(500, function () {
                    $(this).addClass('input_validation_error').fadeIn(100);
                })
            } else {
                if ($(this).hasClass('input_validation_error')) {
                    $(this).removeClass('input_validation_error');
                }
            }
            return false;
        });
        //for cart quantity up button in order page
        $('.cart_quantity_up').unbind('click').click(function () {
            var prodId = $(this).attr('id').replace('cart_quantity_up_', '');
            var qty = $('#quantity_' + prodId).val();
            if (ajaxCart.checkInputQuantity('quantity_' + prodId) != false) {
                ajaxCart.showProcessOrderTable(prodId, ajaxCart.add(ajaxCart.updateItemUrl, prodId, qty, null,
                                               function () { ajaxCart.showErrorMessage("Lỗi: không cập nhật được sản phẩm. Vui lòng refresh lại trang.") }, true),
                                           function () {
                                               if ($('#product_' + prodId)) { $('#product_' + prodId).fadeIn(200); }
                                           });
            } else {
                ajaxCart.showErrorMessage("Vui lòng nhập số lớn hơn 0.");
            }
            return false;
        });
        //for cart quantity delete button in order page
        $('.cart_quantity_down').unbind('click').click(function () {
            if (confirm("Bạn muốn bỏ sản phẩm này ra khỏi giỏ hàng?")) {
                var prodId = $(this).attr('id').replace('cart_quantity_delete_', '');
                ajaxCart.remove(prodId, function () { ajaxCart.showErrorMessage("Lỗi: không xoá được sản phẩm. Vui lòng refesh lại trang.") });
            }
            return false;
        });
        //for input quantity in order page
        $('.cart_quantity_input').unbind('change').change(function(){
            ajaxCart.checkInputQuantity($(this).attr('id'));
            return false;
        });
    },
    updateCartView: function (cartHtml, itemCount) {
        var cartBlock = $('#block_content');
        var smallCart = $('#shopping_cart > span');
        cartBlock.fadeToggle(1500, "swing", function () {
            cartBlock.html(cartHtml);
            smallCart.text(itemCount + " sản phẩm");
            cartBlock.fadeToggle("slow");
        })
        return false;
    },
    updateOrderTable: function (id, prodTotal, allProdTotal, shipment, totalOrder) {
        if (prodTotal == 0) {
            var trEl = $('table#cart_summary > tbody > tr#product_' + id);
            trEl.fadeOut(500, function () {
                trEl.remove();
                if ($('table#cart_summary > tbody > tr.cart_item').length == 0) {
                    $('#summary_products_quantity_p').remove();
                    $('#order-detail-content').fadeOut(1000, function () { $('#order-detail-content').remove(); });
                    $('#cart_voucher').fadeOut(1000, function () { $('#cart_voucher').remove(); });
                    $('p.cart_navigation').fadeOut(1000, function () { $(this).remove(); });
                    ajaxCart.showErrorMessage("Bạn không có sản phẩm nào trong giỏ.");
                }
            });
        } else {
            $('#total_product_price_' + id).text(prodTotal);
        }       
        $('#total_product').text(allProdTotal);
        $('#total_shipping').text(shipment);
        $('#total_price').text(totalOrder);
        return false;
    },
    checkInputQuantity:function(id){
        var inputEl = $('#'+ id);
        if (window.isNaN(inputEl.val()) || inputEl.val() <= 0) {
            inputEl.fadeOut(500, function () {
                inputEl.addClass('input_validation_error').fadeIn(100);
                ajaxCart.showErrorMessage("Vui lòng nhập số lớn hơn 0.");
            })
            return false;
        }
        else if (inputEl.hasClass('input_validation_error')) {
            inputEl.removeClass('input_validation_error');
        }
        return true;
    },
    showProcessOrderTable: function(id, proccessFunc, doneFunc){
        var trElem = $('#product_' + id);
        trElem.fadeOut(1000);
        $.when(proccessFunc).done(doneFunc());
    },
    showErrorMessage: function(message){
        var msgEl = $('#emptyCartWarning');
        var closeEl = $('img.warning');
        closeEl.click(function () {
            closeEl.hide(300);
            msgEl.hide(300);
        });
        msgEl.html(message);        
        msgEl.show(300, function () { closeEl.show(100); });
    },
    //add product
    add: function (resource_url, prodId, quantity, toppingId, errCallback, updateOrderTable) {
        $.ajax({
            url: resource_url,
            type: "POST",
            data: { itemId: prodId, quantity: quantity, toppingId: toppingId, getOrderRow: updateOrderTable }
        }).success(function (data) {
            ajaxCart.updateCartView(data.cartHtml, data.itemCount);
            if (updateOrderTable) ajaxCart.updateOrderTable(prodId, data.itemTotal, data.productsTotal, data.shipping, data.totalOrder);
        })
          .error(function (jqXHR) {
              if (errCallback) { errCallback;}
          }).always(function () {
              $('.ajax_add_to_cart_button').removeAttr('disabled');
          });
    },
    remove: function (prodId, errCallback) {
        $.ajax({
            url: ajaxCart.deleteItemUrl,
            type: "DELETE",
            data: { itemId: prodId }
        }).success(function (data) {
            ajaxCart.updateCartView(data.cartHtml, data.itemCount);
            ajaxCart.updateOrderTable(prodId, 0, data.productsTotal, data.shipping, data.totalOrder);
        })
          .error(function (jqXHR) {              
              if (errCallback) { errCallback; }
          });
    }
};

//when document is loaded...
$(document).ready(function(){
    ajaxCart.overrideButtonsInThePage();
});

