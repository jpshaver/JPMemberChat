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
using DotNetNuke.Entities.Users;
using System.Data;
using System.Linq;

namespace JPShaver.Modules.JPMemberChat.Components
{
    class ConnectionRecordController
    {

        public void CreateConnectionRecord(ConnectionRecord t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ConnectionRecord>();
                rep.Insert(t);
            }
        }

        public void DeleteConnectionRecord(string connectionId)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ConnectionRecord>();
                rep.Delete("WHERE ConnectionId = @0",connectionId);
            }            
        }

        public void DeleteConnectionRecord(ConnectionRecord t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ConnectionRecord>();
                rep.Delete(t);
            }
        }

        public void DeleteConnectionRecordByUserId(int userid)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ConnectionRecord>();
                rep.Delete("WHERE UserId = " + userid.ToString());
            } 
        }

        public IEnumerable<ConnectionRecord> GetConnectionRecords()
        {
            IEnumerable<ConnectionRecord> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ConnectionRecord>();
                t = rep.Get();
            }
            return t;
        }

        public ConnectionRecord GetConnectionRecord(int connectionrecordId)
        {
            ConnectionRecord t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ConnectionRecord>();
                t = rep.GetById(connectionrecordId);
            }
            return t;
        }

        public string GetConnectionRecordByUserId(int userid)
        {
            string r;
            string sql = "SELECT TOP 1 ConnectionId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ConnectionRecord] WHERE UserId = " + userid.ToString();
            using (IDataContext ctx = DataContext.Instance())
            {
                r = ctx.ExecuteSingleOrDefault<string>(CommandType.Text, sql);
            }

            return r;
        }

        public IEnumerable<ConnectionRecord> GetConnectionRecordByConnectionId(string connectionid)
        {
            IEnumerable<ConnectionRecord> t;
            string sql = "SELECT * FROM {databaseOwner}[{objectQualifier}JPMemberChat_ConnectionRecord] WHERE ConnectionId = " + connectionid;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteQuery<ConnectionRecord>(CommandType.Text, sql);
            }

            return t;
        }

        public int GetUserIdByConnectionId(string connectionid)
        {
            int t;
            string sql = "SELECT UserId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ConnectionRecord] WHERE ConnectionId = " + connectionid;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<int>(CommandType.Text, sql);
            }

            return t;
        }

        public int checkConnectionExists(int userid)
        {
            int t;
            string sql = "SELECT UserId FROM {databaseOwner}[{objectQualifier}JPMemberChat_ConnectionRecord] WHERE UserId = " + userid;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<int>(CommandType.Text, sql);
            }

            if (t >= 1)
            {
                return t;
            }
            else
            {
                return 0;
            }
        }

        public void UpdateConnectionRecord(ConnectionRecord t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ConnectionRecord>();
                rep.Update(t);
            }
        }

    }
}
