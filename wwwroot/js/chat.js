"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const Chat = (function () {
    const myName = document.getElementById("userInput").value;

    function init() {
        $(document).on('keydown', 'div.input-div textarea', function (e) {
            // console.log(e.keyCode);
            if (e.keyCode === 13 && !e.shiftKey) {
                e.preventDefault();
                const message = $(this).val();


                sendMessage(message);

                clearTextarea();
            }
        });
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
        connection.invoke("SendMessage", myName, message).catch(function (err) {
            return console.error(err.toString());
        });

    }

    connection.on("ReceiveMessage", function (user, message) {
        const LR = (user != myName) ? "left" : "right";
        appendMessageTag(LR, user, message);

    });
    
    connection.start();

    function clearTextarea() {
        $('div.input-div textarea').val('');
    }
    
    return {
        'init': init
    };
})();

$(function () {
    Chat.init();
});
