
var connection = new signalR.HubConnectionBuilder().withUrl("/notify").build();

connection.on("RecievedNotification", function (msg, counter) {

    var currentdate = new Date().getDate();
    $('span.badge').css("display", "inline");
    $("span.counterNum").html(counter)
    $("div#notification-list").prepend(`
                <div class="dropdown-divider"></div>
                <a href="#" class="dropdown-item">
                    <i class="fas fa-envelope mr-2"></i> `+ msg
                    + `<span class="moment-date float-right text-muted text-sm">` + currentdate + `</span>
                </a>
    `);
});

connection.start();

 