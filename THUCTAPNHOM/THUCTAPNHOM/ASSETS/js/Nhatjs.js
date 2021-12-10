// Change pass=====================================
function a(user) {
    var e1 = document.getElementById("old_password");
    var e2 = document.getElementById("new_password");
    var e3 = document.getElementById("new_password2");
    var oldpass = e1.value;
    var newpass = e2.value;
    var newpass2 = e3.value;
    $.ajax({
        type: "GET",
        data: {
            user: user, a: oldpass, b: newpass, c: newpass2
        },
        url: "/UserInformation/Pass_Change_Action",
        success: function (e) {
            if (e == "0") {
                alert("Đổi mật khẩu thành công!!")
            }
            if (e == "1") {
                alert("Mật khẩu cũ không đúng!!")
            }
            if (e == "2") {
                alert("Xác nhận mật khẩu mới không đúng!!!")
            }
        }
    })
}

function change_pass_button() {
    var e = document.getElementById("12ab")
    if (e != null) {
        e.addEventListener('click', function () {
            var user = e.name
            a(user)
        })
    }

}

window.addEventListener('load', function () {
    change_pass_button()
})

// Change pass=====================================end

// Add_to_cart=======================================

function add_to_cart_button() {
    var e = document.getElementsByClassName("btn btn-addto-cart a");
    for (var i = 0; i < e.length; i++) {
        button = e[i]
        button.addEventListener('click', function (event) {
            var buttonclicked = event.target
            var is_logined = String(buttonclicked.name).substring(0, 1);
            var product_id = String(buttonclicked.name).substring(1, 4);
            var user_logined = String(buttonclicked.id)
            Add_To_Cart_Click(is_logined, product_id, user_logined);
        })
    }
}

function add_minicart_qty(user_logined) {
    $.ajax({
        type: "GET",
        data: { user_logined: user_logined },
        url: "/Product/Get_Cart_Qty",
        success: function (e) {
            console.log(e)
            var a = document.getElementById("CartCount")
            a.innerHTML = e
        }
    })
}

function Add_To_Cart_Click(a, b, c) {
    if (String(a) == "1") {
        $.ajax({
            type: "GET",
            data: { id: b },
            url: "/Product/Add_To_CartAjax",
            success: function (e) {
                alert("Thêm vào giỏ hàng thành công!!");
                add_minicart_qty(c)

            }
        })
    }
    else {
        alert("Bạn chưa đăng nhập!!");
    }
}

function Add_To_Cart_Click2(a) {
    if (String(a) == "1") {
        alert("Thêm vào giỏ hàng thành công!!");
    }
    else {
        alert("Bạn chưa đăng nhập !!");
    }
}

window.addEventListener('load', function () {
    add_to_cart_button()
})

// Add_to_cart=======================================end

// Update_memer=======================================
function Update_Member_Information(id) {
    var e1 = document.getElementById("member_name_id");
    var e2 = document.getElementById("member_phone_id");
    var e3 = document.getElementById("member_address_id");
    var name = e1.value;
    var phone = e2.value;
    var address = e3.value;
    $.ajax({
        type: "GET",
        data: {
            member_id: id, name: name, phone: phone, address: address
        },
        url: "/UserInformation/update_information",
        success: function (e) {
            alert("Cập nhật thông tin thành công!!")
        }
    })
}
// Update_memer=======================================end


// tang giam trong cart==========================
/*
function button_plus_click() {
    var e = document.getElementsByClassName("qtyBtn")
    for (var i = 0; i < e.length; i++) {
        var button = e[i]
        button.addEventListener('click', function (event) {
            var buttonclicked = event.target
            var product_id = String(buttonclicked.name).replace("plus_button", "");
            var amount_e_id = "amount" + product_id;
            var qty_e_id = "qty" + product_id;
            var price_id = "price" + product_id;
            var amount_e = document.getElementById(amount_e_id);
            var qty_e = document.getElementById(qty_e_id)
            var price_e = document.getElementById(price_id)
            var price = price_e.innerHTML.replace(" VND", "")
            amount_e.innerHTML = parseInt(qty_e.value) * parseInt(price);
            count_all_amount_after_plus()
        })
    }
}

function count_all_amount_after_plus() {
    var all_amount = 0;
    var e1 = document.getElementsByClassName("money a")
    for (var k = 0; k < e1.length; k++) {
        var price_index = e1[k].innerHTML.replace(" VND", "")
        all_amount = parseInt(all_amount) + parseInt(price_index)
        var all_amount_string = String(all_amount) + " VND"
    }
    var all_amount_e = document.getElementById("tongtien")
    all_amount_e.innerHTML = all_amount_string

    var chietkhau_e = document.getElementById("chietkhau")
    var chietkhau = parseInt(chietkhau_e.innerHTML.substring(0, 7))

    var total_string = String(all_amount + chietkhau) + " VND"

    var total_e = document.getElementById("total_money")
    total_e.innerHTML = total_string
}
*/
window.addEventListener('load', function () {
    button_plus_click("qtyBtn")
})

// ====== remove_cart_item =================

function remove_cart_item() {
    var e = document.getElementsByClassName("anm anm-times-l remove")
    for (var i = 0; i < e.length; i++) {
        var button = e[i]
        button.addEventListener('click', function (event) {
            var buttonclicked = event.target
            var size = buttonclicked.id
            var id = buttonclicked.getAttribute("name")
            buttonclicked.parentElement.parentElement.parentElement.remove()
            $.ajax({
                type: "GET",
                data: {
                    product_id: id,
                    size: size
                },
                url: "/Cart/Remove_Item",
                success: function (e) {
                    count_all_amount_after_plus()
                }
            })
        })
    }
}

window.addEventListener('load', function () {

    remove_cart_item()
})

//===============Mini cart ==============

function mini_cart_count_all_amount() {

}


window.addEventListener('load', function () {
    mini_cart_count_all_amount()
})

//=================Tang giam toi ưu

function button_plus_click(class_name) {
    var e = document.getElementsByClassName(class_name)
    for (var i = 0; i < e.length; i++) {
        var button = e[i]
        button.addEventListener('click', function (event) {
            var buttonclicked = event.target
            var product_id = buttonclicked.id
            var amount_e_id = "amount" + product_id;
            var qty_e_id = "qty" + product_id;
            var price_e_id = "price" + product_id;
            button_plus(price_e_id, qty_e_id, amount_e_id)
        })
    }
}

function button_plus(price_e_id, qty_e_id, amount_e_id) {
    var amount_e = document.getElementById(amount_e_id);
    var qty_e = document.getElementById(qty_e_id)
    var price_e = document.getElementById(price_e_id)
    var price = price_e.innerHTML.replace(" VND", "")
    amount_e.innerHTML = parseInt(qty_e.value) * parseInt(price);
    count_all_amount_after_plus()
}


function count_all_amount_after_plus() {
    var all_amount = 0;
    var total = 0;
    var chietkhau = 0;
    var amount_class = "money amount";
    var chietkhau_id = "chietkhau";
    var all_amount_id = "all_amount";
    var total_id = "total_final";

    var e1 = document.getElementsByClassName(amount_class)
    for (var k = 0; k < e1.length; k++) {
        var price_index = e1[k].innerHTML.replace(" VND", "")
        all_amount = parseInt(all_amount) + parseInt(price_index)
    }

    var all_amount_e = document.getElementById(all_amount_id)
    all_amount_e.innerHTML = String(all_amount) + " VND"

    var chietkhau_e = document.getElementById(chietkhau_id)
    chietkhau = all_amount / 10
    chietkhau_e.innerHTML = String(chietkhau) + " VND"



    var total_e = document.getElementById(total_id)
    total = all_amount + chietkhau;
    total_e.innerHTML = String(total) + " VND"
}


// Subcriber
window.addEventListener('load', function () {
    dieukhoan_click()
})

function dieukhoan_click() {
    $(document).ready(function () {
        $("#dieukhoan_checkbox").click(function () {
            if ($("#dieukhoan_checkbox").is(":checked")) {
                document.getElementById("cartCheckout").disabled = false
            }
            else {
                document.getElementById("cartCheckout").disabled = true
            }

        })
    })
}

// Them review cho san pham
window.addEventListener('load', function () {
    button_review_click()
})

function button_review_click() {
    var e = document.getElementById("review_button")
    e.addEventListener('click', function () {
        var str = e.name
        var arr = str.split(",")
        var product_id = arr[0]
        var username = arr[1]
        $.ajax({
            type: "GET",
            data: {
                product_id: product_id,
                username: username
            },
            url: "/Product/Add_Review_Check",
            success: function (e) {
                if (e == "1") {
                    var input_e = document.getElementById("review_body_10508262282")
                    var input_content = input_e.value
                    input_e.value = "";
                    $.ajax({
                        type: "GET",
                        data: {
                            review: input_content,
                            product_id: product_id,
                            username: username
                        },
                        url: "/Product/Add_Review",
                        success: function (e) {
                            alert("Review của bạn sẽ được xét duyệt để đăng tải!!!")
                        }
                    })
                }
                else {
                    alert("Bạn chưa mua sản phẩm, không thể đăng Review!!!")
                }
            }
        })
    })
}


// Number of Minicart
window.addEventListener('load', function () {
    number_minicart_edit()
    // Theem su kien cho button mua hàng
})

function number_minicart_edit() {
    var number = 0;
    var e = document.getElementById("hidden_div_id")
    var user = e.innerHTML
    console.log(user)
    $.ajax({
        type: "GET",
        data: {
            username: user,
        },
        url: "/Cart/Number_For_MiniCart",
        success: function (e) {
            var e2 = document.getElementById("CartCount")
            e2.innerHTML = e
        }
    })
}

