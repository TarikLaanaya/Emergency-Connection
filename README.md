# Emergency Connection

## Description

Emergency Connection is a puzzle game based on the connection based mini games from popular titles like Bioshock. Navigate wires around obstacles making sure they dont intertwine, connecting various modules to perform different tasks such as rotate solar pannels or redirect energy flow. The original idea was a narrative based around the final days of the mars rover, desperately trying to fix itself before its inevitable end.

## Features

All scripts can be found in `Emergency-Connection/Assets/Scripts/`

This game was a massive challenge for me so far. There were a lot of concepts I had to figure my way around specifically to do with quaternions and how I could use them within unity to create the desired effect. What I was trying to achieve was to create wires that would wrap around obstacles but also unwrap. A good way to visualise this would be a pin board, and imagine you were wrapping/unwrapping string around multiple pins. For this prototype there is only one very important script which handles this problem:
  
`WireController` is the main script and it is what decides wether or not a wire is wrapped around an obstacles corner. There are various complicated systems within this script so I will try my best to explain. I use a line renderer to create the effect of the wires. With line renderers you can split them into multiple vertices. You can then control the location of said vertices. These vertices are organised as -1 = 1st vertice -2 = 2nd and so on. The first would always be on the players finger and the second would be on either the last obstacle corner or the wire spawner. In the update I raycast from the first and second vertices to see if there is a obstacle in the way. If so we check the closest corner on that obstacle from the point that the raycast hit. Then we add a vertice to the wire (line renderer) and set the second position to the corner. This simulates the effect of wrapping. There are other smaller systems I had to put in place to make this function smoothly however for now I will move on.

The second most important part of this script was unwrapping. This was also the hardest and took the most time and still needs further work. Firstly I check the angle between the first and second vertices and the second and third vertices. when this angle is completley flat (180 degrees) then the wire should unwrap. The way I check this provides a result between -180 and 180. This was the best method I found would work. I then check wether the angle was positive. If the angle is positive I check for when it is less than -160. If it is negative I check for when the angle is greater than 160. This method provided decent results but overall did not work. I had a bug and I struggled for a while trying to figure out what was causing it. The bug would create multiple vertices on a corner when it shouldnt. This would only happen in very select cases. For now my solution was to check the distance between the 3rd and 2nd vertices on the wire. If they were too close then the bug had occured and I would run the remove point method. For now this provides mostly positive results. However it is a work around and as such it provides inconsistency in gameplay.

## Viewing The Project

If you would like to see the project and how this prototype functions within unity follow these steps:

1. Install Unity Hub from the official [Unity Website](https://unity.com/download), the version this project uses is: 2021.3.4f1.
2. Press the green "code" button located on this repositories home page, from the drop down click download zip.
3. Extract the downloaded file (right click > extract).
4. Open the Unity Hub application and press add located at the top right. Find your extracted folder.
5. The Unity project named "Emergency Connection" should appear. Click on it.
6. When the application opens the play button is located at the very top.

Notes: To play the game you either need to connect your mobile device, tutorials for which can be found [here](https://www.youtube.com/watch?v=J2T92mzvmHk&ab_channel=DigieraGames)
or you can use the built in Device Simulator tutorials for which are found [here](https://www.youtube.com/watch?v=dI1IEajUg_Y&ab_channel=JehoshaphatAbu)
