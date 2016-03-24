/*
' Copyright (c) 2014 John Shaver
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/
using System.Collections.Generic;
using DotNetNuke.Data;
using System.Data;

namespace JPShaver.Modules.JPMemberChat.Components
{
    class ChatController
    {
        public Chat CreateChat(Chat t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Chat>();
                rep.Insert(t);
            }

            return t;
        }

        public void DeleteChat(int chatId, int moduleId)
        {
            var t = GetChat(chatId, moduleId);
            DeleteChat(t);
        }

        public void DeleteChat(Chat t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Chat>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Chat> GetChats(int moduleId)
        {
            IEnumerable<Chat> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Chat>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Chat GetChat(int chatId, int moduleId)
        {
            Chat t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Chat>();
                t = rep.GetById(chatId, moduleId);
            }
            return t;
        }

        public string GetChatNameByChatId(int chatId)
        {
            string t;
            string sql = "SELECT Name FROM {databaseOwner}[{objectQualifier}JPMemberChat_Chat] WHERE ChatId = " + chatId;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<string>(CommandType.Text, sql);
            }

            return t;
        }

        public int GetChatIdByChatName(string chatname)
        {
            int t;
            string sql = "SELECT ChatId FROM {databaseOwner}[{objectQualifier}JPMemberChat_Chat] WHERE Name = " + chatname;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<int>(CommandType.Text, sql);
            }

            return t;
        }

        public string GetChatNameByMembers(int mOneId, int mTwoId)
        {
            string t;
            string sql = "SELECT NAME FROM {databaseOwner}[{objectQualifier}JPMemberChat_Chat] WHERE ChatId = (SELECT ChatId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ChatMember] WHERE MemberId = " + mOneId.ToString() + " INTERSECT SELECT ChatId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ChatMember] WHERE MemberId = " + mTwoId.ToString() + ")";
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<string>(CommandType.Text, sql);
            }

            return t;
        }

        public IEnumerable<Chat> GetChatsByMemberId(int memberId)
        {
            IEnumerable<Chat> t;
            string sql = "SELECT * FROM {databaseOwner}[{objectQualifier}JPMemberChat_Chat] c INNER JOIN {databaseOwner}[{objectQualifier}JPMemberChat_ChatMember] cm ON c.ChatId = cm.ChatId WHERE MemberId = " + memberId.ToString();
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteQuery<Chat>(CommandType.Text, sql);
            }
            return t;
        }


        public void UpdateChat(Chat t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Chat>();
                rep.Update(t);
            }
        }

    }
}
