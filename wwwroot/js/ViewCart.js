window.onload = function () {
    let elem = document.getElementById("checkoutBtn");

    elem.addEventListener("click", function () {
        alert("ready to check out?")

        //send the order cart to server
        sendCartOrder(data);
    })
}