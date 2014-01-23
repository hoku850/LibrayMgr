$(document).ready(function () {
    //触发focus事件时
    $("#bookid").focus(function () {
        $("#labbookid").hide();
    });
    //触发blur事件时
    $("#bookid").blur(function () {
        if (testnull("bookid")) {
            // 检测输入框是否为空
            $("#labbookid").show();
        }
        else {
            getbookname();
            $("#labbookid").hide();
        }
    });

    //触发focus事件时
    $("#borrowman").focus(function () {
        $("#labborrowman").hide();
    });

    // 检测借阅人输入框
    $("#borrowman").blur(function () {
        if (!$("#erromessage").is(":hidden")) {
            $("#erromessage").hide();//隐藏div  
            $("#borrowbut").show();//显示正常借阅界面
        }
        if (testnull("borrowman")) {
            // 检测输入框是否为空
            $("#labborrowman").show();
        }
        else if (!checkname("labborrowman")) {
            alert("借阅人输入格式不正确，只能为字母，数字，下划线");
        }
        else {

            CheckBorrowName();
            $("#labborrowman").hide();
        }
    });

    // 借阅按钮事件
    $("#BorrowbtnOK").click(function () {
        $.ajax({
            url: "/Home/AjaxBorrowBook",
            data: {
                IsPaied: "0",
                bookid:$.trim( $("#bookid").val()),
                name: $.trim( $("#borrowman").val()),
            },
            type: "post",
            success: function (data) {
                if (data == "") {
                    alert("借阅成功！");
                }
                else {
                    alert(data);
                }
                $("#borrowman").attr("value", "");//清空内容
                $("#bookid").attr("value", "");//清空内容
                $("#BorrowbtnOK").attr("disabled", true);
            },
            error: function (e) {
            }
        });
    });

    $("#borbookmaney").click(function () {
        $.ajax({
            url: "/Home/AjaxBorrowBook",
            data: {
                IsPaied:"1",
                bookid:$.trim( $("#bookid").val()),
                name: $.trim( $("#borrowman").val()),
            },
            type: "post",
            success: function (data) {
                if (data == "") {
                   
                    alert("借阅成功！");
                    $("#erromessage").hide();
                }
                else {
                    alert(data);
                }
                $("#borrowman").attr("value", "");//清空内容
                $("#bookid").attr("value", "");//清空内容
                $("#borbookmaney").attr("disabled", true);
            },
            error: function (e) {
            }
        });
    });

})



// 输入为空返回true
function testnull(textid) {
    if (document.getElementById(textid).value == '') {
        return true;
    }
    else {
        return false;
    }
}
/**
* 检查输入的用户名是否正确
* 输入:str  字符串
*  返回:true 或 flase; true表示格式正确
*/
function checkname(textid) {
    if ($("#borrowman").val().match(/^[A-Z0-9a-z_]{1,}$/) == null) {
        return false
    }
    else {
        return true;
    }
}
// 获取图书名字
function getbookname() {
    $.ajax({
        url: "/Home/GetBookName",
        data: {
            bookid:$.trim( $("#bookid").val()),
        },
        type: "post",
        success: function (data) {
            if (data == "") {
                alert("没有该图书或者条形码错误");
                $("#borrowman").attr("disabled", true)
            }
            else {
                $("#borrowman").attr("disabled", false)
                $("#labbookname").html(data);
            }
        },
        error: function (e) {
        }
    });
}
// 检测借书人借书信息
function CheckBorrowName() {
    $.ajax({
        url: "/Home/CheckBorrowName",
        data: {
            name:$.trim( $("#borrowman").val()),
               
        },
        type: "post",
        success: function (data) {
            if (data == "") {
                $("#BorrowbtnOK").attr("disabled", false)
            }
            else {
                checkdata(data);
            }
        },
        error: function (e) {
        }
    });
}
// 解析返回的数据
function checkdata(data) {
    var arrdata = data.split("#");//按照顿号分割
    switch (arrdata[1]) {
        case "0":
            alert(arrdata[0]);
            break;
        case "1":
            if ($("#erromessage").is(":hidden") ? true : false) {
                $("#erromessage").show();//显示div  
                $("#borrowbut").hide();//隐藏正常借阅按钮
                $("#borrowmessage").html(arrdata[0]);
            }
            break;
    }
}
// 根据书名模糊查询图书信息
function QueryBook() {
    $.ajax({
        url: "/Home/LibrayMgr",
        data: {
            bookname:$.trim( $("#chaStaffName").val()),
        },
        type: "post",
        success: function (data) {
            $("#querylist").html(data);
        },
        error: function (e) {
        }
    });
}
