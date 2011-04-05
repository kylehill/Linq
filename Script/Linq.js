var t = 0;

function login() {
    $("#loginText").text("logging in...");
    var email = $("#txtEmail").val();
    var pass = $("#txtPassword").val();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"email":"' + email + '", "password":"' + pass + '"}',
        dataType: "json",
        url: "Linq.asmx/Login",
        success: function (data) {
            if (data.d.length > 0) {
                $("body").data("token", data.d);
                $("body").data("email", email);
                $("#loginBox").css("display", "none");
                $("#chatBox").css("display", "block");
                chatInit();
            }
            else {
                $("#loginText").text("incorrect login/password.");
            }
        }
    });
}

function create() {
    $("#loginText").text("creating account...");
    var email = $("#txtEmail").val();
    var nickname = $("#txtEmail").val();
    var pass = $("#txtPassword").val();

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"email":"' + email + '", "nickname":"' + nickname + '", "password":"' + pass + '"}',
        dataType: "json",
        url: "Linq.asmx/CreateNewUser",
        success: function (data) {
            if (data.d == true) {
                login();
            }
            else {
                $("#loginText").text("an account with this email already exists.");
            }
        }
    });
}

function chatInit() {
    $("#loggedInAsText").html("Logged in as <b>" + $("body").data("email") + "</b>");
    var token = $("body").data("token");

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"token":"' + token + '"}',
        dataType: "json",
        url: "Linq.asmx/ChatInit",
        success: function (data) {
            for (var i = 0; i < data.d.length; i++) {
                var newdiv = document.createElement("div");
                $(newdiv).html("<b>" + data.d[i].voice + "</b>: " + data.d[i].message);
                $("#chatLines").append(newdiv);
                $("body").data("lastChatLine", data.d[i].lineId);

            }
            t = setInterval("refreshChat()", 500);
        }
    });
    
}

function refreshChat() {
    var token = $("body").data("token");
    var latestLine = $("body").data("lastChatLine");

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"token":"' + token + '", "latestLine":"' + latestLine + '"}',
        dataType: "json",
        url: "Linq.asmx/GetLatestChat",
        success: function (data) {
            for (var i = 0; i < data.d.length; i++) {
                var newdiv = document.createElement("div");
                $(newdiv).html("<b>" + data.d[i].voice + "</b>: " + data.d[i].message);
                $("#chatLines").append(newdiv);
                $("body").data("lastChatLine", data.d[i].lineId);
                var chatLines = document.getElementById("chatLines");
                chatLines.scrollTop = chatLines.scrollHeight;
            }
        }
    });
}

function enterChat() {
    var token = $("body").data("token");
    var email = $("body").data("email");
    var message = $("#txtChatSubmission").val();

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"token":"' + token + '", "email":"' + email + '", "message":"' + message + '"}',
        dataType: "json",
        url: "Linq.asmx/PostChat",
        success: function (data) {
            if (data.d == true) {
                $("#txtChatSubmission").val('');
            }
        }
    });    
}

function checkForEnter(evt) {
    if (evt.keyCode == 13) {
        enterChat();
    }
}