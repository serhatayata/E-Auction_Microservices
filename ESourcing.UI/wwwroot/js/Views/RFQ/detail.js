//import { signalR } from "../../../lib/microsoft-signalr";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:41282/auctionhub").build();
var auctionId = document.getElementById("AuctionId").value;
//Disable sendButton button until connection is established.
document.getElementById("sendButton").disabled = true;

var groupName = "auction-" + auctionId;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    connection.invoke("AddToGroup", groupName).catch(function (err) {
        return console.error(err.toString());

    });
}).catch(function (err) {
    return console.error(err.toString());
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("SellerUserName").value;
    var productId = document.getElementById("ProductId").value;
    var sellerUser = user;
    var bid = document.getElementById("exampleInputPrice").value;

    var sendBidRequest = {
        AuctionId: auctionId,
        ProductId: productId,
        SellerUserName: sellerUser,
        Price: parseFloat(bid).toString()
    }

});

SendBid(sendBidRequest);
event.preventDefault();

function SendBid(model) {
    
}