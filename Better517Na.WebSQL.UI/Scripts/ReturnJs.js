//还书脚本
$(document).ready(function () {
    //触发focus事件时
    $("#orguid").focus(function () {
        $("#returnbooklab").hide();
        $("#retouttimebook").hide();//隐藏超期还书按钮  
        $("#returnbook").show();//显示正常还书按钮
    });
    //触发blur事件时
    $("#orguid").blur(function () {
        if (testnull("orguid")) {
            // 检测输入框是否为空
            $("#returnbooklab").show();
        }
        else {
            GetReturnBookInfo();
            $("#returnbooklab").hide();
        }
    });
})

// 获取还书信息
function GetReturnBookInfo() {
    $.ajax({
        url: "/Home/AjaxCheckReturnBook",
        data: {
            bookguid:$.trim( $("#orguid").val()),
           
        },
        type: "post",
        success: function (data) {
            checkdata(data);
        },
        error: function (e) {
        }
    });
}
// 输入为空返回true
function testnull(textid) {
    if (document.getElementById(textid).value == '') {
        return true;
    }
    else {
        return false;
    }
}
// 解析返回的数据
function checkdata(data) {
    var arrdata = new Array();
    arrdata = data.split("#");//按照顿号分割
    var i = 0;
    for (var n in arrdata) {
        i++;
    }
    switch (i) {
        case 3:
            if (arrdata[0] != null) {
                // 借书人
                $("#labborrowman").html(arrdata[0]);
            }
            if (arrdata[1] != null) {
                // 书名
                $("#labbookname").html(arrdata[1]);
            }
            if (arrdata[2] != null) {
                if ($("#retouttimebook").is(":hidden") ? true : false) {
                    $("#retouttimebook").show();//显示div  
                    $("#returnbook").hide();//隐藏正常还书按钮
                    $("#retlabmessage").html(arrdata[2]);
                }
            }
            break;
        case 2:
            if (arrdata[0] != null) {
                // 借书人
                $("#labborrowman").html(arrdata[0]);
            }
            if (arrdata[1] != null) {
                // 书名
                $("#labbookname").html(arrdata[1]);
            }
            $("#butreturnbook").attr("disabled", false);
            break;
        default:
            alert(data);
            break;
    }
}

// 还书
function ReturnBook(flag)
{
    $.ajax({
        url: "/Home/AjaxReturnBook",
        data: {
            Returnflag: flag,//正常归还/还书不交钱/还书并付款
            bookid:$.trim($("#orguid").val()),
               //$("#orguid").val().trim(),
        },
        type: "post",
        success: function (data) {
            if (data == "") {
                alert("还书成功！");
            }
            else {
                alert(data);
            }
            $("#retouttimebook").attr("disabled", true);
            $("#orguid").attr("value", "");//清空内容
            $("#labborrowman").attr("value", "");//清空内容
            $("#labbookname").attr("value", "");//清空内容
        },
        error: function (e) {
        }
    });
}