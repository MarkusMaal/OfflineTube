# OfflineTube
This is a video browser application written in C# WinForms for Windows. Designed to play videos downloaded using JDownloader from YouTube offline. Just point to a folder with downloaded videos and begin.  
The bin folder contains precompiled binaries you can use.
> Note: The WMPLIB dynamic link libraries are required for the program to run

## File descriptions
* bin - Required binaries  
  * Release - binaries with no debugging support  
    * AxInterop.WMPLib.dll - Ax Windows Media Player libraries  
    * Interop.WMPLib.dll - Ax Windows Media Player libraries  
* Properties
  * AssemblyInfo.cs - Final \*.exe file details
  * Resources.Designer.cs - Metadata for resources, generated using a tool
  * Resources.resx - Real metadata for resources
  * Settings.Designer.cs
  * Settings.settings
* Resources - contains images used by the program
  * media_fullscreen.png - Small icon for fullscreening the current video
  * ot_random.png - Large icon for choosing a random video file
  * ots_docs.png - Small icon for opening the folder, specified video file is located at
  * ots_docs1.png - Read above
  * ots_play.png - Small icon for opening the video file using an external video player
  * ots_thumbnail.png - Small icon for opening the thumbnail file using an external image viewer
  * ots_videopage.png - Small icon for opening the video using the internal video form
  * ott_back.pn.png - Tiny icon for closing the video page
  * ott_random.png - Tiny icon for opening a random video
  * playbutton.png - Play button overlay

DescView.cs - Form that shows the description of the selected video file  
\*.Designer.cs - Form design documents  
\*.resx - Form resource documents  
Form1.cs - First form, which is shown when no repository is specified. You can also trigger this dialog within the video browser using the "Change repository" button  
OfflineTube.csproj - Project file  
OfflineTube.sln - Solution file (open this using Visual Studio to design and/or compile)  
offlinetube_PBE_icon.ico - Icon file for the OfflineTube program  
Program.cs - Main code of the program, launches the initial form and allows the other forms to communicate with each other  
VideoBrowser.cs - Code for the form that is shown when a repository is specified  
VideoPage.cs - Code for the form that is shown when an internal video player is used  
