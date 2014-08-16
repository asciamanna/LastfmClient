using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Repositories {
  public interface IRepositoryFactory {
    ILibraryRepository CreateLibraryTracksRepository(string apiKey);
    ILibraryRepository CreateLibraryAlbumsRepository(string apiKey);
  }
  public class RepositoryFactory : IRepositoryFactory {
    public ILibraryRepository CreateLibraryTracksRepository(string apiKey) {
      return new LibraryTrackRepository(apiKey);
    }

    public ILibraryRepository CreateLibraryAlbumsRepository(string apiKey) {
      return new LibraryAlbumRepository(apiKey);
    }
  }
}
