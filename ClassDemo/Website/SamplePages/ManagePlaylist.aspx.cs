using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using ChinookSystem.BLL;
using Chinook.Data.POCOs;

#endregion
public partial class SamplePages_ManagePlaylist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.IsAuthenticated)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
        else
        {
            TracksSelectionList.DataSource = null;
        }
    }

    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.HandleDataBoundException(e);
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        //PreRenderComplete occurs just after databinding page events
        //load a pointer to point to your DataPager control
        DataPager thePager = TracksSelectionList.FindControl("DataPager1") as DataPager;
        if (thePager !=null)
        {
            //this code will check the StartRowIndex to see if it is greater that the
            //total count of the collection
            if (thePager.StartRowIndex > thePager.TotalRowCount)
            {
                thePager.SetPageProperties(0, thePager.MaximumRows, true);
            }
        }
    }

    protected void ArtistFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "Artist";
        SearchArgID.Text = ArtistDDL.SelectedValue;
        TracksSelectionList.DataBind();
    }

    protected void MediaTypeFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "MediaType";
        SearchArgID.Text = MediaTypeDDL.SelectedValue;
        TracksSelectionList.DataBind();
    }

    protected void GenreFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "Genre";
        SearchArgID.Text = GenreDDL.SelectedValue;
        TracksSelectionList.DataBind();
    }

    protected void AlbumFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "Album";
        TracksSelectionList.DataBind();
    }

    protected void PlayListFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        // Standard Query
        if (string.IsNullOrEmpty(PlaylistName.Text))
        {
            // Put out an error message
            // This form uses a User Control called MessageUserControl
            // This User Control will be the mechanism to display messages on this form.
            MessageUserControl.ShowInfo("Warning", "Playlist Name is required.");
        }
        else
        {
            // MessageUserControl has the try/catch coding embedded in the control.
            MessageUserControl.TryRun(() =>
            {
                // This is the process coding block to be executed under the "Watchful eye" of the MessageUserControl

                // Obtain the username from the security part of the application.
                string username = User.Identity.Name;
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                List<UserPlaylistTrack> retrievedPlaylist = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);

                PlayList.DataSource = retrievedPlaylist;
                PlayList.DataBind();
            },"Success","Here is your current playlist");
        }
    }

    protected void TracksSelectionList_ItemCommand(object sender, 
        ListViewCommandEventArgs e)
    {
        // code to go here
        // ListViewCancelEventArgs parameter contains the CommandArg value
        if (string.IsNullOrEmpty(PlaylistName.Text))
        {
            // Put out an error message
            MessageUserControl.ShowInfo("Warning", "Playlist Name is required.");
        }
        else
        {
            string username = User.Identity.Name;

            // TrackID is going to come from e.CommandArgument
            // e.CommandArgument is an object therefore it needs to be converted to String, then parsed.
            int trackID = int.Parse(e.CommandArgument.ToString());

            // The following code calls a BLL method to add to the database.
            MessageUserControl.TryRun(() =>
            {
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                List<UserPlaylistTrack> refreshResults = sysmgr.Add_TrackToPLaylist(PlaylistName.Text, username, trackID);
                PlayList.DataSource = refreshResults;
                PlayList.DataBind();
            }, "Success", "Track added to Playlist");
        }

    }

    protected void MoveUp_Click(object sender, EventArgs e)
    {
        // Playlist fetched
        if (PlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("Warning","No playlist has been retrieved");
        }
        else
        {
            // Playlist name present
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Warning", "No playlist name has been supplied");
            }
            else
            {
                // Check only one row selected
                int trackID = 0;
                int trackNumber = 0; // Optional
                int rowSelected = 0; // Search flag

                // Create a pointer to use for the access of the gridview control
                CheckBox playlistSelection = null;

                // Traverse the GridView checking each row for a checked checkbox
                for (int i = 0; i < PlayList.Rows.Count; i++)
                {
                    // Find the checkbox on the indexed gridview row
                    // PlaylistSelection will point to the checkbox
                    playlistSelection = PlayList.Rows[i].FindControl("Selected") as CheckBox;
                    // If is checked
                    if (playlistSelection.Checked)
                    {
                        trackID = int.Parse((PlayList.Rows[i].FindControl("TrackID") as Label).Text);
                        trackNumber = int.Parse((PlayList.Rows[i].FindControl("TrackNumber") as Label).Text);
                        rowSelected++;
                    }
                }

                if (rowSelected != 1)
                {
                    MessageUserControl.ShowInfo("Warning", "Exactly one track musttt be selected");
                }
                else
                {
                    if (trackNumber == 1)
                    {
                        MessageUserControl.ShowInfo("Information", "Track is in top position and can not be moved up");
                    }
                    else
                    {
                        // At this point we have PlayListName, Username, TrackID, which is needed to move the track

                        // Move track via BLL
                        MoveTrack(trackID, trackNumber, "up");
                    }
                }
            }
        }
    }

    protected void MoveDown_Click(object sender, EventArgs e)
    {
        // Playlist fetched
        if (PlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("Warning", "No playlist has been retrieved");
        }
        else
        {
            // Playlist name present
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Warning", "No playlist name has been supplied");
            }
            else
            {
                // Check only one row selected
                int trackID = 0;
                int trackNumber = 0; // Optional
                int rowSelected = 0; // Search flag

                // Create a pointer to use for the access of the gridview control
                CheckBox playlistSelection = null;

                // Traverse the GridView checking each row for a checked checkbox
                for (int i = 0; i < PlayList.Rows.Count; i++)
                {
                    // Find the checkbox on the indexed gridview row
                    // PlaylistSelection will point to the checkbox
                    playlistSelection = PlayList.Rows[i].FindControl("Selected") as CheckBox;
                    // If is checked
                    if (playlistSelection.Checked)
                    {
                        trackID = int.Parse((PlayList.Rows[i].FindControl("TrackID") as Label).Text);
                        trackNumber = int.Parse((PlayList.Rows[i].FindControl("TrackNumber") as Label).Text);
                        rowSelected++;
                    }
                }

                if (rowSelected != 1)
                {
                    MessageUserControl.ShowInfo("Warning", "Exactly one track musttt be selected");
                }
                else
                {
                    if (trackNumber == PlayList.Rows.Count)
                    {
                        MessageUserControl.ShowInfo("Information", "Track is in bottom position and can not be moved down");
                    }
                    else
                    {
                        // At this point we have PlayListName, Username, TrackID, which is needed to move the track
                        // Move track via BLL
                        MoveTrack(trackID, trackNumber, "down");
                    }
                }
            }
        }
    }
    protected void MoveTrack(int trackid, int tracknumber, string direction)
    {
        MessageUserControl.TryRun(() => 
        {
            // Standard call to a BLL method
            PlaylistTracksController sysmgr = new PlaylistTracksController();
            sysmgr.MoveTrack(User.Identity.Name, PlaylistName.Text, trackid, tracknumber, direction);

            // Refresh the list
            List<UserPlaylistTrack> refreshResults = sysmgr.List_TracksForPlaylist(PlaylistName.Text, User.Identity.Name);
            PlayList.DataSource = refreshResults;
            PlayList.DataBind();
        }, "Success", "Track moved");
    }
    protected void DeleteTrack_Click(object sender, EventArgs e)
    {
        //code to go here
    }
}
