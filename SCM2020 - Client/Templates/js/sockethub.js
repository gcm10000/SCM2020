"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5000/sockethub", { skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets  }).build();

connection.on("ReceiveMessage", function (window, message)
{
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = window + " says " + msg;
    var obj = JSON.parse(message);
    alert(encodedMsg);
    if (window === "ContentInventoryTurnover") {
        var content = document.getElementById("content");
        //Adicionar objeto
        var producthtml = document.getElementById(obj.Id);
        //Caso for inexistente...
        if (producthtml == null) {

            var button = "<div class=\"dropdown\">" +
                "<a type=\"text\" class=\"threedots\" id=\"dropdownMenuButton\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">" +
                //"<i class=\"fa fa-user-o\"></i>" +
                "</a>" +
                "<div class=\"dropdown-menu dropdown-menu-right\" aria-labelledby=\"dropdownMenuButton\">" +
                "<a class=\"dropdown-item\" href=\"javascript:void(0)\" onclick=\"Remove(" + obj.Id + ")\">Remover</a>" +
                "</div>" +
                "</div>";
            content.innerHTML += "<tr id=\"" + obj.Id + "\"><th>" + obj.Code + "</th><td>" + obj.Description + "</td><td>" + obj.Stock + "</td><td> " + obj.Unity + " </td><td>" + button + "</td></tr>";
        }
        //Caso já existir, remove o objeto
        else {
            content.removeChild(producthtml);
        }
    }
    else if (window === "ContentListPermanentProduct") {
        var content = document.getElementById("content");
        //Adicionar objeto
        var producthtml = document.getElementById(obj.Id);
        content.innerHTML += "<tr id=\"" + obj.Id + "\"><th>" + obj.Code + "</th><td>" + obj.Description + "</td><td>" + obj.Patrimony + "</td><td> " + obj.Group + " </td></tr>";


    }
});
connection.start().then(function () {
    console.log("ready");
});

function SendMessage(window, message)
{
    connection.invoke("SendMessage", window, message).catch(function (err) {
        return console.error(err.toString());
    });

}

function Remove(Id) {
    var content = document.getElementById("content");
    var producthtml = document.getElementById(Id);
    content.removeChild(producthtml);

}