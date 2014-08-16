using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Repositories {
  public interface IRepositoryFactory {
    ILibraryRepository CreateLibraryTrackRepository(string apiKey);
    ILibraryRepository CreateLibraryAlbumRepository(string apiKey);
  }
  public class RepositoryFactory : IRepositoryFactory {
    public ILibraryRepository CreateLibraryTrackRepository(string apiKey) {
      return new LibraryTrackRepository(apiKey);
    }

    public ILibraryRepository CreateLibraryAlbumRepository(string apiKey) {
      return new LibraryAlbumRepository(apiKey);
    }
  }
}
