﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public class LastfmLibraryAlbumResponse : LastfmResponse, ILastfmResponse {
    public List<LastfmLibraryAlbum> Albums = new List<LastfmLibraryAlbum>();
  }

  public class LastfmLibraryAlbum {
    public string Name;
    public string Artist;
    public int PlayCount;
    public string ArtworkLocation;
  }
}
