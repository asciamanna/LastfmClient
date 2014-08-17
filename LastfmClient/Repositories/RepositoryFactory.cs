using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Repositories {
  public interface IRepositoryFactory {
    ILibraryRepository CreateLibraryTrackRepository(string apiKey);
    ILibraryRepository CreateLibraryAlbumRepository(string apiKey);
    IUserRepository CreateUserRecentTrackRepository(string apiKey);
    IUserRepository CreateUserTopArtistRepository(string apiKey);
  }
  public class RepositoryFactory : IRepositoryFactory {
    public ILibraryRepository CreateLibraryTrackRepository(string apiKey) {
      return new LibraryTrackRepository(apiKey);
    }

    public ILibraryRepository CreateLibraryAlbumRepository(string apiKey) {
      return new LibraryAlbumRepository(apiKey);
    }

    public IUserRepository CreateUserRecentTrackRepository(string apiKey) {
      return new UserRecentTrackRepository(apiKey);
    }

    public IUserRepository CreateUserTopArtistRepository(string apiKey) {
      return new UserTopArtistRepository(apiKey);
    }
  }
}
