using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using DotNetNuke.Entities.Users;
using System.Web.Security;
using DotNetNuke.Entities.Portals;

namespace JPShaver.Modules.JPMemberChat.Components
{
    [HubName("JPMemberChatHub")]
    public class JPMemberChatHub : Hub
    {
        public override Task OnConnected()
        {
            //Clients.All.logToConsole("connected");            
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            //Clients.All.logToConsole("reconnected");
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            //Clients.All.logToConsole("disconnected");
            new ConnectionRecordController().DeleteConnectionRecord(Context.ConnectionId);
            Clients.Others.deleteUserFromList(Context.ConnectionId);
            return base.OnDisconnected();
        }

        public async Task AddConnection()
        {
            if (Convert.ToInt32(Clients.Caller.userid) > 0)
            {
                var i = new ConnectionRecordController().checkConnectionExists(Convert.ToInt32(Clients.Caller.userid));

                if (i >= 1)
                {
                    new ConnectionRecordController().DeleteConnectionRecordByUserId(Convert.ToInt32(Clients.Caller.userid));
                }

                ConnectionRecord cr = new ConnectionRecord();
                cr.ConnectionId = Context.ConnectionId;
                cr.UserId = Convert.ToInt32(Clients.Caller.userid);
                cr.UserName = Clients.Caller.username;

                new ConnectionRecordController().CreateConnectionRecord(cr);

                foreach (ChatMember cm in new ChatMemberController().GetChatsByMemberId(Convert.ToInt32(Clients.Caller.userid)))
                {
                    ChatController cc = new ChatController();
                    var cname = cc.GetChatNameByChatId(cm.ChatId);

                    await JoinChat(Context.ConnectionId, cname);
                }

                foreach (ConnectionRecord cr1 in new ConnectionRecordController().GetConnectionRecords())
                {
                    var pid = new PortalSettings().PortalId;

                    if (cr1.UserId.ToString() != Clients.Caller.userid)
                    {
                        int statusid = new MemberStatusController().GeStatusIdByMemberId(cr1.UserId);
                        await Clients.Caller.printUsersToList(cr1.ConnectionId, cr1.UserId, UserController.GetUser(pid, cr1.UserId, true).DisplayName, "../profilepic.ashx?userid=" + cr1.UserId.ToString(), statusid);
                    }
                }

                await StartStatus();
                await StartBlocks();
            }
            else
            {
                foreach (ConnectionRecord cr1 in new ConnectionRecordController().GetConnectionRecords())
                {
                    var pid = new PortalSettings().PortalId;

                    int statusid = new MemberStatusController().GeStatusIdByMemberId(cr1.UserId);
                    await Clients.Caller.printUsersToList(cr1.ConnectionId, cr1.UserId, UserController.GetUser(pid, cr1.UserId, true).DisplayName, "../profilepic.ashx?userid=" + cr1.UserId.ToString(), statusid);

                }
            }
        }

        public async Task StartChat(string dialogid, int mOneId, int mTwoId, string message)
        {
            Chat c = new Chat();
            c.Name = dialogid;
            c.CreatedOnDateTime = DateTime.Now;

            Chat h = new ChatController().CreateChat(c);

            //add chat members
            ChatMember cm1 = new ChatMember();
            cm1.ChatId = h.ChatId;
            cm1.MemberId = mOneId;

            ChatMember cm2 = new ChatMember();
            cm2.ChatId = h.ChatId;
            cm2.MemberId = mTwoId;

            new ChatMemberController().CreateChatMember(cm1);
            new ChatMemberController().CreateChatMember(cm2);

            //get member connection id and add to Chat (group)
            var mo = new ConnectionRecordController().GetConnectionRecordByUserId(mOneId);
            var mt = new ConnectionRecordController().GetConnectionRecordByUserId(mTwoId);

            await JoinChat(mo, dialogid);
            await JoinChat(mt, dialogid);

            await Clients.Group(dialogid).setChat(dialogid, Clients.Caller.displayname, mOneId, mTwoId);

            SendFirstMessage(dialogid, message, mOneId);
        }

        public async Task JoinChat(string m, string name)
        {
            await Groups.Add(m, name);
        }

        public void SendFirstMessage(string dialogid, string message, int sentmemberid)
        {
            Clients.Caller.broadcastMessage(Context.ConnectionId, dialogid, sentmemberid, "../profilepic.ashx?userid=" + Clients.Caller.userid.ToString(), Clients.Caller.displayname, message);
            RecordMessage(new ChatController().GetChatIdByChatName(dialogid), message, sentmemberid);
        }

        public void SendMessage(string dialogid, string message, int sentmemberid)
        {
            Clients.Group(dialogid).broadcastMessage(Context.ConnectionId, dialogid, sentmemberid, "../profilepic.ashx?userid=" + Clients.Caller.userid.ToString(), Clients.Caller.displayname, message);
            RecordMessage(new ChatController().GetChatIdByChatName(dialogid), message, sentmemberid);
        }

        public void RecordMessage(int cid, string msg, int sentmemberid)
        {
            ChatMessage cm = new ChatMessage();
            cm.ChatId = cid;
            cm.Message = msg;
            cm.SentMemberId = sentmemberid;
            cm.SentDateTime = DateTime.Now;

            new ChatMessageController().CreateMessage(cm);
        }

        public void KeyPress(string dialogid, int mTwoId)
        {
            Clients.Client(new ConnectionRecordController().GetConnectionRecordByUserId(mTwoId)).broadcastKeypress(Clients.Caller.displayname, dialogid);
        }

        public string CheckChatExists(int mOneId, int mTwoId)
        {
            var cname = new ChatController().GetChatNameByMembers(mOneId, mTwoId);

            if (String.IsNullOrEmpty(cname))
            {
                return "empty";
            }
            else
            {
                return cname;
            }
        }

        public async Task ChatHistory(string dialogid, int mOneId, int mTwoId)
        {
            int cid = new ChatMemberController().GetChatIdByMembers(mOneId, mTwoId);

            foreach (ChatMessage cm in new ChatMessageController().GetMessagesHistory(cid, 10))
            {
                await Clients.Caller.broadcastHistory(dialogid, cm.SentMemberId, "../profilepic.ashx?userid=" + cm.SentMemberId.ToString(), Clients.Caller.displayname, cm.Message);
            }
        }

        public async Task StartStatus()
        {
            int ms = new MemberStatusController().GeStatusIdByMemberId(Convert.ToInt32(Clients.Caller.userid));

            if (ms == 0)
            {
                var ms1 = new MemberStatus();
                ms1.MemberId = Convert.ToInt32(Clients.Caller.userid);
                ms1.StatusId = 1;

                new MemberStatusController().CreateMemberStatus(ms1);
            }
            await UpdateUserListStatus();
        }

        public async Task UpdateUserStatus(string statusid)
        {
            var ms = new MemberStatusController().GetMemberStatusByMemberId(Convert.ToInt32(Clients.Caller.userid));
            ms.StatusId = Convert.ToInt32(statusid);

            new MemberStatusController().UpdateMemberStatus(ms);

            await UpdateUserListStatus();
        }

        public async Task UpdateUserListStatus()
        {
            int ms = new MemberStatusController().GeStatusIdByMemberId(Convert.ToInt32(Clients.Caller.userid));

            await Clients.Caller.setStatusBtnText(new StatusController().GetTypeByStatusId(ms));

            if (ms == 1)
            {
                //is online
                await Clients.Others.addUserToList(Context.ConnectionId, Clients.Caller.userid, Clients.Caller.displayname, "../profilepic.ashx?userid=" + Clients.Caller.userid.ToString());
            }

            if (ms == 2)
            {
                //is offline
                await Clients.Others.deleteUserFromList(Context.ConnectionId);
            }

            if (ms == 3)
            {
                // is busy
                await Clients.Others.addUserToList(Context.ConnectionId, Clients.Caller.userid, Clients.Caller.displayname, "../profilepic.ashx?userid=" + Clients.Caller.userid.ToString());
                await Clients.Others.setUserBusyStatus(Clients.Caller.userid);
            }
        }

        public async Task StartBlocks()
        {
            foreach (Block b in new BlockController().GetBlocksByMemberId(Convert.ToInt32(Clients.Caller.userid)))
            {
                await UpdateUserListBlock(true, b.MemberId, b.BlockedMemberId);
            }

            foreach (Block b in new BlockController().GetBlocksByBlockedMemberId(Convert.ToInt32(Clients.Caller.userid)))
            {
                var connid = new ConnectionRecordController().GetConnectionRecordByUserId(b.MemberId);
                await Clients.Caller.deleteUserFromList(connid);
                await Clients.Client(connid).setBlockText(true, b.BlockedMemberId);
            }
        }

        public async Task BlockMember(int bmid)
        {
            var b = new Block();
            b.MemberId = Convert.ToInt32(Clients.Caller.userid);
            b.BlockedMemberId = bmid;

            new BlockController().CreateBlock(b);

            await UpdateUserListBlock(true, b.MemberId, b.BlockedMemberId);

        }

        public async Task UnblockMember(int bmid)
        {
            var b = new Block();
            b.MemberId = Convert.ToInt32(Clients.Caller.userid);
            b.BlockedMemberId = bmid;

            new BlockController().DeleteBlock(b);

            await UpdateUserListBlock(false, b.MemberId, b.BlockedMemberId);
        }

        public async Task UpdateUserListBlock(Boolean isBlocked, int mid, int bmid)
        {
            var connid = new ConnectionRecordController().GetConnectionRecordByUserId(bmid);

            if (isBlocked)
            {
                Clients.Client(connid).deleteUserFromList(Context.ConnectionId);
            }
            else
            {
                Clients.Client(connid).addUserToList(Context.ConnectionId, Clients.Caller.userid, Clients.Caller.displayname, "../profilepic.ashx?userid=" + Clients.Caller.userid.ToString());
            }

            await Clients.Caller.setBlockText(isBlocked, bmid);
        }

        public Task LeaveChat(string chatname)
        {
            return Groups.Remove(Context.ConnectionId, chatname);
        }
    }
}