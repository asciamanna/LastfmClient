using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public abstract class LastfmLibraryItem {
    public string Name;
    public int PlayCount;
    public string Artist;
  }
}
