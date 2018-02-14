<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Website.Default" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table style="width:100%">
            <tr>
            <td>
                <asp:Label Text="Location " runat="server"></asp:Label>
                <asp:DropDownList
                    ID="ddlLocations"
                    runat="server" 
                    OnSelectedIndexChanged="DdlLocations_SelectedIndexChanged"
                    AutoPostBack="True"
                    Width="200px" Enabled="False"/>
                <asp:Label Text="Room " runat="server"></asp:Label>
                <asp:DropDownList
                    ID="ddlRooms"
                    runat="server" 
                    OnSelectedIndexChanged="DdlRooms_SelectedIndexChanged"
                    AutoPostBack="True"
                    Width="200px"/>
                <asp:Label ID="lblLastUpdateTime" runat="server" Text="update time"></asp:Label>
            </td>
            <td align="middle">
                <asp:Label ID="lblMotion" runat="server" Text="Unoccupied" ForeColor="Green" Font-Bold="true"></asp:Label>
            </td>
            <td align="right">
                <asp:Label ID="lblTemp" runat="server" Text="Temp"></asp:Label>
                /
                <asp:Label ID="lblHumidity" runat="server" Text="Humidity"></asp:Label>
            </td> 
            </tr>
        </table>
        
    </div>
    <div>
        <DayPilot:DayPilotCalendar
            runat="server"
            id="DayPilotCalendar1" style="top: 0px; left: 0px">
        </DayPilot:DayPilotCalendar>
    </div>    
</asp:Content>