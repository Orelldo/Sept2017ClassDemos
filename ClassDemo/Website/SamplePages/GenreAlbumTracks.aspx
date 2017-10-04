<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GenreAlbumTracks.aspx.cs" Inherits="SamplePages_GenreAlbumTracks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>Genre Albums and Tracks</h1>
    <!-- 
        Inside a repeater you need a minimum of a ItemTemplate other templates include HeaderTemplate FooterTemplate
            AlternatingItemTemplate, SeparatorTemplate.
        
        Outer repeater will display the first fields from the DTO Class which do not repeat.
        Outer repeater gets it's data from an ODS
        
        Nested (inner) repeater will display the collection of the DTO file
        Nested (inner) repeater will get it's Datasource from the collection of the DTO Class (Either a POCO or another DTO)
        
        This pattern repeats for all levels of the data set.    
    -->

    <asp:Repeater ID="GenreAlbumTrackList" runat="server" DataSourceID="GenreAlbumTrackListODS" ItemType="Chinook.Data.DTOs.GenreDTO">
        <ItemTemplate>
            <h2>Genre: <%# Eval("genre") %></h2>
            <asp:Repeater ID="Albums" runat="server" DataSource='<%# Eval("albums") %>' ItemType="Chinook.Data.DTOs.AlbumDTO">
                <ItemTemplate>
                    <h4>Album: <%# string.Format("{0} ({1}) Tracks: {2}", Eval("title"), Eval("releaseYear"), Eval("numberOfTracks")) %></h4>
                    <br />
                    <asp:ListView ID="AlbumTracks" runat="server" DataSource="<%# Item.tracks %>" ItemType="Chinook.Data.POCOs.TrackPOCO">
                        <LayoutTemplate>
                            <table>
                                <tr>
                                    <th>Song</th>
                                    <th>Length</th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width:600px"><%# Item.song %></td>
                                <td><%# Item.length %></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <tr>
                                <td colspan="2">
                                    No Data Available At This Time.
                                </td>
                            </tr>
                        </EmptyDataTemplate>
                    </asp:ListView>
<%--                    <asp:GridView ID="AlbumTracks" runat="server" DataSource="<%# Item.tracks %>" ItemType="Chinook.Data.POCOs.TrackPOCO" AutoGenerateColumns="False" >
                        <Columns>
                            <asp:TemplateField HeaderText="Song">
                                <AlternatingItemTemplate>
                                    <asp:Label runat="server" Text="<%# Item.song %>" Width="600px" BackColor="#c0c0c0" />
                                </AlternatingItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text="<%# Item.song %>" Width="600px" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Length">
                                <AlternatingItemTemplate>
                                    <asp:Label runat="server" Text="<%# Item.length %>" BackColor="#c0c0c0" />
                                </AlternatingItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text="<%# Item.length %>" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>

<%--                <asp:Repeater ID="AlbumTracks" runat="server" DataSource="<%# Item.tracks %>" ItemType="Chinook.Data.POCOs.TrackPOCO">
                        <HeaderTemplate>
                            <table>
                                <tr>
                                    <th>Song</th>
                                    <th>Length</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width:600px">
                                    <%# Item.song %>
                                </td>
                                <td>
                                    <%# Item.length %>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>--%>
                </ItemTemplate>
                <SeparatorTemplate>
                    <hr style="height: 2px; border: none; color: black; background-color: black" />
                </SeparatorTemplate>
            </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
    <asp:ObjectDataSource ID="GenreAlbumTrackListODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="Genre_GenreAlbumTracks" TypeName="ChinookSystem.BLL.GenreController" />
</asp:Content>
