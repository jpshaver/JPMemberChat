<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="JPShaver.Modules.JPMemberChat.View" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Import Namespace="DotNetNuke.Entities.Users" %>



<input id="btnStatus" type="button" value="Status" class="dnnSecondaryAction" />
<div id="spanOnline"><%=LocalizeString("ChatAvailable")%></div>
<table id="tblMembers">
    <tbody>
    </tbody>
</table>



<div id="dialog-status" title="Status" style="display: none">
    <br />
    <asp:RadioButtonList ID="rblStatus" runat="server" onchange="javascript:changeStatus();"></asp:RadioButtonList>
</div>


<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/JPMemberChat/Scripts/jquery.signalR-2.0.3.min.js" Priority="10" />
<dnn:DnnJsInclude runat="server" FilePath="~/signalr/hubs" Priority="100" />

<script type="text/javascript">

    $('#btnStatus').hide();

    // Get the window dimensions
    var $window = $(window);
    var center_x = $window.width() / 2;
    var center_y = $window.height() / 2;

    var dialog_x = 0;
    var dialog_y = 0;

    var timer;
    var pulsateInterval = null;

    $('<audio id="chatAudio"><source src="../DesktopModules/JPMemberChat/Audio/alert.ogg" type="audio/ogg"><source src="../DesktopModules/JPMemberChat/Audio/alert.mp3" type="audio/mpeg"><source src="../DesktopModules/JPMemberChat/Audio/alert.wav" type="audio/wav"></audio>').appendTo('body');

    var wfMemberChatHub = $.connection.JPMemberChatHub;

    wfMemberChatHub.state.userid = '<%= UserId %>';
    wfMemberChatHub.state.username = '<%= UserController.GetCurrentUserInfo().Username %>';
    wfMemberChatHub.state.firstname = '<%= UserController.GetCurrentUserInfo().FirstName %>';
    wfMemberChatHub.state.displayname = '<%= UserController.GetCurrentUserInfo().DisplayName %>';

    // Start the connection.
    $.connection.hub.start().done(function () {

        if ($.connection.hub.state === $.connection.connectionState.connected) {
            wfMemberChatHub.server.addConnection();
        }

        if ($.connection.hub.state === $.connection.connectionState.reconnecting) {
            wfMemberChatHub.state.userid = '<%= UserId %>';
            wfMemberChatHub.state.username = '<%= UserController.GetCurrentUserInfo().Username %>';
            wfMemberChatHub.state.firstname = '<%= UserController.GetCurrentUserInfo().FirstName %>';
            wfMemberChatHub.state.displayname = '<%= UserController.GetCurrentUserInfo().DisplayName %>';
        }

    });

    wfMemberChatHub.client.logToConsole = function (logtext) {
        //console.log(logtext);
    };

    wfMemberChatHub.client.printUsersToList = function (connid, uid, uname, upic, ustatusid) {
        if (ustatusid == 1) {
            setUserList(connid, uid, uname, upic);
        } else if (ustatusid == 3) {
            setUserList(connid, uid, uname, upic);
            $('#tblMembers tbody:first #' + uid).hide();
            $('#tblMembers tbody:first #busy' + uid).show();
        }
    };

    wfMemberChatHub.client.addUserToList = function (connid, uid, uname, upic) {
        if ($('#tblMembers tbody:first #' + uid).length) {
            if ($('#tblMembers tbody:first #' + uid).is(":hidden")) {
                $('#tblMembers tbody:first #' + uid).show();
                $('#tblMembers tbody:first #busy' + uid).hide();
            }
        } else {
            setUserList(connid, uid, uname, upic);
        }
    };

    wfMemberChatHub.client.deleteUserFromList = function (connid) {
        $('#tblMembers #' + connid).remove();

        if ($('#tblMembers').closest("tbody").find("tr").length) {
            $('#spanOnline').hide();                        
        } else {
            $('#spanOnline').show();
        }

    };

    wfMemberChatHub.client.setUserBusyStatus = function (uid) {
        if ($('#tblMembers tbody:first #' + uid).length) {
            $('#tblMembers tbody:first #' + uid).hide();
            $('#tblMembers tbody:first #busy' + uid).show();
        }
    };

    wfMemberChatHub.client.setStatusBtnText = function (type) {
        $('#btnStatus').attr('value', "My Status: " + type);

        var radioButtons = document.getElementById('<%= rblStatus.ClientID %>');
        var radio = radioButtons.getElementsByTagName("input");
        var label = radioButtons.getElementsByTagName("label");

        for (var x = 0; x < radio.length; x++) {
            if (label[x].innerHTML == type) {
                radio[x].checked = true;
            }
        }
    };

    wfMemberChatHub.client.setChat = function (dialogid, dialogTitle, mOneId, mTwoId) {

        createChatDialog(true, dialogid, dialogTitle, mOneId, mTwoId);
    };

    wfMemberChatHub.client.broadcastHistory = function (dialogid, uid, upic, uname, message) {
        displayMessage(dialogid, uid, upic, uname, message);
    }

    wfMemberChatHub.client.broadcastMessage = function (connid, dialogid, uid, upic, uname, message) {

        if (connid != $.connection.hub.id) {

            if ($('#tblMembers tbody:first #' + uid).length) {

                if ($('#tblMembers tbody:first #' + uid).is(":hidden")) {

                    $('#tblMembers tbody:first #' + uid).show();
                    $('#tblMembers tbody:first #busy' + uid).hide();

                } else {
                    //console.log('not hidden');
                }

            } else {
                setUserList(connid, uid, uname, upic);
            }
        }

        displayMessage(dialogid, uid, upic, uname, message);

    };

    wfMemberChatHub.client.broadcastKeypress = function (name, dialogid) {

        var t = $('#' + dialogid).parent().find('#dialog-span-typing');

        if (!$.trim(t.html()).length) {
            t.append('( Typing )');
        }

        clearTimeout(timer);
        t.stop().fadeIn(20);

        timer = setTimeout(function () {
            t.fadeOut(500);
        }, 1000);
    };

    wfMemberChatHub.client.setBlockText = function (b, uid) {

        if (b) {
            $('#tblMembers tbody:first #block' + uid).hide();
            $('#tblMembers tbody:first #unblock' + uid).show();
        }
        else {
            $('#tblMembers tbody:first #block' + uid).show();
            $('#tblMembers tbody:first #unblock' + uid).hide();
        }

    };

    function startChat(dialogTitle, mOneId, mTwoId) {

        if (pulsateInterval != null) {
            clearInterval(pulsateInterval);
        }

        wfMemberChatHub.server.checkChatExists(mOneId, mTwoId).done(function (result) {

            var chatstarted;
            var dialogid;

            if (result == "empty") {
                chatstarted = false;
                dialogid = Math.floor((Math.random() * 1000000000) + 1).toString();
            }
            else {
                chatstarted = true;
                dialogid = result;
            }

            createChatDialog(chatstarted, dialogid, dialogTitle, mOneId, mTwoId);

        });
    };

    function getChatHistory(dialogid, mOneId, mTwoId) {

        wfMemberChatHub.server.chatHistory(dialogid, mOneId, mTwoId);
    };

    function displayMessage(dialogid, uid, upic, uname, message) {

        if ($('#' + dialogid).length == 0 || $('#' + dialogid).dialog("isOpen") == false) {

                pulsateInterval = setInterval(function () {

                    $('#tblMembers tbody:first #' + uid).animate({
                        color: '<%= alertTextColor1 %>'
                    }, 200).animate({
                        color: '<%= alertTextColor2 %>'
                    }, 200);

                }, 500);

                $('#chatAudio')[0].play();
        }

        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text(message).html();

        $('#' + dialogid + ' #discussion').append('<li><img id="dialog-img" src="' + upic + '" alt=""/>&nbsp;&nbsp;' + replaceMessage(encodedMsg) + '</li>');

        $('#' + dialogid).animate({ scrollTop: $(document).height() }, "slow");
    }

    function viewProfile(mId) {
        var p = '<%= profilePage %>';
        if(p != ""){
            window.location.href = <%= profilePage %> + "/userId/" + mId.toString();
        }
    };

    function setUserList(connid, uid, uname, upic) {

        if ($('#tblMembers').closest("tbody").find("tr").length) {
            $('#spanOnline').show();
        } else {
            $('#spanOnline').hide();
        }

        var tcolor = '<%= busyTextColor %>';

        if ('<%= userLoggedIn %>' == "True") {
            $('#tblMembers tbody:first').append('<tr id="' + connid + '"><td><img id="dialog-img" src="' + upic + '" alt=""/></td><td><table><tr><td><a href="javascript:viewProfile(\'' + uid + '\');">' + uname + '</a></td></tr><tr><td><span id="busy' + uid.toString() + '" style="color:' + tcolor + '; display:none">Busy</span><a id="' + uid.toString() + '" href="javascript:startChat(\'' + uname.trim() + '\',\'' + <%= UserId %> + '\',\'' + uid + '\');">Chat</a>&nbsp;|&nbsp;<a id="block' + uid.toString() + '" href="javascript:blockMember(\'' + uid + '\');">Block</a><a id="unblock' + uid.toString() + '" href="javascript:unblockMember(\'' + uid + '\');" style="display:none">Unblock</a></td></tr></table></td></tr>');
        } else {
            $('#tblMembers tbody:first').append('<tr id="' + connid + '"><td><img id="dialog-img" src="' + upic + '" alt=""/></td><td><table><tr><td><a href="javascript:viewProfile(\'' + uid + '\');">' + uname + '</a></td></tr></table></td></tr>');
        }

    };

    function blockMember(mId) {
        wfMemberChatHub.server.blockMember(mId);
    }

    function unblockMember(mId) {
        wfMemberChatHub.server.unblockMember(mId);
    }

    function changeStatus() {

        var stext;

        var radioButtons = document.getElementById('<%= rblStatus.ClientID %>');
        var radio = radioButtons.getElementsByTagName("input");
        var label = radioButtons.getElementsByTagName("label");

        for (var x = 0; x < radio.length; x++) {
            if (radio[x].checked) {
                stext = label[x].innerHTML;
                wfMemberChatHub.server.updateUserStatus(radio[x].value);
            }
        }

        $('#btnStatus').attr('value', "My Status: " + stext);
    }

    function createChatDialog(chatstarted, dialogid, dialogTitle, mOneId, mTwoId) {

        if ($('#' + dialogid).length == 0) {

            var mname = "message-" + dialogid;

            var dynamicDialog = $('<div id="' + dialogid + '"><ul id="discussion"></ul></div>');

            dynamicDialog.dialog({
                title: dialogTitle,
                modal: false,
                width: 340,
                height: 320,
                dialogClass: "dnnFormPopup",
                buttons: [{
                    text: "Send", id: "btnSend", class: "dnnPrimaryAction", click: function () {

                        if ($('#' + mname).val().length > 0) {

                            if (chatstarted == false) {

                                chatstarted = true;

                                wfMemberChatHub.server.startChat(dialogid, mOneId, mTwoId, $('#' + mname).val());

                            } else {

                                wfMemberChatHub.server.sendMessage(dialogid, $('#' + mname).val(), wfMemberChatHub.state.userid);
                            }

                            $('#' + mname).val('').focus();

                        } else {

                            $('#' + mname).val('').focus();

                        }
                    }
                }]
            });

            if (mOneId.toString() != '<%= UserId %>') {
                $('#' + dialogid).dialog('close');
            }

            $('#' + mname).focus();

            $('<span id="dialog-span-typing"></span>').appendTo($('#' + dialogid).parent().find('.ui-dialog-titlebar '));

            $('#' + dialogid).next().prepend('<input type="text" id="' + mname + '" class="dialog-textbox" />');

            $('#' + mname).keypress(function (e) {

                if (mOneId.toString() == '<%= UserId %>') {
                    wfMemberChatHub.server.keyPress(dialogid, mTwoId);
                } else {
                    wfMemberChatHub.server.keyPress(dialogid, mOneId);
                }

                if (e.keyCode == $.ui.keyCode.ENTER) {

                    $('#' + dialogid).parent().find('.ui-dialog-buttonpane button:first').click();
                }

            });

            getChatHistory(dialogid, mOneId, mTwoId);

            if (dialog_x != 0 && dialog_y != 0) {
                $('#' + dialogid).parent().offset({ top: (center_y - (($('#' + dialogid).outerHeight() / 2)) + dialog_y) - 40, left: (center_x - ($('#' + dialogid).outerWidth() / 2)) + dialog_x });
            } else {
                $('#' + dialogid).parent().offset({ top: (center_y - ($('#' + dialogid).outerHeight() / 2)) - 40, left: (center_x - ($('#' + dialogid).outerWidth() / 2)) });
            }

            dialog_x += 20;
            dialog_y += 20;

            if (dialog_x >= 100 && dialog_y >= 100) {
                dialog_x = 0;
                dialog_y = 0;
            }


            } else {

                $('#' + dialogid).dialog('open');
            }

            $('#' + dialogid).animate({ scrollTop: $(document).height() }, "slow");
        };

        var emoticonsUrl = "../DesktopModules/JPMemberChat/Images/emoticons/";

        var emoticons = {
            ':-)': 'smiling.png',
            ':)': 'smiling.png',
            '=)': 'smiling.png',
            ';)': 'wink.png',
            ';P': 'silly_tongue_out.png',
            ';D': 'mad.png',
            ':D': 'cool-hair.png',
            '=D': 'cool-hair.png',
            ':P': 'tongue_out.png',
            ':(': 'silly-tongue-out.png',
            ':\\': 'unsure_2.png'
        }, url = emoticonsUrl, patterns = [],
               metachars = /[[\]{}()*+?.\\|^$\-,&#\s]/g;

        // build a regex pattern for each defined property
        for (var i in emoticons) {
            if (emoticons.hasOwnProperty(i)) { // escape metacharacters
                patterns.push('(' + i.replace(metachars, "\\$&") + ')');
            }
        }

        function replaceMessage(message) {

            //urls
            var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
            message = message.replace(exp, "<a href='$1' target='_blank'>$1</a>");

            //emoticons
            message = message.replace(new RegExp(patterns.join('|'), 'g'), function (match) {
                return typeof emoticons[match] != 'undefined' ? '<img src="' + url + emoticons[match] + '"/>' : match;
            });

            //check for long string of characters
            return message;
        }

        $(document).ready(function () {            

            if ('<%= userLoggedIn %>' == "True") {
                $('#btnStatus').show();
            } else {
                $('#btnStatus').hide();
            }

            $('#btnStatus').click(function () {

                $('#dialog-status').dialog({
                    title: 'Status',
                    modal: false,
                    width: 340,
                    height: 320,
                    dialogClass: "dnnFormPopup",
                    buttons: [{
                        text: "Close", class: "dnnPrimaryAction", click: function () { $(this).dialog("close"); }
                    }]
                });

                return false;
            });

            //if ($('#tblMembers').closest("tbody").find("tr").length) {
            //    $('#spanOnline').show();
            //} else {
            //    $('#spanOnline').hide();
            //}

        });

</script>
