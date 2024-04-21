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

