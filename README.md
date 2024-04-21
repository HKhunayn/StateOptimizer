![1 Asset store covers](https://github.com/HKhunayn/StateOptimizer/assets/102166198/781a23d4-bf0d-4b4f-800c-8a9acf311578)

A simple unity asset to use less CPU/GPU power by dynamically limiting some unity features when it is not needed.

 ## Screenshots
 | ![2 State](https://github.com/HKhunayn/StateOptimizer/assets/102166198/cc28f730-da3a-421b-a11f-35f3672533d1) |  ![3 profiles](https://github.com/HKhunayn/StateOptimizer/assets/102166198/7d2af779-e680-46eb-a409-c17aae63c530) |
| --- | --- |
| ![4 Dynamic render scale](https://github.com/HKhunayn/StateOptimizer/assets/102166198/d95f8f9d-2eee-4fb2-bc10-141f672abcbf) |  ![5 Customize](https://github.com/HKhunayn/StateOptimizer/assets/102166198/2a3104f6-06ce-4056-bc94-cf6f1efc7691) |

 ## How it works
 The idea is simple, The State Profile has 3 modes: Active, Idle, and Sleep. Each mode has 6 settings: FPS, buffer manager, physics auto simulate, physics iterations, render scale, and mode event. 
 
 
**Active mode:** called when the player is active (detected by keyboard/mouse input)

**Idle mode:** called when the player is not giving any input for 10 sec (can be changed)

**Sleep mode:** called when the player is outside/minimized the game/application!


 
 ## How to use
  There are 2 ways: 
| 1- Add the “StateOptimizer” prefab **OR** | 2- Add “StateOptimizer” script to any|
| --- | --- |
| ![how_to_add_1 1](https://github.com/HKhunayn/StateOptimizer/assets/102166198/f9ca1f42-d053-43e7-91b9-f04ed8af457c) ![how_to_add_1 2](https://github.com/HKhunayn/StateOptimizer/assets/102166198/891d032b-9243-4b5e-be44-341bdc0b26cd) | ![how_to_add_2 1](https://github.com/HKhunayn/StateOptimizer/assets/102166198/52d45f86-5abe-4071-8673-6bbeb3ded016) ![how_to_add_2 2](https://github.com/HKhunayn/StateOptimizer/assets/102166198/085c61e5-80d4-4a8e-8097-00e78f5040af) |

## Profiles 

There are 3 predefined profiles: Application Profile, Default Profile, and No Idle Profile 

**Default Profile:** Recommended for most games. 

**Application Profile:** Recommended for Application like games. 

**No Idle Profile:** with no idle state. 
 

 ## Custom profile
 To create your own: 
 ![add_profile](https://github.com/HKhunayn/StateOptimizer/assets/102166198/274f089f-a874-493e-aa8f-950b186f73cf)

 
Right click at project window then
###### `Create -> StateOptimizer -> StateProfile`
  


 ## CODE EXAMPLE 
While coding isn't required, this serves as an illustrative example.  


  ```CS
using UnityEngine;
using SOptimizer;


public class StateOptimizerExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get The current mode
        Debug.Log("current mode is " + StateOptimizer.GetMode());

        // Set The mode for the optimizer
        StateOptimizer.SetMode(StateMode.Active);

        // call a function when mode state change
        StateOptimizer.ModeEvent(StateMode.Idle).AddListener(IdleFunction);
    }

    void IdleFunction()
    {
        Debug.Log("its idle now");
    }
}

```

