#Last.fm Client [![Build status](https://ci.appveyor.com/api/projects/status/e703ayk1nydyngqm?svg=true)](https://ci.appveyor.com/project/asciamanna/lastfmclient)

A .NET client for the Last.fm REST API.


##Currently Supported
These are the parts of the Last.FM REST API that are currently implemented:  

* [library.getTracks](http://www.last.fm/api/show/library.getTracks "library.getTracks")
* [library.getAlbums](http://www.last.fm/api/show/library.getAlbums "library.getAlbums")
* [user.getRecentTracks](http://www.last.fm/api/show/user.getRecentTracks "user.getRecentTracks")
* [user.getTopArtists](http://http://www.last.fm/api/show/user.getTopArtists "user.getTopArtists")
* [album.getInfo](http://www.last.fm/api/show/album.getInfo  "album.getInfo")
* [artist.getInfo](http://www.last.fm/api/show/artist.getInfo "artist.getInfo")
* Currently Playing from Service information for a user (scrapes last.fm user page)

##Coming Soon
* Parse additional artist data from [artist.getInfo](http://www.last.fm/api/show/artist.getInfo "artist.getInfo") including band members and a list of formation years 
* Page the library API method (getTracks & getAlbums) results

## Exceptions
There are three types of exceptions you can expect to be thrown from the last.fm client:  
1. **ArgumentException** - Thrown if an API key is not specified  
2. **WebException** - Thrown by the .NET WebClient if it cannot make a connection to the last.fm REST services   
3. **LastfmException** - A custom exception thrown if a last.fm service returns a failed lfm status. It includes the error code and the message returned by the last.fm service.  

##End-to-End Tests
There are a few XUnit style end to end tests in the test project. These tests will call the last.fm services and verify that results are being returned. They are marked as _Explicit_ so that they are isolated from the actual unit tests (in the same project).

##Example Usage
For examples of how to use the library take a look at the End-To-End tests previously mentioned. They can be found in the LastfmServiceEndToEndTest.cs file. Each test case is an
example of how to call each of the exposed Lastfm service methods.

##Suggestions
The last.fm API is quite large and I'm barely scratching the surface of it, implementing the parts of the API that I am currently using. 
<p>
If you have any requests for parts of the API that you would like implemented drop me a line or go ahead and fork and send pull requests.
</p>
##Project Dependencies
HtmlAgilityPack 1.4.8  
NUnit 2.6.3  
RhinoMocks 3.6.1

##Contact
**Anthony Sciamanna**
<br/>
**Email:** asciamanna@gmail.com  
**Web:** [http://www.anthonysciamanna.com](http://www.anthonysciamanna.com)  
**Twitter:** [@asciamanna](http://www.twitter.com/asciamanna)


