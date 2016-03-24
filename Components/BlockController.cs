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
    class BlockController
    {
        public Block CreateBlock(Block t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Block>();
                rep.Insert(t);
            }

            return t;
        }

        public void DeleteBlock(int blockId, int moduleId)
        {
            var t = GetBlock(blockId, moduleId);
            DeleteBlock(t);
        }

        public void DeleteBlock(Block t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Block>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Block> GetBlocks(int moduleId)
        {
            IEnumerable<Block> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Block>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Block GetBlock(int chatId, int moduleId)
        {
            Block t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Block>();
                t = rep.GetById(chatId, moduleId);
            }
            return t;
        }

        public IEnumerable<Block> GetBlocksByMemberId(int memberId)
        {
            IEnumerable<Block> t;
            string sql = "SELECT * FROM {databaseOwner}[{objectQualifier}JPMemberChat_Block] WHERE MemberId = " + memberId.ToString();

            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteQuery<Block>(CommandType.Text, sql);
            }

            return t;
        }

        public IEnumerable<Block> GetBlocksByBlockedMemberId(int memberId)
        {
            IEnumerable<Block> t;
            string sql = "SELECT * FROM {databaseOwner}[{objectQualifier}JPMemberChat_Block] WHERE BlockedMemberId = " + memberId.ToString();

            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteQuery<Block>(CommandType.Text, sql);
            }

            return t;
        }

        public void UpdateBlock(Block t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Block>();
                rep.Update(t);
            }
        }

    }
}
