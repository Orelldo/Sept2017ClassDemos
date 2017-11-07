using Chinook.Data.DTOs;
using Chinook.Data.POCOs;
using ChinookSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.BLL
{
    [DataObject]
    public class GenreController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<GenreDTO> Genre_GenreAlbumTracks()
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Genres
                              select new GenreDTO
                              {
                                  genre = x.Name,
                                  albums = from y in x.Tracks
                                           group y by y.Album into gResults
                                           select new AlbumDTO
                                           {
                                               title = gResults.Key.Title,
                                               releaseYear = gResults.Key.ReleaseYear,
                                               numberOfTracks = gResults.Count(),
                                               tracks = from z in gResults
                                                        select new TrackPOCO
                                                        {
                                                            song = z.Name,
                                                            milliseconds = z.Milliseconds
                                                        }
                                           }
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> List_GenreNames()
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Genres
                              orderby x.Name
                              select new SelectionList
                              {
                                  IDValueField = x.GenreId,
                                  DisplayText = x.Name
                              };
                return results.ToList();
            }
        }
    }
}
