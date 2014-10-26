#Last.fm Client

A .NET client for the Last.FM REST API.
## Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/e703ayk1nydyngqm)](https://ci.appveyor.com/project/asciamanna/lastfmclient)

##Currently Supported
These are the parts of the Last.FM REST API that are currently implemented:  

* [library.getTracks](http://www.last.fm/api/show/library.getTracks "library.getTracks")
* [library.getAlbums](http://www.last.fm/api/show/library.getAlbums "library.getAlbums")
* [user.getRecentTracks](http://www.last.fm/api/show/user.getRecentTracks "user.getRecentTracks")
* [user.getTopArtists](http://http://www.last.fm/api/show/user.getTopArtists "user.getTopArtists")
* [album.getInfo](http://www.last.fm/api/show/album.getInfo  "album.getInfo")
* Currently Playing from Service information for a user (scrapes last.fm user page)

##Coming Soon
* Get artist summary from [artist.getInfo](http://www.last.fm/api/show/artist.getInfo "artist.getInfo") 
* Page the library API method (getTracks & getAlbums) results
* Console Application to run end-to-end testing of the client
* Example usage of the client

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


