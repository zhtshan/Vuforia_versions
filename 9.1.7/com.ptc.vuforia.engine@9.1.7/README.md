# Vuforia Engine Package for Unity

*Copyright (c) 2010-2019 PTC Inc.  
All Rights Reserved.*

Vuforia Engine is the most widely used platform for AR development, with support for leading phones, tablets, and eyewear. Developers can easily add advanced computer vision functionality to Android, iOS, and UWP apps, to create AR experiences that realistically interact with objects and the environment.

To learn more about Vuforia Engine, visit [https://library.vuforia.com/articles/Training/getting-started-with-vuforia-in-unity.html](https://library.vuforia.com/articles/Training/getting-started-with-vuforia-in-unity.html)  
To view the Vuforia Developer Agreement, go to: [https://developer.vuforia.com/legal/vuforia-developer-agreement](https://developer.vuforia.com/legal/vuforia-developer-agreement)  
To view the release notes, go to: [https://library.vuforia.com/articles/Release\_Notes.html](https://library.vuforia.com/articles/Release_Notes.html)

## Using this package

To make this package available to the Unity package manager, a scoped registry needs to be added to your Unity project.
This can be done by importing the Unity editor script available here: [https://developer.vuforia.com/downloads/sdk](https://developer.vuforia.com/downloads/sdk)

As an alternative, you can directly add the scoped registry to your project by editing the *manifest.json* in your project’s *Packages* folder, adding an entry for “scopedRegistries”: 

    {
      "scopedRegistries": [ 
        { 
          "name": "Vuforia", 
          "url": " https://registry.packages.developer.vuforia.com/", 
          "scopes": [ 
            "com.ptc.vuforia" 
          ] 
        } 
      ], 
      "dependencies": { 
        ...
      } 
    } 

Once the scoped registry has been added, the latest version of the Vuforia Engine package is available in the Unity Package Manager UI.