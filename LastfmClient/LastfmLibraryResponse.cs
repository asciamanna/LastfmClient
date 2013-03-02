using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient {
  public abstract class LastfmLibraryResponse {
    public string Status;
    public int Page = 0;
    public int PerPage = 0;
    public int TotalPages = 0;
    public int TotalRecords = 0;
  }
}
