/*
Template Name: Veltrix - Responsive Bootstrap 4 Admin Dashboard
Author: Themesbrand
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Form wizard Js File
*/

$(function ()
{
    $("#form-horizontal").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slide"
    });
});

var _stt = 0;
var fileImages = true;
$(document).ready(function () {
    $("#fulProducts").change(function () {
        var File = this.files;
        if (File && File[0]) {
            fileImages = true;
        }
    });
});

function Plus_LeadSetting_HTML() {
    var htmlPlusLeadSetting = "";
    htmlPlusLeadSetting += "";
    htmlPlusLeadSetting += "<tr id='row_lead_item_" + _stt + "'>";
    htmlPlusLeadSetting += "    <td style='width:20%'>";
    htmlPlusLeadSetting += "        <input type='text' id='txtCol' class='claCol form-control' maxlength='1' />";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "    <td style='width:75%'>";
    htmlPlusLeadSetting += "        <select class='claField form-control'>";
    htmlPlusLeadSetting += "            <option value='NONE'>--- Select ---</option>";
    htmlPlusLeadSetting += "            <option value='FIRSTNAME'>Họ</option>";
    htmlPlusLeadSetting += "            <option value='LASTNAME'>Tên</option>";
    htmlPlusLeadSetting += "            <option value='CMND'>Chứng minh nhân dân</option>";
    htmlPlusLeadSetting += "            <option value='PHONE'>Điện thoại</option>";
    htmlPlusLeadSetting += "            <option value='PROVINCE'>Thành phố</option>";
    htmlPlusLeadSetting += "            <option value='COMPANY'>Tên công ty</option>";
    htmlPlusLeadSetting += "        </select>";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "    <td style='text-align:center;width:5%'>";
    htmlPlusLeadSetting += "        <a onclick = Delete_PlusLeadSetting_HTML('" + "row_lead_item_" + _stt + "')><i style='color:#bd081c;font-size: 20px;cursor: pointer;' class='mdi mdi-delete' aria-hidden='true'></i></a>";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "    <td style='display:none;'>";
    htmlPlusLeadSetting += "        <input type='text' class='claDefault form-control' value='false' />";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "</tr>";
    $("#bodyLeadSetting").append(htmlPlusLeadSetting);
    _stt++;
}

function Plus_LeadDefaul_HTML() {
    var htmlPlusLeadSetting = "";
    htmlPlusLeadSetting += "";
    htmlPlusLeadSetting += "<tr id='row_lead_item_" + _stt + "'>";
    htmlPlusLeadSetting += "    <td style='width:20%'>";
    htmlPlusLeadSetting += "        <select class='claField form-control'>";
    htmlPlusLeadSetting += "            <option value='NONE'>--- Select ---</option>";
    htmlPlusLeadSetting += "            <option value='FIRSTNAME'>Họ</option>";
    htmlPlusLeadSetting += "            <option value='LASTNAME'>Tên</option>";
    htmlPlusLeadSetting += "            <option value='CMND'>Chứng minh nhân dân</option>";
    htmlPlusLeadSetting += "            <option value='PHONE'>Điện thoại</option>";
    htmlPlusLeadSetting += "            <option value='PROVINCE'>Thành phố</option>";
    htmlPlusLeadSetting += "            <option value='COMPANY'>Tên công ty</option>";
    htmlPlusLeadSetting += "        </select>";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "    <td style='width:75%'>";
    htmlPlusLeadSetting += "        <input type='text' id='txtValue' class='claValue form-control' />";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "    <td style='text-align:center;width:5%'>";
    htmlPlusLeadSetting += "        <a onclick = Delete_PlusLeadDefault_HTML('" + "row_lead_item_" + _stt + "')><i style='color:#bd081c;font-size: 20px;cursor: pointer;' class='mdi mdi-delete' aria-hidden='true'></i></a>";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "    <td style='display:none;'>";
    htmlPlusLeadSetting += "        <input type='text' class='claDefault form-control' value='true' />";
    htmlPlusLeadSetting += "    </td>";
    htmlPlusLeadSetting += "</tr>";
    $("#bodyLeadDefault").append(htmlPlusLeadSetting);
    _stt++;
}


function Delete_PlusLeadSetting_HTML(_idHtml) {
    var r = confirm("Bạn có muốn xóa dòng này không?");
    if (r === true) {
        $("#" + _idHtml).remove();
    }
}

function Delete_PlusLeadDefault_HTML(_idHtml) {
    var r = confirm("Bạn có muốn xóa dòng này không?");
    if (r === true) {
        $("#" + _idHtml).remove();
    }
}

function PushLeads() {
    $("#loader").show();
    var productImagefile = "";
    if (fileImages === true)
        productImagefile = $("#fulProducts").get(0).files;
    else
        productImagefile = "";
    var dataObj = {
        LeadItems: GetListLeadItem()
    }
    PostAjaxAndFile("/Lead/ImportLeadsFromExcelJs", dataObj, productImagefile, reCallDoSaveProducts);
}

function reCallDoSaveProducts(result) {
    if (result.data.result === true) {
        $("#downloadResultImportLead").append("<a class='btn btn-primary waves-effect waves-light mr-1' style='color: white' href='" + result.data.url + "'>Download file seccuess</a>");
        $("#divInfoReult").show();
        $("#bInfo").html("Import thành công.");
        $("#bTotalSuccess").html(result.data.totalRowSuccess);
        $("#bTotalFail").html(result.data.totalRowFail);
    }
    else {
        $("#downloadResultImportLead").remove();
        $("#bInfo").html("Import thất bại, xin kiểm tra lại.");
        $("#bTotalSuccess").html(result.data.totalRowSuccess);
        $("#bTotalFail").html(result.data.totalRowFail);
    }
    $("#loader").hide();
}

function GetListLeadItem() {
    var arrList = [];
    $('#tableLead tbody tr').each(function () {
        var col = $(this).find(".claCol").val();
        var field = $(this).find(".claField").val();
        var value = $(this).find(".claValue").val();
        var isDefault = $(this).find(".claDefault").val();
        var arrayItem = { Col: col, Field: field, Value: value, IsDefault: isDefault };
        arrList.push(arrayItem);
    });
    return arrList;
}

function Download(urlExcel) {
    $.ajax({
        type: "POST",
        url: "/Lead/LinkDownloadImportLeadExcel",
        data: { urlExcel: urlExcel },
        dataType: "text",
        success: function (msg) {

        },
        error: function (req, status, error) {

        }
    });
}