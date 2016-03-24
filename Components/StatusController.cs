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
    class StatusController
    {
        public Status CreateStatus(Status t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Status>();
                rep.Insert(t);
            }

            return t;
        }

        public void DeleteStatus(int statusId)
        {
            var t = GetStatus(statusId);
            DeleteStatus(t);
        }

        public void DeleteStatus(Status t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Status>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Status> GetStatuses()
        {
            IEnumerable<Status> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Status>();
                t = rep.Find("Where Archived = 0");
            }
            return t;
        }

        public Status GetStatus(int statusId)
        {
            Status t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Status>();
                t = rep.GetById(statusId);
            }
            return t;
        }

        public string GetTypeByStatusId(int statusId)
        {
            string t;
            string sql = "SELECT Type FROM {databaseOwner}[{objectQualifier}JPMemberChat_Status] WHERE StatusId = " + statusId.ToString();
            
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<string>(CommandType.Text, sql);
            }

            return t;
        }

        public void UpdateStatus(Status t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Status>();
                rep.Update(t);
            }
        }

    }
}
