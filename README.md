### Note 
I've copied the project over as-is. There is no .gitignore right now, so there are a lot of strange files littered throughout the project that need cleaning up.

## Readme
 
This was the old readme I left myself about 3 years ago, when I decided to try and start cleaning up the original code that I started about 5 or so years ago! (I have come a long way coding-wise since then)

MonoGameTest is the old version of the game as a Mono Project (Open Source cross-platform of XNA 4.0)
WindowsGame1 is the old version of the game (In XNA 4.0)
GameData (Sprite sheets, xml files and shaders) are in ContentXNA\ContentXNAContent
GameName3 is the new version of the project, as it was being retructured. As I was developing the old version of the game, I realised that a lot of code was being reused (i.e. all the manager classes), and it quickly became overwhelming. I had learnt a lot more about how game engines were structured, and general OOP in this time, so I decided to rework the class structure of the engine - at the same time Microsoft abandoned XNA 4.0, so I moved to the Open-Source version, MonoGame. A lot of classes could be kept (especially on the rendering side), but I decided to utilise a component-entity system for all my game entities over the normal inheritance method. Because I was completely restructring the game, I haven't been able to test it for a long time. However, I did have a blog of my progress at http://outrageousbobiv.tumblr.com/, which shows off a few of the features that were created.
