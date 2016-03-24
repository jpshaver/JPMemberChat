<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="JPShaver.Modules.JPMemberChat.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>

<div class="dnnForm" id="panel">

    <h2 id="Settings" class="dnnFormSectionHead"><a href="#">Settings</a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUserProfilePage" runat="server" />
            <asp:DropDownList ID="ddlUserProfilePage" runat="server" AppendDataBoundItems="True">
                <asp:ListItem Value="-1">Select</asp:ListItem>
            </asp:DropDownList>
        </div>
   
        <div class="dnnFormItem">
            <dnn:Label ID="lblAlertTextColor1" runat="server" />
            <asp:TextBox ID="txtAlertTextColor1" runat="server"></asp:TextBox>
        </div>
   
        <div class="dnnFormItem">
            <dnn:Label ID="lblAlertTextColor2" runat="server" />
            <asp:TextBox ID="txtAlertTextColor2" runat="server"></asp:TextBox>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblBusyTextColor" runat="server" />
            <asp:TextBox ID="txtBusyTextColor" runat="server"></asp:TextBox>
        </div>
    </fieldset>

    <h2 id="Statuses" class="dnnFormSectionHead"><a href="#">Status Types</a></h2>
    <fieldset class="dnnClear">
        <div class="dnnFormItem">
            <dnn:Label ID="lblStatusType" runat="server" />
            <asp:TextBox ID="txtStatusType" runat="server" />
            <asp:LinkButton ID="btnStatus" runat="server" OnClick="btnStatus_Click" resourcekey="btnStatus" CssClass="dnnPrimaryAction" />
            <asp:HiddenField ID="hdfStatusId" runat="server" />
        </div>

        <div class="dnnForm dnnSecurityRoles">

            <asp:DataGrid ID="grdStatusTypes" Width="98%" runat="server" BorderStyle="None" AutoGenerateColumns="false" GridLines="None" CssClass="dnnGrid" OnItemCommand="grdStatusTypes_ItemCommand">
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
                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditStatusType" CommandArgument='<%# Eval("StatusId") %>' ImageUrl="/DesktopModules/JPMemberChat/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnArchive" runat="server" CommandName="DeleteStatusType" CommandArgument='<%# Eval("StatusId") %>' ImageUrl="/DesktopModules/JPMemberChat/Images/delete.png" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="Type" HeaderText="Status Type"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid>

        </div>
    </fieldset>

</div>
