using System;
using System.Collections;
using UnityEngine;

public class PeformenceManger : MonoBehaviour
{

    [Header("Common:")]
    [Min(0), Tooltip("Time of no active to be idle in seconds")]
    [SerializeField] float idleTime = 10f;
    [Min(0.1f), Tooltip("Time of checking in second")]
    [SerializeField] float checkTime = 1f;
    [Min(0), Tooltip("How many moving pixel needed for the mouse to active")]
    [SerializeField] float allowedPixelChange = 2f;

    [Header("Active Mode:")]
    [Min(0), Tooltip("How many frame per second for not Active mode \n(0 for current resolution max)")]
    [SerializeField] int activeFPS = 0;
    [Range(0.1f,1f),Tooltip("change the bufferManger scale")]
    [SerializeField] float activeBufferManger = 1f;
    [Tooltip("AutoSimulate when the active mode enabled \n(false means the physics will be stopped)")]
    [SerializeField] bool activePhysicsAutoSimulate = true;
    [Range(1, 255), Tooltip("less iterations means less physics accurecy \n(this is a 3dsolver / 2dVelocity iterations)")]
    [SerializeField] int activePhysicsIterations = 4;

    [Header("Idle Mode:")]
    [Min(0), Tooltip("How many frame per second for idle mode \n(0 for current resolution max)")]
    [SerializeField] int idleFPS = 30;
    [Range(0.1f, 1f), Tooltip("change the bufferManger scale")]
    [SerializeField] float idleBufferManger = 0.5f;
    [Tooltip("AutoSimulate when the idle mode enabled \n(false means the physics will be stopped)")]
    [SerializeField] bool idlePhysicsAutoSimulate = true;
    [Range(1,255),Tooltip("less iterations means less physics accurecy \n(this is a 3dsolver / 2dVelocity iterations)")]
    [SerializeField] int idlePhysicsIterations = 2;

    [Header("Not Focused Mode:")]
    [Min(0), Tooltip("How many frame per second for not focused mode \n(0 for current resolution max)")]
    [SerializeField] int notFocusedFPS = 5;
    [Range(0.1f, 1f), Tooltip("change the bufferManger scale")]
    [SerializeField] float notFocusedBufferManger = 0.1f;
    [Tooltip("AutoSimulate when the not focused mode enabled \n(false means the physics will be stopped)")]
    [SerializeField] bool notFocusedPhysicsAutoSimulate = false;
    [Range(1, 255), Tooltip("less iterations means less physics accurecy \n(this is a 3dsolver / 2dVelocity iterations)")]
    [SerializeField] int notFocusedPhysicsIterations = 1;

    private Vector2 lastPos;
    private float lastActive = 0;

    IEnumerator Start()
    {
        lastPos = Input.mousePosition;
        QualitySettings.vSyncCount = 0;
        while (true)
        {
            yield return new WaitForSecondsRealtime(checkTime);
            isMouseChanged();
            if (lastActive + idleTime <= Time.time)
            {
                doIdle();
            }
        }
    }

    private void isMouseChanged() { // check weather the mouse changed or not
        if ((Mathf.Abs(lastPos.x - Input.mousePosition.x) > allowedPixelChange || Mathf.Abs(lastPos.y - Input.mousePosition.y) > allowedPixelChange) && Application.isFocused)
        {
            doActive();
        }
        lastPos = Input.mousePosition;
    }

    private void LateUpdate()
    {
        if (Input.anyKey && Application.isFocused) // logic to set Active
            doActive();

    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            doActive();
        else 

            doIdle();
    }

    private void doActive()// Active state
    {
        lastActive = Time.time;
        Application.targetFrameRate = activeFPS == 0? Screen.currentResolution.refreshRate : activeFPS;
        ScalableBufferManager.ResizeBuffers(activeBufferManger, activeBufferManger);
        Physics.autoSimulation = Physics2D.autoSimulation = activePhysicsAutoSimulate;
        Physics.defaultSolverIterations = Physics2D.velocityIterations = activePhysicsIterations;
    }

    private void doIdle() // idle/not fouced state
    {
        GC.Collect();
        if (Application.isFocused) // idle state
        {
            Application.targetFrameRate = idleFPS == 0 ? Screen.currentResolution.refreshRate : idleFPS;
            ScalableBufferManager.ResizeBuffers(idleBufferManger, idleBufferManger);
            Physics.autoSimulation = Physics2D.autoSimulation = idlePhysicsAutoSimulate;
            Physics.defaultSolverIterations = Physics2D.velocityIterations = idlePhysicsIterations;
        }

        else { // not fouced state
            Application.targetFrameRate = notFocusedFPS == 0 ? Screen.currentResolution.refreshRate : notFocusedFPS;
            ScalableBufferManager.ResizeBuffers(notFocusedBufferManger, notFocusedBufferManger);
            Physics.autoSimulation = Physics2D.autoSimulation = notFocusedPhysicsAutoSimulate;
            Physics.defaultSolverIterations = Physics2D.velocityIterations = notFocusedPhysicsIterations;
        }
            
    }
 
}
