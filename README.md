# PeformenceManger
 a simple unity script to use less CPU/GPU power by dynamically limiting some unity features when it is not needed.

 ## How it works
  the idea is very simple, we have 3 modes Active, Idle, and notFouced, and for each mode have 4 settings. FPS, bufferManger, physicsAutoSimulate, and physicsIterations

  **Active mode:** called when the player is active (detected by keyboard/mouse input)

  **Idle mode:** called when the player is not giving any input for 10 sec (can be changed)

  **notFouced mode:** called when the player is outside/minimized the game/application
 
 ## How to use
  simply download the script (PeformenceManger.cs file) then after importing it to your project, put it in active gameObject.
 
 ## Application unity-based recommended settings 
  ![recommendedSettings](https://user-images.githubusercontent.com/102166198/223015254-79298f8b-0651-4f8a-9d0a-431519b682f6.png)

 ## Known Issue
  when your game/application is in notFouced mode and you want to change the window location, it is updating the location every notFoucedFPS. To fix it you have 2 options:

  1) disable the Run in Background option in your project from:
	   ###### `Project Settings -> Player -> Resolution and Presentation -> Run in Background`
 
  2) increase notFoucedFPS

