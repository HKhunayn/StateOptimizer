using System;
using UnityEngine;
using UnityEngine.Events;
namespace SOptimizer
{
    [Serializable]
    public class SMode
    {
        [Min(0), Tooltip("How many frame per second for the mode \n(0 means current refreshRate)")]
        [SerializeField] public int FPS = 0;
        [Range(0.1f, 1f), Tooltip("Change the BufferManager scale")]
        [SerializeField] public float BufferManager = 1f;
        [Tooltip("AutoSimulate when the mode enabled \n(false means the physics will be stopped)")]
        [SerializeField] public bool PhysicsAutoSimulate = true;
        [Range(1, 255), Tooltip("Less iterations means less physics accurecy \n(this is a 3dSolver / 2dVelocity iterations)")]
        [SerializeField] public int PhysicsIterations = 4;
        [Tooltip("Min and Max scale factor for render (less means lower power usage and higher FPS)")]
        [SerializeField] public Vector2 RenderScale = new Vector2(0.8f, 1f);

        public UnityEvent ModeEvent;

        public SMode(int fps = 0, float bufferManager = 1, bool physicsAutoSimulate = true, int physicsIterations = 4, float renderScaleMin = 0.7f, float renderScaleMax = 1f)
        {
            FPS = fps;
            BufferManager = bufferManager;
            PhysicsAutoSimulate = physicsAutoSimulate;
            PhysicsIterations = physicsIterations;
            RenderScale = new Vector2(Mathf.Max(0.1f, renderScaleMin), Mathf.Min(2, renderScaleMax));

            ModeEvent = new UnityEvent();
        }
    }
}
