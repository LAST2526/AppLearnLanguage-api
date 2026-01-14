
function formatNumber(nStr, decSeperate, groupSeperate) {
    nStr += '';
    x = nStr.split(decSeperate);
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + groupSeperate + '$2');
    }
    return x1 + x2;
}
//======= SSKEY Common JS ==== //

function PostAjax(strUrl, obj, fnSucccess) {
    var DTO = {};
    DTO.dataPost = JSON.stringify(obj);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: strUrl,
        data: JSON.stringify(DTO),
        datatype: "json",
        success: fnSucccess,
        failure: saveFail
    });
}

function isEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

// $("#fupload").get(0).files
//Post object and file
function PostAjaxAndFile(strUrl, obj, file, fnSucccess) {
    var dataObj = new FormData;
    dataObj.append("DataInfo", JSON.stringify(obj));
    dataObj.append("ImageFile", file[0]);
    $.ajax({
        type: 'POST',
        url: strUrl,
        data: dataObj,
        contentType: false,
        processData: false,
        xhrFields: {
            responseType: 'blob'
        },
        success: fnSucccess
    });
}


//Post Data
function PostAjaxForm(strUrl, dtPost, fnSucccess) {
    $.ajax({
        type: 'POST',
        url: strUrl,
        data: dtPost,
        contentType: false,
        processData: false,
        success: fnSucccess
    });
}

function saveSuccess(d) {
    alert("Save ok");
}

function saveFail(d) {
    alert("Fail");
}

/* === PopUp ===*/

function showPopUp(url, popTitle) {
    var hmtlMast = "<div id='bg-Mask'></div>";
    $("body").append(hmtlMast);
    $("#popTitle").html(popTitle);
    $("#SSKeyPopUp").fadeIn(500);
    $("#PopUpContent").load(url, function () { marginPop(); });
}

function marginPop() {
    var divWidth = $("#SSKeyPopUp").width();
    var wdWidth = $("#bg-Mask").width();
    var spWidth = (wdWidth - divWidth) / 2;

    var divHeight = $("#SSKeyPopUp").height();
    var wdHeight = window.innerHeight;
    var spHeight = 0;
    if (wdHeight > divHeight)
        spHeight = (wdHeight - divHeight) / 2;
    $("#SSKeyPopUp").css({ marginLeft: spWidth + "px", marginTop: "10px" });
}

function closePopUp(fnRecall, obj) {
    $("#SSKeyPopUp").fadeOut(100);
    $("#PopUpContent").html("");
    $("#popTitle").html("");
    $("#bg-Mask").remove();

    if (fnRecall !== "") {
        this[fnRecall](obj);
    }
}

function fnRefesh(obj) {
    if (obj === 1)
        window.location.reload = true;
}

function checkPhoneNumber(strPhone) {
    var flag = false;
    var phone = strPhone.trim(); // ID của trường Số điện thoại
    phone = phone.replace('(+84)', '0');
    phone = phone.replace('+84', '0');
    phone = phone.replace('0084', '0');
    phone = phone.replace(/ /g, '');
    if (phone !== '') {
        var firstNumber = phone.substring(0, 2);
        if ((firstNumber === '09' || firstNumber === '08') && phone.length === 10) {
            if (phone.match(/^\d{10}/)) {
                flag = true;
            }
        } else if (firstNumber === '01' && phone.length === 11) {
            if (phone.match(/^\d{11}/)) {
                flag = true;
            }
        }
    }
    return flag;
}

//----------Popupdialog----create:thuyntc-18/10/18--//

function popupdialog() {

    var dialog, form,
        tips = $(".validateTips");
    dialog = jQuery_1_12_4("#dialog-form").dialog({
        title: $("#message").val(),
        dialogClass: "custom-dialog-1",
        height: 600,
        width: 700,
        modal: true,
        open: function (event, ui) {//ẩn button close 
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });
}

function btnpopup(url, title) {
    $(".ui-dialog-title").html(title);
    $("#panel-body").load(url, function () {
    });

}

//-----Show message--------//
///Hiển thị div thông báo. message: nội dung, color:màu sắc,   type: 0(gọi hàm) ,1 (không gọi)
function showmessage(message, color, type) {
    $('.ajax-box').hide();
    $("#sms-message").html(message);
    $("#sms-message").attr("style", "color:" + color);
    $("#sms-message").fadeIn;

    if (type === 0) {
        setTimeout(function () {
            $("#sms-message").fadeOut(1000);
            $("#sms-message").html("");
        }, 2000);
    }
    else return;
}

function ShowInformation(message, color, type, id) {
    $('.ajax-box').hide();
    $("#" + id).html(message);
    $("#" + id).attr("style", "color:" + color);
    $("#" + id).fadeIn;

    if (type === 0) {
        setTimeout(function () {
            $("#" + id).fadeOut(5000);
            $("#" + id).html("");
        }, 4000);
    }
    else return;
}

// FORMAT TIEN
function addPeriod(nStr) {
    nStr += '';
    x = nStr.split(',');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return x1 + x2;
}

function number_format(number, decimals, dec_point, thousands_sep) {
    // * example 1: number_format(1234.5678, 2, '.', '');
    // * returns 1: 1234.57
    var n = number, c = isNaN(decimals = Math.abs(decimals)) ? 2 : decimals;
    var d = dec_point === undefined ? "," : dec_point;
    var t = thousands_sep === undefined ? "." : thousands_sep, s = n < 0 ? "-" : "";
    var i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
}

//Onchange price is format
function OnChangePriceFormat(_id) {
    var value = $("#" + _id).val();
    if (value.includes(".")) {
        value = value.replace(".", "");
        if (value.includes(".")) {
            value = value.replace(".", "");
            if (value.includes(".")) {
                value = value.replace(".", "");
                if (value.includes(".")) {
                    value = value.replace(".", "");
                    if (value.includes(".")) {
                        value = value.replace(".", "");
                        if (value.includes(".")) {
                            value = value.replace(".", "");
                            if (value.includes(".")) {
                                value = value.replace(".", "");
                            }
                        }
                    }
                }
            }
        }
    }
    var _hasFormat = formatNumber(value, ',', '.');
    $("#" + _id).val(_hasFormat);
    OnChangePrice();
}

//Onchange price is format
function OnChangePriceDot(_id) {
    var value = $("#" + _id).val();
    if (value.includes(".")) {
        value = value.replace(".", "");
        if (value.includes(".")) {
            value = value.replace(".", "");
            if (value.includes(".")) {
                value = value.replace(".", "");
                if (value.includes(".")) {
                    value = value.replace(".", "");
                }
            }
        }
    }
    var _hasFormat = formatNumber(value, ',', '.');
    $("#" + _id).val(_hasFormat);
}

//Onchange price is format
function OnChangePriceFormat_V2(_id, func) {
    var value = $("#" + _id).val();
    if (value.includes(".")) {
        value = value.replace(".", "");
        if (value.includes(".")) {
            value = value.replace(".", "");
            if (value.includes(".")) {
                value = value.replace(".", "");
                if (value.includes(".")) {
                    value = value.replace(".", "");
                }
            }
        }
    }
    var _hasFormat = formatNumber(value, ',', '.');
    $("#" + _id).val(_hasFormat);
    func;
}

function OnChangePriceFormatData(element) {
    var _id = $(element).attr('data');
    var value = $("#" + _id).val();
    if (value.includes(".")) {
        value = value.replace(".", "");
        if (value.includes(".")) {
            value = value.replace(".", "");
            if (value.includes(".")) {
                value = value.replace(".", "");
                if (value.includes(".")) {
                    value = value.replace(".", "");
                }
            }
        }
    }
   
    var _hasFormat = parseFloat(value);
    _hasFormat = formatNumber(_hasFormat, ',', '.');
    if (value === "") {
        $("#" + _id).val("0");
    }
    else {
        $("#" + _id).val(_hasFormat);
    }

}
function ChangePriceFormatReturn(value) {
    if (value.includes(".")) {
        value = value.replace(".", "");
        if (value.includes(".")) {
            value = value.replace(".", "");
            if (value.includes(".")) {
                value = value.replace(".", "");
                if (value.includes(".")) {
                    value = value.replace(".", "");
                }
            }
        }
    }
    var _hasFormat = formatNumber(value, ',', '.');
    return _hasFormat;
}
function replaceNumber(value) {
    if (value.includes(".")) {
        value = value.replace(".", "");
        if (value.includes(".")) {
            value = value.replace(".", "");
            if (value.includes(".")) {
                value = value.replace(".", "");
                if (value.includes(".")) {
                    value = value.replace(".", "");
                }
            }
        }
    }
    return value;
}

function FormatMoneyTND(Num) {
    Num += '';
    Num = Num.replace('.', ''); Num = Num.replace('.', ''); Num = Num.replace('.', '');
    Num = Num.replace('.', ''); Num = Num.replace('.', ''); Num = Num.replace('.', '');
    x = Num.split(',');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    return x1 + x2;
}

function keypress(e) {
    if (window.event) {
        keypressed = window.event.keyCode; //IE
    }
    else {
        keypressed = e.which; //NON-IE, Standard
    }
    if (keypressed < 48 || keypressed > 57) {
        if (keypressed === 8 || keypressed === 127) {
            return;
        }
        if (keypressed === 17) {
            return false;
        }
        return false;
    }
}

// ACTIVE MENU
function ActiveMenu() {
    var sUrl = window.location.search;
    var sListUrl = "";
    if (sUrl.includes('&')) {
        sListUrl = sUrl.split('&');
        var sListUrlNew = sListUrl[1].split('=');
        return sListUrlNew[1];
    }
    else {
        sListUrl = sUrl.split('=');
        return sListUrl[1];
    }
}

//Formart date

function isDate($text) {
    //var date = /[0 - 9]{ 2 } \/[0-9]{2}\/[0 - 9]{ 4 }/;
    var date = /([0-9][1-2])\/([0 - 2][0 - 9] | [3][0 - 1]) \/ ((19 | 20)[0 - 9]{ 2})/;
    return date.test($text);
}

function GetTheFirstDayOfTheMonth(_idHtml) {
    var d = new Date();
    var _day = d.getDate();
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _firstDay = '01' + '/' + _month + '/' + _year;
    $("#" + _idHtml).val(_firstDay);
}

function GetCurentDate(_idHtml) {
    var d = new Date();
    var _day = d.getDate();
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _firstDay = _day + '/' + _month + '/' + _year;
    $("#" + _idHtml).val(_firstDay);
}

function GetTheLastDayOfTheMonth(_idHtml) {
    var d = new Date();
    var _day = 0;
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _hour = d.getHours();
    var _min = d.getMinutes();
    if (_year % 100 === 0 && _year % 400 === 0 && _year % 4 === 0) {
        if (_month === 2) {
            _day = 29;
        }
        else {
            if (_month === 4 || _month === 6 || _month === 9 || _month === 11) {
                _day = 30;
            }
            else {
                if (_month === 1 || _month === 3 || _month === 5 || _month === 7 || _month === 8 || _month === 10 || _month === 12) {
                    _day = 31;
                }
            }
        }
    }
    else {
        if (_month === 2) {
            _day = 28;
        }
        else {
            if (_month === 4 || _month === 6 || _month === 9 || _month === 11) {
                _day = 30;
            }
            else {
                if (_month === 1 || _month === 3 || _month === 5 || _month === 7 || _month === 8 || _month === 10 || _month === 12) {
                    _day = 31;
                }
            }
        }
    }
    var _lastDay = _day + '/' + _month + '/' + _year;
    $("#" + _idHtml).val(_lastDay);
}

function GetTheLastDayOfTheMonthHoursMin(_idHtml) {
    var d = new Date();
    var _day = 0;
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _hour = d.getHours();
    var _min = d.getMinutes();
    if (_year % 100 === 0 && _year % 400 === 0 && _year % 4 === 0) {
        if (_month === 2) {
            _day = 29;
        }
        else {
            if (_month === 4 || _month === 6 || _month === 9 || _month === 11) {
                _day = 30;
            }
            else {
                if (_month === 1 || _month === 3 || _month === 5 || _month === 7 || _month === 8 || _month === 10 || _month === 12) {
                    _day = 31;
                }
            }
        }
    }
    else {
        if (_month === 2) {
            _day = 28;
        }
        else {
            if (_month === 4 || _month === 6 || _month === 9 || _month === 11) {
                _day = 30;
            }
            else {
                if (_month === 1 || _month === 3 || _month === 5 || _month === 7 || _month === 8 || _month === 10 || _month === 12) {
                    _day = 31;
                }
            }
        }
    }
    var _lastDay = _day + '/' + _month + '/' + _year + " " + "23:59" ;
    $("#" + _idHtml).val(_lastDay);
}