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

using System;
using System.Web.Caching;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;

namespace JPShaver.Modules.JPMemberChat.Components
{
    [TableName("JPMemberChat_ChatMessage")]
    //setup the primary key for table
    [PrimaryKey("MessageId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Messages", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    //[Scope("ModuleId")]
    class ChatMessage
    {        
        public int MessageId { get; set; }
        
        public int ChatId { get; set; }
                
        public string Message { get; set; }

        public int SentMemberId { get; set; }
         
        public DateTime SentDateTime { get; set; }

    }
}
