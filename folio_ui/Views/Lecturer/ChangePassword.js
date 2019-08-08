

$("#changeStatus").hide();

//$("#change-submit").click(() => {
//    $("#change.form").submit();
//});

$("#change-form").submit(function (event) {

    var submit = true;

    var modelpw = $("#modelPw").text();
    const oldpass = $("#old-Password").val();
    const newpass = $("#new-Password").val();
    const comfirmpass = $("#comfirm-Password").val();

    if (newpass != comfirmpass) {
        $("#changeStatus").show();
        $("#changeStatus").text("You have re-entered a different password");
        submit = false;
    }

    if (newpass == null) {
        $("#changeStatus").show();
        $("#changeStatus").text("New Password cannot be blank");
        submit = false;
    }

    if (oldpass == null) {
        $("#changeStatus").show();
        $("#changeStatus").text("Old Password cannot be blank");
        submit = false;
    }

    if (oldpass != modelpw) {
        $("#changeStatus").show();
        $("#changeStatus").text("Your old Password is incorrect" + oldpass+"///" + modelpw);
        submit = false;
    }

    if (newpass.length < 8) {
        $("#changeStatus").show();
        $("#changeStatus").text("New Password has to be more than 8 charactors");
        submit = false;
    }

    if (newpass.length > 18) {
        $("#changeStatus").show();
        $("#changeStatus").text("New Password has to be less than 18 charactors");
        submit = false;
    }

    if (submit == false) {
        event.preventDefault();
    }

})