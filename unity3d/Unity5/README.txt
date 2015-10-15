
Getting Data from Motiv in Unity3d 5.2.1f1 for windows 8.1

1. Download the github repository and go to the folder: MAPLAB_natnet-unity3d-bridge\UnitySetup\Executable Server Unity

2. Setting up Unity
In the Unity5Scripts folder you will find two scripts, Body.cs and SlipStream.cs. Attach the Body script to an empty gameObject in your Unity Scene.
You can create your own prefabs for the tracked objects by dragging your prefab in the "Prefab Block" field of the Body script in the inspector. 
The SlipStream script should be added to the same gameObject as the Body script is attached to, or you can attach it to another gameobject and drag that gameobject into the "Slip Stream Object" field of the Body.cs script.

3. Connecting to the server from Unity
In the folder: MAPLAB_natnet-unity3d-bridge\UnitySetup\Executable Server Unity
There are two files, one is a .bat, the other .exe, run the .bat, put these in a folder inside Unity called 'Server'
Then attach the Body.cs to a gameObject and input your own ip in the inspector, if you run the game at this point you should be getting data streamed to your computer, if it does not work try turning off the windows firewall.


Alternatively if connecting from Unity does not work:
Go to the fodler named: MAPLAB_natnet-unity3d-bridge\UnitySetup\Executable Server Unity
There are two files, here one is a .bat, the other .exe, save both files in the same folder somewhere on your computer and run the .bat and it will ask for the server's ip(10.200.200.14 for MapLab). 
Then input your own ip and press enter, at this point you should be getting data streamed to your computer, if it does not work try turning off the windows firewall.