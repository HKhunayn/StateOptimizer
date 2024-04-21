using UnityEngine;

namespace SOptimizer
{
    [CreateAssetMenu(menuName = "StateOptimizer/StateProfile")]
    public class StateProfile : ScriptableObject
    {

        [Header("Common:")]
        [Min(0), Tooltip("Time of no active to be idle in seconds")]
        [SerializeField] public float IdleTime = 10f;
        [Min(0.1f), Tooltip("Time of checking in seconds")]
        [SerializeField] public float CheckTime = 0.2f;
        [Min(0), Tooltip("How many moving pixel needed for the mouse to active")]
        [SerializeField] public float AllowedPixelChange = 2f;
        [Tooltip("Application runs when it is in the background")]
        [SerializeField] public bool RunInBackground = false;
        [Tooltip("Change the RenderScale based on the modes (Only for URP)")]
        [SerializeField] public bool EnableDynamicRenderScale = true;

        [Header("Modes:")]
        [Tooltip("Active mode is the Defualt Mode")]
        [SerializeField] public SMode ActiveMode = new SMode(fps: 0, bufferManager: 1, physicsAutoSimulate: true, physicsIterations: 4);
        [Tooltip("Idle mode is called when the player is idle")]
        [SerializeField] public SMode IdleMode = new SMode(fps: 30, bufferManager: 0.5f, physicsAutoSimulate: true, physicsIterations: 2, renderScaleMin: 0.5f, renderScaleMax: 0.7f);
        [Tooltip("Sleep mode is called when the player minimize/not fouced it the window of the game")]
        [SerializeField] public SMode SleepMode = new SMode(fps: 5, bufferManager: 0.1f, physicsAutoSimulate: false, physicsIterations: 1, renderScaleMin: 0.1f, renderScaleMax: 0.2f);
    }
}
