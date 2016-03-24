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
    class ChatMessageController
    {
        public void CreateMessage(ChatMessage t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMessage>();
                rep.Insert(t);
            }
        }

        public void DeleteMessage(int messageId, int moduleId)
        {
            var t = GetMessage(messageId, moduleId);
            DeleteMessage(t);
        }

        public void DeleteMessage(ChatMessage t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMessage>();
                rep.Delete(t);
            }
        }

        public IEnumerable<ChatMessage> GetMessages(int moduleId)
        {
            IEnumerable<ChatMessage> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMessage>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public ChatMessage GetMessage(int messageId, int moduleId)
        {
            ChatMessage t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMessage>();
                t = rep.GetById(messageId, moduleId);
            }
            return t;
        }

        public IEnumerable<ChatMessage> GetMessagesHistory(int chatId, int records)
        {
            IEnumerable<ChatMessage> t;            

            string sql = "SELECT TOP " + records.ToString() + " * FROM {databaseOwner}[{objectQualifier}JPMemberChat_ChatMessage] WHERE ChatId = " + chatId + " ORDER BY MessageId DESC";

            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteQuery<ChatMessage>(CommandType.Text, sql);
            }
            return t;
        }

        public void UpdateMessage(ChatMessage t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ChatMessage>();
                rep.Update(t);
            }
        }

    }
}
