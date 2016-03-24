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
using System.Web.UI.WebControls;
using JPShaver.Modules.JPMemberChat.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Entities.Tabs;
using JPshaver.Modules.JPMemberChat;

namespace JPShaver.Modules.JPMemberChat
{
    public partial class View : JPMemberChatModuleBase, IActionable
    {        
        public string profilePage;
        public string alertTextColor1;
        public string alertTextColor2;
        public string busyTextColor;

        public Boolean userLoggedIn = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                userLoggedIn = Request.IsAuthenticated;

                if (Settings.Contains("AlertTextColor1"))
                {
                    alertTextColor1 = Settings["AlertTextColor1"].ToString();
                }
                else
                {
                    alertTextColor1 = "#FF0000";                                 
                }

                if (Settings.Contains("AlertTextColor2"))
                {
                    alertTextColor2 = Settings["AlertTextColor2"].ToString();
                }
                else
                {
                    alertTextColor2 = "#3B96B6";                                 
                }

                if (Settings.Contains("BusyTextColor"))
                {
                    busyTextColor = Settings["BusyTextColor"].ToString();
                }
                else
                {
                    busyTextColor = "#FF0000";
                }


                if (Settings.Contains("UserProfilePage"))
                {
                    profilePage = "\"" + Page.ResolveUrl(DotNetNuke.Common.Globals.NavigateURL(Convert.ToInt32(Settings["UserProfilePage"]))) + "\"";
                }
                else
                {
                    var tc = new TabController();
                    TabInfo ti = tc.GetTabByName("Activity Feed", PortalId);

                    if (ti != null)
                    {
                        profilePage = "\"" + Page.ResolveUrl(DotNetNuke.Common.Globals.NavigateURL(ti.TabID)) + "\"";
                    }
                    else
                    {
                        profilePage = "";
                    }
                    
                }

                rblStatus.DataSource = new StatusController().GetStatuses();
                rblStatus.DataTextField = "Type";
                rblStatus.DataValueField = "StatusId";
                rblStatus.DataBind();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }        

        public string GetProfileImage(int userId)
        {
            return Page.ResolveUrl("~/profilepic.ashx?userid=" + userId.ToString());
        }                

        public ModuleActionCollection ModuleActions
        {
            get
            {
                //var actions = new ModuleActionCollection();

                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

    }
}