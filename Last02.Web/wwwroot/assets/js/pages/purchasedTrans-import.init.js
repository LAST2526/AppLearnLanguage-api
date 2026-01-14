/*
Template Name: Veltrix - Responsive Bootstrap 4 Admin Dashboard
Author: Themesbrand
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Form wizard Js File
*/

var _stt = 0;
var fileUploads = true;
$(document).ready(function () {
    $("#fileCSVUpload").change(function () {
        var File = this.files;
        if (File && File[0]) {
            fileUploads = true;
        }
    });
});

function Plus_PurchasedTransSetting_HTML() {
    var htmlPlusPurchasedTransSetting = "";
    htmlPlusPurchasedTransSetting += "";
    htmlPlusPurchasedTransSetting += "<tr id='row_purchasedTrans_item_" + _stt + "'>";
    htmlPlusPurchasedTransSetting += "    <td style='width:20%'>";
    htmlPlusPurchasedTransSetting += "        <input type='text' id='txtCol' class='claCol form-control' maxlength='1' />";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "    <td style='width:75%'>";
    htmlPlusPurchasedTransSetting += "        <select class='claField form-control'>";
    htmlPlusPurchasedTransSetting += "            <option value='NONE'>--- Select ---</option>";
    htmlPlusPurchasedTransSetting += "            <option value='INVOICE_NO'>Mã</option>";
    htmlPlusPurchasedTransSetting += "            <option value='REF_ID'>id giới thiệu</option>";
    htmlPlusPurchasedTransSetting += "            <option value='AMOUNT'>Số Lượng</option>";
    htmlPlusPurchasedTransSetting += "            <option value='PURCHASED_DATE'>Ngày Mua</option>";
    htmlPlusPurchasedTransSetting += "            <option value='CURRENCY'>Tiền Tệ</option>";
    htmlPlusPurchasedTransSetting += "            <option value='MEMBER_REFERENCE'>Thành Viên Tham Khảo</option>";
    htmlPlusPurchasedTransSetting += "            <option value='REMARK'>Lưu ý</option>";
    htmlPlusPurchasedTransSetting += "        </select>";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "    <td style='text-align:center;width:5%'>";
    htmlPlusPurchasedTransSetting += "        <a onclick = Delete_PlusPurchasedTransSetting_HTML('" + "row_purchasedTrans_item_" + _stt + "')><i style='color:#bd081c;font-size: 20px;cursor: pointer;' class='mdi mdi-delete' aria-hidden='true'></i></a>";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "    <td style='display:none;'>";
    htmlPlusPurchasedTransSetting += "        <input type='text' class='claDefault form-control' value='false' />";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "</tr>";
    console.log(htmlPlusPurchasedTransSetting);
    $("#bodyPurchasedTransSetting").append(htmlPlusPurchasedTransSetting);
    _stt++;
}

function Plus_PurchasedTransDefaul_HTML() {
    var htmlPlusPurchasedTransSetting = "";
    htmlPlusPurchasedTransSetting += "";
    htmlPlusPurchasedTransSetting += "<tr id='row_purchasedTrans_item_" + _stt + "'>";
    htmlPlusPurchasedTransSetting += "    <td style='width:20%'>";
    htmlPlusPurchasedTransSetting += "        <select class='claField form-control'>";
    htmlPlusPurchasedTransSetting += "            <option value='NONE'>--- Select ---</option>";
    htmlPlusPurchasedTransSetting += "            <option value='INVOICE_NO'>Mã</option>";
    htmlPlusPurchasedTransSetting += "            <option value='REF_ID'>id giới thiệu</option>";
    htmlPlusPurchasedTransSetting += "            <option value='AMOUNT'>Số Lượng</option>";
    htmlPlusPurchasedTransSetting += "            <option value='PURCHASED_DATE'>Ngày Mua</option>";
    htmlPlusPurchasedTransSetting += "            <option value='CURRENCY'>Tiền Tệ</option>";
    htmlPlusPurchasedTransSetting += "            <option value='MEMBER_REFERENCE'>Thành Viên Tham Khảo</option>";
    htmlPlusPurchasedTransSetting += "            <option value='REMARK'>Lưu ý</option>";
    htmlPlusPurchasedTransSetting += "        </select>";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "    <td style='width:75%'>";
    htmlPlusPurchasedTransSetting += "        <input type='text' id='txtValue' class='claValue form-control' />";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "    <td style='text-align:center;width:5%'>";
    htmlPlusPurchasedTransSetting += "        <a onclick = Delete_PlusPurchasedTransDefault_HTML('" + "row_purchasedTrans_item_" + _stt + "')><i style='color:#bd081c;font-size: 20px;cursor: pointer;' class='mdi mdi-delete' aria-hidden='true'></i></a>";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "    <td style='display:none;'>";
    htmlPlusPurchasedTransSetting += "        <input type='text' class='claDefault form-control' value='true' />";
    htmlPlusPurchasedTransSetting += "    </td>";
    htmlPlusPurchasedTransSetting += "</tr>";
    $("#bodyPurchasedTransDefault").append(htmlPlusPurchasedTransSetting);
    _stt++;
}

function Delete_PlusPurchasedTransSetting_HTML(_idHtml) {
    var r = confirm("Bạn có muốn xóa dòng này không?");
    if (r === true) {
        $("#" + _idHtml).remove();
    }
}

function Delete_PlusPurchasedTransDefault_HTML(_idHtml) {
    var r = confirm("Bạn có muốn xóa dòng này không?");
    if (r === true) {
        $("#" + _idHtml).remove();
    }
}

function PushPurchasedTrans() {
    $("#loader").show();
    var uploadingfile = "";
    if (fileUploads === true)
        uploadingfile = $("#fileCSVUpload").get(0).files;
    else
        uploadingfile = "";

    var dataObj = {
        PurchasedTransColumnMap: GetListPurchasedTransItem()
    }

    PostAjaxAndFile("/Store/ImportPurchasedTransFromExcelJs", dataObj, uploadingfile, reCallDoSaveProducts);
}

function reCallDoSaveProducts(result) {
    console.log(result.data);
    if (result.data.result === true) {
        alert("Success Import: " + result.data.Success + "/" + result.data.TotalRow);
        $("#downloadResultImportPurchasedTrans").append("<a class='btn btn-primary waves-effect waves-light mr-1' style='color: white' href='" + result.data.url + "'>Download file seccuess</a>");
        $("#divInfoReult").show();
        $("#bInfo").html("Import thành công.");
        $("#bTotalSuccess").html(result.data.totalRowSuccess);
        $("#bTotalFail").html(result.data.totalRowFail);
    }
    else {
        $("#downloadResultImportPurchasedTrans").remove();
        $("#bInfo").html("Import thất bại, xin kiểm tra lại.");
        $("#bTotalSuccess").html(result.data.totalRowSuccess);
        $("#bTotalFail").html(result.data.totalRowFail);
    }
    $("#loader").hide();
}

function GetListPurchasedTransItem() {
    var arrList = [];
    $('#tablePurchasedTrans tbody tr').each(function () {
        var col = $(this).find(".claCol").val();
        var field = $(this).find(".claField").val();
        var value = $(this).find(".claValue").val();
        var isDefault = $(this).find(".claDefault").val();
        var arrayItem = { Col: col, Field: field, Value: value, IsDefault: isDefault };
        arrList.push(arrayItem);
    });
    return arrList;
}