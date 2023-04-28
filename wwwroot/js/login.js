
function checkForm(form) {
    var checkUsername = document.getElementById("checkUsername");
    var checkPassword = document.getElementById("checkPassword");
    var msg = document.getElementById("msg");

    checkUsername.innerHTML = "";

    if (msg != null && msg != "") {
        msg.innerHTML = "";
    }

    if (form.username.value == "") {
        checkUsername.innerHTML = "Username cannot be null."
        form.username.focus();
        return false;
    }

    if (form.password.value == "") {
        checkPassword.innerHTML = "Password cannot be null."
        form.password.focus();
        return false;
    }
    return true;
}

window.onload = function () {
    //forms[0] means the first form in the page which will run this function.
    var myform = document.forms[0];
    var username = document.getElementById("usernameId");
    var password = document.getElementById("passwordId");

    myform.onsubmit = function () {
        return checkForm(this);
    }

    username.onblur = function () {
        return checkForm(myform);
    }

    password.onblur = function () {
        return checkForm(myform);
    }



}
