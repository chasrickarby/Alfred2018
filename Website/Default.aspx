<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Website.Default" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Label Text="Room " runat="server"></asp:Label>
        <asp:DropDownList
            ID="ddlRooms"
            runat="server" 
            OnSelectedIndexChanged="DdlRooms_SelectedIndexChanged"
            AutoPostBack="True"/>
        <asp:Label ID="lblLastUpdateTime" runat="server" Text="update time"></asp:Label>
    </div>
    <div>
        <DayPilot:DayPilotCalendar
            runat="server"
            id="DayPilotCalendar1">
        </DayPilot:DayPilotCalendar>
    </div>    
</asp:Content>