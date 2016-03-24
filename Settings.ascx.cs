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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using JPShaver.Modules.JPMemberChat.Components;
using JPshaver.Modules.JPMemberChat;

namespace JPShaver.Modules.JPMemberChat
{
    public partial class Settings : JPMemberChatModuleSettingsBase
    {
        #region Base Method Implementations       

        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    ddlUserProfilePage.DataSource = TabController.GetPortalTabs(PortalId, -1, true, Localization.GetString("DefaultPagesListItem.Text", LocalResourceFile), true, false, false, true, true);
                    ddlUserProfilePage.DataTextField = "TabName";
                    ddlUserProfilePage.DataValueField = "TabId";
                    ddlUserProfilePage.DataBind();

                    var modules = new ModuleController();

                    if (Settings.Contains("UserProfilePage"))
                        ddlUserProfilePage.SelectedValue = Settings["UserProfilePage"].ToString();

                    if (Settings.Contains("AlertTextColor1"))
                        txtAlertTextColor1.Text = Settings["AlertTextColor1"].ToString();

                    if (Settings.Contains("AlertTextColor2"))
                        txtAlertTextColor2.Text = Settings["AlertTextColor2"].ToString();

                    if (Settings.Contains("BusyTextColor"))
                        txtBusyTextColor.Text = Settings["BusyTextColor"].ToString();

                    bindControls();

                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                var modules = new ModuleController();

                if (ddlUserProfilePage.SelectedValue != "-1")
                {
                    modules.UpdateModuleSetting(ModuleId, "UserProfilePage", ddlUserProfilePage.SelectedValue);
                }

                modules.UpdateModuleSetting(ModuleId, "AlertTextColor1", txtAlertTextColor1.Text);
                modules.UpdateModuleSetting(ModuleId, "AlertTextColor2", txtAlertTextColor2.Text);
                modules.UpdateModuleSetting(ModuleId, "BusyTextColor", txtBusyTextColor.Text);


                //the following are two sample Module Settings, using the text boxes that are commented out in the ASCX file.                
                //modules.UpdateModuleSetting(ModuleId, "Setting1", txtSetting1.Text);
               

                //tab module settings
                //modules.UpdateTabModuleSetting(TabModuleId, "Setting1",  txtSetting1.Text);
                
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion        
 
        protected void grdStatusTypes_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteStatusType":
                    {
                        Status s = new StatusController().GetStatus(Convert.ToInt32(e.CommandArgument));
                        s.Archived = true;

                        new StatusController().UpdateStatus(s);
                        
                        bindControls();

                        break;
                    }

                case "EditStatusType":
                    {
                        Status s = new StatusController().GetStatus(Convert.ToInt32(e.CommandArgument));

                        txtStatusType.Text = s.Type;

                        hdfStatusId.Value = e.CommandArgument.ToString();

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        protected void btnStatus_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(hdfStatusId.Value))
            {
                var s = new Status();
                s.Type = txtStatusType.Text;
                s.Archived = false;

                new StatusController().CreateStatus(s);               
            }
            else
            {
                Status s = new StatusController().GetStatus(Convert.ToInt32(hdfStatusId.Value));
                s.Type = txtStatusType.Text;

                new StatusController().UpdateStatus(s);                
            }

            bindControls();
        }

        private void bindControls()
        {
            grdStatusTypes.DataSource = new StatusController().GetStatuses();
            grdStatusTypes.DataBind();
            txtStatusType.Text = "";
            hdfStatusId.Value = "";
        }
    }
}