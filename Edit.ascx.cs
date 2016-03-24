/*
' Copyright (c) 2014  John Shaver
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
using DotNetNuke.Entities.Users;
using JPShaver.Modules.JPMemberChat.Components;
using DotNetNuke.Services.Exceptions;
using System.Data;
using JPshaver.Modules.JPMemberChat;

namespace JPShaver.Modules.JPMemberChat
{
    /// -----------------------------------------------------------------------------
    /// <summary>   
    /// The Edit class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from JPMemberChatModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : JPMemberChatModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Implement your edit logic for your module
                if (!Page.IsPostBack)
                {
                    //get a list of users to assign the user to the Object
                    ddlMember.DataSource = UserController.GetUsers(PortalId);
                    ddlMember.DataTextField = "DisplayName";
                    ddlMember.DataValueField = "UserId";
                    ddlMember.DataBind();

                    //check if we have an ID passed in via a querystring parameter, if so, load that item to edit.
                    //ItemId is defined in the ItemModuleBase.cs file
                    if (memberId > 0)
                    {
                        ddlMember.SelectedValue = memberId.ToString();
                        //var tc = new ItemController();

                        //var t = tc.GetItem(ItemId, ModuleId);
                        //if (t != null)
                        //{
                        //    txtName.Text = t.ItemName;
                        //    txtDescription.Text = t.ItemDescription;
                        //    ddlAssignedUser.Items.FindByValue(t.AssignedUserId.ToString()).Selected = true;
                        //}
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void ddlMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMember.SelectedValue != "-1")
            {
                grdMessages.DataSource = "";
                grdMessages.DataBind();
                grdMessages.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ChatId", typeof(int)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("CreatedOnDateTime", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("MemberName", typeof(string)));
                dt.Columns.Add(new DataColumn("MemberUserName", typeof(string)));

                foreach (Chat c in new ChatController().GetChatsByMemberId(Convert.ToInt32(ddlMember.SelectedValue)))
                {
                    DataRow dr = dt.NewRow();
                    dr["ChatId"] = c.ChatId;
                    dr["Name"] = c.Name;
                    dr["CreatedOnDateTime"] = c.CreatedOnDateTime;

                    //get member username by id
                    int mid = new ChatMemberController().GetOtherChatMemberId(c.ChatId, Convert.ToInt32(ddlMember.SelectedValue));
                    dr["MemberName"] = UserController.GetUserById(PortalId, mid).DisplayName;
                    dr["MemberUserName"] = UserController.GetUserById(PortalId, mid).Username;

                    dt.Rows.Add(dr);
                }

                grdChats.Visible = true;
                grdChats.DataSource = dt;
                grdChats.DataBind();
            }
            else
            {
                grdChats.DataSource = "";
                grdChats.DataBind();
                grdChats.Visible = false;

                grdMessages.DataSource = "";
                grdMessages.DataBind();
                grdMessages.Visible = false;
            }
        }

        protected void grdChats_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ViewLogs":
                    {
                        grdMessages.Visible = true;
                        grdMessages.DataSource = new ChatMessageController().GetMessagesHistory(Convert.ToInt32(e.CommandArgument), 30);
                        grdMessages.DataBind();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }       

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }       

        
    }
}