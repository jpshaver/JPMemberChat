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
    class ChatMemberController
    {
        public void CreateChatMember(ChatMember t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMember>();
                rep.Insert(t);
            }
        }

        public void DeleteChatMember(int chatmemberId)
        {
            var t = GetChatMember(chatmemberId);
            DeleteChatMember(t);
        }

        public void DeleteChatMember(ChatMember t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMember>();
                rep.Delete(t);
            }
        }

        public IEnumerable<ChatMember> GetChatMembers(int moduleId)
        {
            IEnumerable<ChatMember> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMember>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public ChatMember GetChatMember(int chatmemberId)
        {
            ChatMember t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMember>();
                t = rep.GetById(chatmemberId);
            }
            return t;
        }

        public IEnumerable<ChatMember> GetChatsByMemberId(int memberId)
        {
            IEnumerable<ChatMember> t;
            string sql = "SELECT * FROM {databaseOwner}[{objectQualifier}JPMemberChat_ChatMember] WHERE MemberId = " + memberId.ToString();

            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteQuery<ChatMember>(CommandType.Text, sql);
            }

            return t;
        }


        public int GetChatIdByMembers(int mOneId, int mTwoId)
        {
            int t;
            string sql = "SELECT ChatId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ChatMember] WHERE MemberId = " + mOneId.ToString() + " INTERSECT SELECT ChatId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ChatMember] WHERE MemberId = " + mTwoId.ToString();
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<int>(CommandType.Text, sql);
            }

            return t;
        }

        public int GetOtherChatMemberId(int chatId, int memberId)
        {
            int t;
            string sql = "SELECT MemberId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ChatMember] WHERE MemberId <> " + memberId.ToString() + " AND ChatId = " + chatId;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<int>(CommandType.Text, sql);
            }

            return t;
        }

        public void UpdateChatMember(ChatMember t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMember>();
                rep.Update(t);
            }
        }

    }
}
