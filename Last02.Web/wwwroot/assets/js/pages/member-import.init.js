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

function Plus_MemberSetting_HTML() {
    var htmlPlusMemberSetting = "";
    htmlPlusMemberSetting += "";
    htmlPlusMemberSetting += "<tr id='row_member_item_" + _stt + "'>";
    htmlPlusMemberSetting += "    <td style='width:20%'>";
    htmlPlusMemberSetting += "        <input type='text' id='txtCol' class='claCol form-control' maxlength='2' />";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "    <td style='width:75%'>";
    htmlPlusMemberSetting += "        <select class='claField form-control'>";
    htmlPlusMemberSetting += "            <option value='NONE'>--- Select ---</option>";
    htmlPlusMemberSetting += "            <option value='MEMBERCODE'>Mã</option>";
    htmlPlusMemberSetting += "            <option value='FIRSTNAME'>Họ</option>";
    htmlPlusMemberSetting += "            <option value='LASTNAME'>Tên</option>";
    htmlPlusMemberSetting += "            <option value='GENDER'>Giới Tính</option>";
    htmlPlusMemberSetting += "            <option value='YOB'>Năm Sinh</option>";
    htmlPlusMemberSetting += "            <option value='DOB'>Ngày Tháng Năm</option>";
    htmlPlusMemberSetting += "            <option value='PHONE'>Điện Thoại</option>";
    htmlPlusMemberSetting += "            <option value='EMAIL'>Email</option>";
    htmlPlusMemberSetting += "            <option value='ADDRESS1'>Địa Chỉ </option>";
    htmlPlusMemberSetting += "            <option value='MARITAL_STATUS'>Hôn Nhân</option>";
    htmlPlusMemberSetting += "            <option value='REGISTERCHANNEL'>Kênh</option>";
    htmlPlusMemberSetting += "            <option value='REGISTERSTORE'>Cửa Hàng</option>";
    htmlPlusMemberSetting += "            <option value='JOBTITLE'>Chức danh</option>";
    htmlPlusMemberSetting += "            <option value='PROVINCE'>Tỉnh/Thành Phố</option>";
    htmlPlusMemberSetting += "            <option value='DISTRICT'>Quận/Huyện</option>";
    htmlPlusMemberSetting += "        </select>";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "    <td style='text-align:center;width:5%'>";
    htmlPlusMemberSetting += "        <a onclick = Delete_PlusMemberSetting_HTML('" + "row_member_item_" + _stt + "')><i style='color:#bd081c;font-size: 20px;cursor: pointer;' class='mdi mdi-delete' aria-hidden='true'></i></a>";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "    <td style='display:none;'>";
    htmlPlusMemberSetting += "        <input type='text' class='claDefault form-control' value='false' />";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "</tr>";
    console.log(htmlPlusMemberSetting);
    $("#bodyMemberSetting").append(htmlPlusMemberSetting);
    _stt++;
}

function Plus_MemberDefaul_HTML() {
    var htmlPlusMemberSetting = "";
    htmlPlusMemberSetting += "";
    htmlPlusMemberSetting += "<tr id='row_member_item_" + _stt + "'>";
    htmlPlusMemberSetting += "    <td style='width:20%'>";
    htmlPlusMemberSetting += "        <select class='claField form-control'>";
    htmlPlusMemberSetting += "            <option value='NONE'>--- Select ---</option>";
    htmlPlusMemberSetting += "            <option value='MEMBERCODE'>Mã</option>";
    htmlPlusMemberSetting += "            <option value='FIRSTNAME'>Họ</option>";
    htmlPlusMemberSetting += "            <option value='LASTNAME'>Tên</option>";
    htmlPlusMemberSetting += "            <option value='GENDER'>Giới Tính</option>";
    htmlPlusMemberSetting += "            <option value='YOB'>Năm Sinh</option>";
    htmlPlusMemberSetting += "            <option value='DOB'>Ngày Tháng Năm</option>";
    htmlPlusMemberSetting += "            <option value='PHONE'>Điện Thoại</option>";
    htmlPlusMemberSetting += "            <option value='EMAIL'>Email</option>";
    htmlPlusMemberSetting += "            <option value='ADDRESS1'>Địa Chỉ </option>";
    htmlPlusMemberSetting += "            <option value='MARITAL_STATUS'>Hôn Nhân</option>";
    htmlPlusMemberSetting += "            <option value='REGISTERCHANNEL'>Kênh</option>";
    htmlPlusMemberSetting += "            <option value='REGISTERSTORE'>Cửa Hàng</option>";
    htmlPlusMemberSetting += "            <option value='JOBTITLE'>Chức Danh</option>";
    htmlPlusMemberSetting += "            <option value='PROVINCE'>Tỉnh/Thành Phố</option>";
    htmlPlusMemberSetting += "            <option value='DISTRICT'>Quận/Huyện</option>";
    htmlPlusMemberSetting += "        </select>";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "    <td style='width:75%'>";
    htmlPlusMemberSetting += "        <input type='text' id='txtValue' class='claValue form-control' />";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "    <td style='text-align:center;width:5%'>";
    htmlPlusMemberSetting += "        <a onclick = Delete_PlusMemberDefault_HTML('" + "row_member_item_" + _stt + "')><i style='color:#bd081c;font-size: 20px;cursor: pointer;' class='mdi mdi-delete' aria-hidden='true'></i></a>";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "    <td style='display:none;'>";
    htmlPlusMemberSetting += "        <input type='text' class='claDefault form-control' value='true' />";
    htmlPlusMemberSetting += "    </td>";
    htmlPlusMemberSetting += "</tr>";
    $("#bodyMemberDefault").append(htmlPlusMemberSetting);
    _stt++;
}

function Delete_PlusMemberSetting_HTML(_idHtml) {
    var r = confirm("Bạn có muốn xóa dòng này không?");
    if (r === true) {
        $("#" + _idHtml).remove();
    }
}

function Delete_PlusMemberDefault_HTML(_idHtml) {
    var r = confirm("Bạn có muốn xóa dòng này không?");
    if (r === true) {
        $("#" + _idHtml).remove();
    }
}

function PushMembers() {
    $("#loader").show();
    var uploadingfile = "";
    if (fileUploads === true)
        uploadingfile = $("#fileCSVUpload").get(0).files;
    else
        uploadingfile = "";

    var dataObj = {
        MemberColumnMap: GetListMemberItem()
    }

    PostAjaxAndFile("/Member/ImportMembersFromExcelJs", dataObj, uploadingfile, reCallDoSaveMembers);
}

function reCallDoSaveMembers(result) {
 
    var blob = new Blob([result], { type: "application/vnd.ms-excel" });
    alert('Download?: ' + result.length);
    var URL = window.URL || window.webkitURL;
    var downloadUrl = URL.createObjectURL(blob);
    document.location = downloadUrl;
 /*   if (result.data.result === true) {*/
        //alert("Success Import: " + result.data.Success + "/" + result.data.TotalRow);
        //$("#downloadResultImportMember").append("<a class='btn btn-primary waves-effect waves-light mr-1' style='color: white' href='" + result.data.url + "'>Download file seccuess</a>");
        //$("#divInfoReult").show();
        //$("#bInfo").html("Import thành công.");
        //$("#bTotalSuccess").html(result.data.totalRowSuccess);
        //$("#bTotalFail").html(result.data.totalRowFail);
      
    //}
    //else {
    //    $("#downloadResultImportMember").remove();
    //    $("#bInfo").html("Import thất bại, xin kiểm tra lại.");
    //    $("#bTotalSuccess").html(result.data.totalRowSuccess);
    //    $("#bTotalFail").html(result.data.totalRowFail);
   
    //}
    $("#loader").hide(); 
 
}

function GetListMemberItem() {
    var arrList = [];
    $('#tableMember tbody tr').each(function () {
        var col = $(this).find(".claCol").val();
        var field = $(this).find(".claField").val();
        var value = $(this).find(".claValue").val();
        var isDefault = $(this).find(".claDefault").val();
        var arrayItem = { Col: col, Field: field, Value: value, IsDefault: isDefault };
        arrList.push(arrayItem);
    });
    return arrList;
}