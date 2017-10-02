<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GenreAlbumTracks.aspx.cs" Inherits="SamplePages_GenreAlbumTracks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1> Genre Albums and Tracks</h1>
    <!-- 
        Inside a repeater you need a minimum of a ItemTemplate other templates include HeaderTemplate FooterTemplate
            AlternatingItemTemplate, SeparatorTemplate.
        
        Outer repeater will display the first fields from the DTO Class which do not repeat.
        Outer repeater gets it's data from an ODS
        
        Nested (inner) repeater will display the collection of the DTO file
        Nested (inner) repeater will get it's Datasource from the collection of the DTO Class (Either a POCO or another DTO)
        
        This pattern repeats for all levels of the data set.    
    -->
</asp:Content>