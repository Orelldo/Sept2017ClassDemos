using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Chinook.Data.Entities;
using Chinook.Data.DTOs;
using Chinook.Data.POCOs;
using ChinookSystem.DAL;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookContext())
            {
                // What would happen if there is no match for the incoming parameter values
                // We need to ensure that results has a valid value, this value will be an IEnumerable<T> collection or it should be null
                // To ensure that results does end up with a valid value, use the .FirstOrDefault()
                var results = (from x in context.Playlists
                               where x.UserName.Equals(username)
                                   && x.Name.Equals(playlistname)
                               select x).FirstOrDefault();

                var theTracks = from x in context.PlaylistTracks
                                where x.PlaylistId.Equals(results.PlaylistId)
                                orderby x.TrackNumber
                                select new UserPlaylistTrack
                                {
                                    TrackID = x.TrackId,
                                    TrackNumber = x.TrackNumber,
                                    TrackName = x.Track.Name,
                                    Milliseconds = x.Track.Milliseconds,
                                    UnitPrice = x.Track.UnitPrice
                                };

                return theTracks.ToList();
            }
        }//eom

        public List<UserPlaylistTrack> Add_TrackToPLaylist(string playlistname, string username, int trackID)
        {
            using (var context = new ChinookContext())
            {
                // Part 1: Handle Playlist Record
                #region Playlist Record
                // Query for playlistID
                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username)
                                  && x.Name.Equals(playlistname)
                              select x).FirstOrDefault();
                // Initialize the trackNumber for the track going into PlaylistTracks
                int trackNumber = 0;
                // I will need to creat an instanse of PlaylistTrack
                PlaylistTrack newTrack = null;

                // Determine if this is an addition to an existing playlist or if it is a new playlist
                if (exists == null)
                {
                    // This is a new playlist
                    exists = new Playlist();
                    exists.Name = playlistname;
                    exists.UserName = username;
                    exists = context.Playlists.Add(exists);
                    trackNumber = 1;
                }
                else
                {
                    // Playlist already exists, must find the new TrackNumber
                    // TrackNumber will be equal to .Count()+1
                    trackNumber = exists.PlaylistTracks.Count() + 1;

                    // In our example, tracks only exist once on each playlist
                    newTrack = exists.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackID);

                    // This will be null if the track is NOT on the playlist tracks
                    if (newTrack != null)
                    {
                        throw new Exception("Playlist already contains this track");
                    }
                }
                #endregion

                // Part 2: Handle the track for PlaylistTrack
                #region PlaylistTrack
                // Use navigation to .Add the new track to the PlaylistTrack
                newTrack = new PlaylistTrack();
                newTrack.TrackId = trackID;
                newTrack.TrackNumber = trackNumber;

                // NOTE: The pKey for PlaylistID may not yet exist, using navigation one can let HashSet handle the PlayListID pKey
                exists.PlaylistTracks.Add(newTrack);

                // Physically commit work to the database
                context.SaveChanges();
                #endregion

                return List_TracksForPlaylist(playlistname, username);
            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookContext())
            {
                // Query for playlistID
                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username)
                                  && x.Name.Equals(playlistname)
                              select x).FirstOrDefault();



                // Determine if this is an addition to an existing playlist or if it is a new playlist
                if (exists == null)
                {
                    throw new Exception("Playlist has been removed");
                }
                else
                {
                    // Limit search to particular playlist
                    PlaylistTrack moveTrack = (from x in exists.PlaylistTracks
                                               where x.TrackId == trackid
                                               select x).FirstOrDefault();
                    if (moveTrack == null)
                    {
                        throw new Exception("Playlist track has been removed");
                    }
                    else
                    {
                        PlaylistTrack otherTrack = null;

                        #region Move UP
                        if (direction.Equals("up".ToUpper()))
                        {
                            // Doing another (kind of pointless) test to see the position of the item
                            if (moveTrack.TrackNumber == 1)
                            {
                                throw new Exception("Track is in top position and can not be moved up");
                            }
                            else
                            {
                                // Get the other track
                                otherTrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == moveTrack.TrackNumber - 1
                                              select x).FirstOrDefault();

                                if (otherTrack == null)
                                {
                                    throw new Exception("Playlist track cannot be moved up");
                                }
                                else
                                {
                                    moveTrack.TrackNumber--;
                                    otherTrack.TrackNumber++;
                                }
                            }
                        }
                        #endregion

                        #region Move DOWN
                        else
                        {
                            // Doing another (kind of pointless) test to see the position of the item
                            if (moveTrack.TrackNumber == exists.PlaylistTracks.Count)
                            {
                                throw new Exception("Track is in bottom position and can not be moved down");
                            }
                            else
                            {
                                // Get the other track
                                otherTrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == moveTrack.TrackNumber + 1
                                              select x).FirstOrDefault();

                                if (otherTrack == null)
                                {
                                    throw new Exception("Playlist track cannot be moved down");
                                }
                                else
                                {
                                    moveTrack.TrackNumber++;
                                    otherTrack.TrackNumber--;
                                }
                            }
                        }
                        #endregion
                        //eo up/down

                        // Stage changes for Save Changes()
                        // Indicate only the fied that needs to be updated
                        context.Entry(moveTrack).Property(y => y.TrackNumber).IsModified = true;
                        context.Entry(otherTrack).Property(y => y.TrackNumber).IsModified = true;

                        context.SaveChanges();
                    }
                }
            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookContext())
            {
                //code to go here


            }
        }//eom
    }
}
