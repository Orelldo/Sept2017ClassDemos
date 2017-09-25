﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Chinook.Data.Entities;
using ChinookSystem.DAL;
using System.ComponentModel;  //expose methods for ODS wizard
using Chinook.Data.POCOs;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ArtistAlbumByReleaseYear> Albums_ListforArtist(int artistid)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.ArtistId.Equals(artistid)
                              select new ArtistAlbumByReleaseYear
                              {
                                  Title = x.Title,
                                  Released = x.ReleaseYear
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Album> Albums_FindByYearRange(int minYear, int maxYear)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.ReleaseYear >= minYear && x.ReleaseYear <= maxYear
                              orderby x.ReleaseYear, x.Title
                              select x;
                return results.ToList();
            }
        }
    }
}
