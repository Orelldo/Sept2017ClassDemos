﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AlbumsByYearRange.aspx.cs" Inherits="SamplePages_AlbumsByYearRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Albums By Year Range</h1>

    <asp:Label ID="Label1" runat="server" Text="Enter Min Year" />
    <asp:TextBox ID="minYear" runat="server" />

    <asp:Label ID="Label2" runat="server" Text="Enter Max Year" />
    <asp:TextBox ID="maxYear" runat="server" />

    <asp:LinkButton ID="LinkButton1" runat="server">Submit</asp:LinkButton>
    <br />

    <asp:GridView ID="AlbumsList" runat="server" AutoGenerateColumns="False" DataSourceID="AlbumsListODS" AllowPaging="True">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title"></asp:BoundField>
            <asp:BoundField DataField="ReleaseYear" HeaderText="ReleaseYear" SortExpression="ReleaseYear"></asp:BoundField>
            <asp:BoundField DataField="ReleaseLabel" HeaderText="ReleaseLabel" SortExpression="ReleaseLabel"></asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="AlbumsListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Albums_FindByYearRange" TypeName="ChinookSystem.BLL.AlbumController">
        <SelectParameters>
            <asp:ControlParameter ControlID="minYear" PropertyName="Text" DefaultValue="1800" Name="minYear" Type="Int32"></asp:ControlParameter>
            <asp:ControlParameter ControlID="maxYear" PropertyName="Text" DefaultValue="2017" Name="maxYear" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>