//import { signalR } from "../../../lib/microsoft-signalr";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:8001/auctionhub").build();
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

connection.on("Bids", function (user,bid) {
    addBitToTable(user, bid);


});

function addBitToTable(user, bid) {
    var str = "<tr>";
    str += "<td>" + user + "</td>";
    str += "<td>" + bid + "</td>";
    str += "</tr>";
    if ($('table>tbody>tr:first').length>0) {
        $('table>tbody>tr:first').before(str);
    }
    else {
        $('.bidLine').append(str);
    }
}

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

    SendBid(sendBidRequest);
    event.preventDefault();
});

document.getElementById("finishButton").addEventListener("click", function (event) {
    var user = document.getElementById("SellerUserName").value;

    var sendCompleteBidRequest = {
        AuctionId: auctionId,
    }

    SendCompleteBid(sendCompleteBidRequest);
    event.preventDefault();
});




function SendBid(model) {
    $.ajax({

        url: "/Auction/SendBid",
        type: "POST",
        data: model,
        success: function (response) {
            if (response.isSuccess) {
                document.getElementById("exampleInputPrice").value = "";
                connection.invoke("SendBidAsync", groupName, model.SellerUserName, model.Price).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}


function SendCompleteBid(model) {
    var idNo = auctionId;
    $.ajax({

        url: "/Auction/CompleteBid",
        type: "POST",
        data: {id:idNo},
        success: function (response) {
            if (response.isSuccess) {
                console.log("Completed Successfully.")   
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}

