<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="JPShaver.Modules.JPMemberChat.Edit" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>

<div class="dnnForm dnnEditBasicSettings" id="dnnEditBasicSettings">

    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead dnnClear">
        <a href="" class="dnnSectionExpanded">
            <%=LocalizeString("BasicSettings")%></a></h2>

    <fieldset>
        <div class="dnnFormItem">
            <dnn:label ID="lblMember" runat="server" />
            <asp:DropDownList ID="ddlMember" runat="server" OnSelectedIndexChanged="ddlMember_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True">
                <asp:ListItem Value="-1">Select</asp:ListItem>
            </asp:DropDownList>
        </div>


        <div class="dnnFormItem">
            <asp:DataGrid ID="grdChats" Width="98%" runat="server" BorderStyle="None" AutoGenerateColumns="false" GridLines="None" CssClass="dnnGrid" OnItemCommand="grdChats_ItemCommand">
                <HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
                <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
                <AlternatingItemStyle CssClass="dnnGridAltItem" />
                <EditItemStyle CssClass="dnnFormInput" />
                <SelectedItemStyle CssClass="dnnFormError" />
                <FooterStyle CssClass="dnnGridFooter" />
                <PagerStyle CssClass="dnnGridPager" />
                <Columns>                   
                    <asp:TemplateColumn ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnViewLogs" runat="server" CommandName="ViewLogs" CommandArgument='<%# Eval("ChatId") %>' ImageUrl="/DesktopModules/JPMemberChat/Images/view.png" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="ChatId" HeaderText="Chat Id"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Name" HeaderText="Chat Name"></asp:BoundColumn>
                    <asp:BoundColumn DataField="CreatedOnDateTime" HeaderText="Date / Time Created"></asp:BoundColumn>
                    <asp:BoundColumn DataField="MemberName" HeaderText="Member Name"></asp:BoundColumn>                 
                    <asp:BoundColumn DataField="MemberUserName" HeaderText="Member User Name"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>


        <div class="dnnFormItem">
            <asp:DataGrid ID="grdMessages" Width="98%" runat="server" BorderStyle="None" AutoGenerateColumns="false" GridLines="None" CssClass="dnnGrid">
                <HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
                <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
                <AlternatingItemStyle CssClass="dnnGridAltItem" />
                <EditItemStyle CssClass="dnnFormInput" />
                <SelectedItemStyle CssClass="dnnFormError" />
                <FooterStyle CssClass="dnnGridFooter" />
                <PagerStyle CssClass="dnnGridPager" />
                <Columns>
                    <asp:TemplateColumn ItemStyle-Width="35px">
                        <ItemTemplate>
                            <asp:Image ID="imgMember" runat="server" ImageUrl='<%# Page.ResolveUrl("~/profilepic.ashx?userid=" + Convert.ToInt32(Eval("SentMemberId").ToString())) %>' Height="30px" Width="30px" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Name / Date" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td style="padding:0px;"><%# UserController.GetUserById(PortalId, Convert.ToInt32(Eval("SentMemberId"))).DisplayName %></td>
                                </tr>
                                <tr>
                                    <td style="padding:0px;"><%# Eval("SentDateTime") %></td>
                                </tr>
                            </table>                           
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="Message" HeaderText="Message"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>

    </fieldset>
</div>

<asp:LinkButton ID="btnCancel" runat="server"
    OnClick="btnCancel_Click" resourcekey="btnCancel" CssClass="dnnSecondaryAction" />


<script language="javascript" type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function dnnEditBasicSettings() {
            $('#dnnEditBasicSettings').dnnPanels();
            $('#dnnEditBasicSettings .dnnFormExpandContent a').dnnExpandAll({ expandText: '<%=Localization.GetString("ExpandAll", LocalResourceFile)%>', collapseText: '<%=Localization.GetString("CollapseAll", LocalResourceFile)%>', targetArea: '#dnnEditBasicSettings' });
        }

        $(document).ready(function () {
            dnnEditBasicSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                dnnEditBasicSettings();
            });
        });

    }(jQuery, window.Sys));
</script>
