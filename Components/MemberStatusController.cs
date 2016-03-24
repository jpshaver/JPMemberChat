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
using System.Linq;
using System;

namespace JPShaver.Modules.JPMemberChat.Components
{
    class MemberStatusController
    {
        public MemberStatus CreateMemberStatus(MemberStatus t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<MemberStatus>();
                rep.Insert(t);
            }

            return t;
        }

        public void DeleteMemberStatus(int memberStatusId)
        {
            var t = GetMemberStatus(memberStatusId);
            DeleteMemberStatus(t);
        }

        public void DeleteMemberStatus(MemberStatus t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<MemberStatus>();
                rep.Delete(t);
            }
        }

        public IEnumerable<MemberStatus> GetMemberStatuses()
        {
            IEnumerable<MemberStatus> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<MemberStatus>();
                t = rep.Get();
            }
            return t;
        }

        public MemberStatus GetMemberStatus(int memberStatusId)
        {
            MemberStatus t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<MemberStatus>();
                t = rep.GetById(memberStatusId);
            }
            return t;
        }

        public int GeStatusIdByMemberId(int memberId)
        {
            int t;
            string sql = "SELECT COALESCE((SELECT StatusId FROM {databaseOwner}[{objectQualifier}JPMemberChat_MemberStatus] WHERE MemberId = " + memberId.ToString() + "), 0) AS StatusId";
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<int>(CommandType.Text, sql);
            }
            
            return t;            
        }

        public MemberStatus GetMemberStatusByMemberId(int memberId)
        {
            MemberStatus t;
            string sql = "SELECT * FROM {databaseOwner}[{objectQualifier}JPMemberChat_MemberStatus] WHERE MemberId = " + memberId.ToString();
            
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteSingleOrDefault<MemberStatus>(CommandType.Text, sql);
            }
            return t;
        }

        public void UpdateMemberStatus(MemberStatus t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<MemberStatus>();
                rep.Update(t);
            }
        }

    }
}
