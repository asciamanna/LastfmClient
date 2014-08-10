﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public class LastfmUserRecentTrack : LastfmUserItem {
    public bool IsNowPlaying = false;
    public string Artist;
    public string Album;
    public DateTime? LastPlayed;
  }
}
