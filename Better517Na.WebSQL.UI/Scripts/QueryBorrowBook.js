
function QueryBorrowBook()
{
   
    $.ajax({
        url: "/Home/AjaxBorrowBookQuery",
        data: {
            flag: $("#isdispbook").attr("checked")? 1:0,
            username:$.trim( $("#borroname").val()),
               
        },
        type: "post",
        success: function (data) {
            if (data != "")
            {
                $("#borrowquerylist").html(data);
            }
        },
        error: function (e) {
        }
    });


}