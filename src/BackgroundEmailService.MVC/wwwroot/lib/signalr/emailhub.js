const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5001/hubs/emailHub")
    .build();

connection.on("emailObserver", (sender, message) => {
    const li = document.createElement("li");

    const model = JSON.parse(message);
    const email = model.EmailAddress;
    const element = document.getElementById(email);

    if (element != null)
    {
        var classnameclear = element.className        

        switch (model.State) {
            case 1:
                element.className = element.className.replace(classnameclear, "fa fa-spinner fa-spin");
                element.style.color = "black";
                break;
            case 2:
                element.className = element.className.replace(classnameclear, "fa fa-check");
                element.style.color = "green";
                break;
            case 6:
                element.className = element.className.replace(classnameclear, "fa fa-exclamation");
                element.style.color = "red";
                break;
            default:
        }

        
    }
    else
    {
        li.textContent = model.EmailAddress;
        document.getElementById("messageList").appendChild(li);
    }

});

connection.start().catch(err => console.error(err.toString()));