// "use strict";
//
// var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//
// //Disable send button until connection is established
// document.getElementById("sendButton").disabled = true;
//
// connection.on("ReceiveMessage", function (user, message) {
//     var li = document.createElement("li");
//     document.getElementById("messagesList").appendChild(li);
//     // We can assign user-supplied strings to an element's textContent because it
//     // is not interpreted as markup. If you're assigning in any other way, you 
//     // should be aware of possible script injection concerns.
//     li.textContent = `${user} says ${message}`;
// });
//
// connection.start().then(function () {
//     document.getElementById("sendButton").disabled = false;
// }).catch(function (err) {
//     return console.error(err.toString());
// });
//
// document.getElementById("sendButton").addEventListener("click", function (event) {
//     var user = document.getElementById("userInput").value;
//     var message = document.getElementById("messageInput").value;
//     connection.invoke("SendMessage", user, message).catch(function (err) {
//         return console.error(err.toString());
//     });
//     event.preventDefault();
// });

"use strict";

const Chat = (function(){
    const myName = document.getElementById("userInput").value;
    // init 함수
    function init() {
        $(document).on('keydown', 'div.input-div textarea', function(e){
            console.log(e.keyCode);
            if(e.keyCode === 13 && !e.shiftKey) {
                e.preventDefault();
                const message = $(this).val();


                sendMessage(message);

                clearTextarea();
            }
        });
    }

    function mykeydown() {
        if(window.event.keyCode===13) 
        {
            init();
        }
    }

    
    function createMessageTag(LR_className, senderName, message) {
        
        let chatLi = $('div.chat.format ul li').clone();

        
        chatLi.addClass(LR_className);
        chatLi.find('.sender span').text(senderName);
        chatLi.find('.message span').text(message);

        return chatLi;
    }

    
    function appendMessageTag(LR_className, senderName, message) {
        const chatLi = createMessageTag(LR_className, senderName, message);

        $('div.chat:not(.format) ul').append(chatLi);

      
        $('div.chat').scrollTop($('div.chat').prop('scrollHeight'));
    }


    function sendMessage(message) {
        // to be continue...
        const data = {
            "senderName"    : myName,
            "message"        : message
        };

        
        resive(data);
    }

    
    function clearTextarea() {
        $('div.input-div textarea').val('');
    }

    
    function resive(data) {
        const LR = (data.senderName != myName)? "left" : "right";
        appendMessageTag("right", data.senderName, data.message);
    }

    return {
        'init': init
    };
})();

$(function(){
    Chat.init();
});
