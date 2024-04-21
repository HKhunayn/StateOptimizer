using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SOptimizer
{
    public class StateOptimizer : MonoBehaviour
    {

        private static StateOptimizer _instance;

        public static StateOptimizer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = SpawnPrefab();
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        [Tooltip("Will not be working in Editor if enabled")]
        [SerializeField] public bool _disableInEditor = false;
        [SerializeField] StateProfile _profile;

        private Vector2 _lastMousePos;
        private float _lastActiveTime;
        private FPSCounter _fpsCounter;
        private StateMode _currentMode;

        void Awake()
        {
            if (_instance != null)
                Destroy(this);
            else
            {
                if (StateOptimizerUtility.IsStateOptimizerPrefab(gameObject))
                {
                    // using the prefab
                    _fpsCounter = gameObject.GetComponent<FPSCounter>();
                    _fpsCounter.Init(_profile.CheckTime);
                    DontDestroyOnLoad(this);
                    _instance = this;
                    transform.name = "StateOptimizer";
                }
                else
                {
                    _instance = (SpawnPrefab(_profile != null ? _profile: null)) ;
                    Destroy(this);
                }

            }
        }

        IEnumerator Start()
        {

            if (!(_disableInEditor && Application.isEditor))
            {
                _lastMousePos = StateOptimizerUtility.GetMousePos();
                QualitySettings.vSyncCount = 0;
                Application.runInBackground = _profile.RunInBackground;
                while (true)
                {
                    if (_profile.EnableDynamicRenderScale && StateOptimizerUtility.IsURP())
                    {
                        float scale = Mathf.Clamp(_fpsCounter.GetFPSAVG() / Application.targetFrameRate, GetCurrentMode().RenderScale.x, GetCurrentMode().RenderScale.y);
                        DynamicRenderScale.SetRenderScale(scale);
                    }

                    yield return new WaitForSecondsRealtime(_profile.CheckTime);
                    MouseCheck();
                    if (_lastActiveTime + _profile.IdleTime <= Time.time)
                        ApplyMode(StateMode.Idle);
                }
            }
        }
        private void OnDisable()
        {
            if(_instance == this)
                _instance = null;
            if (transform.name.Contains("Clone"))
                Destroy(gameObject);
        }

        SMode GetSMode(StateMode state)
        {
            switch (state) 
            {
                case (StateMode.Active):
                    return _profile.ActiveMode;
                case (StateMode.Idle):
                    return _profile.IdleMode;
                case (StateMode.Sleep):
                    return _profile.SleepMode;
                default:
                    return _profile.ActiveMode;
            }

        }

        SMode GetCurrentMode()
        {
            return GetSMode(_currentMode);
        }

        // check weather the mouse pos changed or not
        private void MouseCheck()
        {
            Vector2 mousePos = StateOptimizerUtility.GetMousePos();

            // when mouse's pos change do the active mode 
            if (Vector2.Distance(mousePos, _lastMousePos) > _profile.AllowedPixelChange && Application.isFocused)
            {
                ApplyMode(StateMode.Active);
                _lastActiveTime = Time.time;
            }
            _lastMousePos = mousePos;
        }

        private void LateUpdate()
        {
            if (StateOptimizerUtility.IsAnyButtonPressed() && Application.isFocused)
                ApplyMode(StateMode.Active);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (_instance == null || Time.time < Time.deltaTime)
                return;

            if (_disableInEditor && Application.isEditor)
                return;
            if (focus)
                ApplyMode(StateMode.Active);
            else if (Application.isFocused)
                ApplyMode(StateMode.Idle);
            else
                ApplyMode(StateMode.Sleep);
        }

        void ApplyMode(StateMode SMode)
        {
            SMode mode = GetSMode(SMode);
            Application.targetFrameRate = mode.FPS == 0 ? Screen.currentResolution.refreshRate : mode.FPS;
            ScalableBufferManager.ResizeBuffers(mode.BufferManager, mode.BufferManager);
            #if UNITY_2020_3_OR_NEWER
            Physics.simulationMode = mode.PhysicsAutoSimulate ? SimulationMode.FixedUpdate : SimulationMode.Script;
            Physics2D.simulationMode = mode.PhysicsAutoSimulate ? SimulationMode2D.FixedUpdate : SimulationMode2D.Script;
            #else
            Physics.autoSimulation = Physics2D.autoSimulation = GetCurrentMode().PhysicsAutoSimulate;
            #endif
            Physics.defaultSolverIterations = Physics2D.velocityIterations = mode.PhysicsIterations;

            if (GetCurrentMode() != mode)
            {
                mode.ModeEvent.Invoke();
                _fpsCounter.ResetFPSList();
            }

            _currentMode = SMode;
        }

        private static StateOptimizer SpawnPrefab(StateProfile stateProfile = null)
        {
            StateOptimizer so = Instantiate((GameObject)Resources.Load("StateOptimizer")).GetComponent<StateOptimizer>();
            if (stateProfile != null)
                so._profile = stateProfile;
            return Instantiate((GameObject)Resources.Load("StateOptimizer")).GetComponent<StateOptimizer>();
        }

        #region Public static methods

        /// <summary>
        /// Set the mode for the optimizer
        /// </summary>
        /// <param name="mode"></param>
        public static void SetMode(StateMode mode)
        {
            Instance.ApplyMode(mode);
        }

        /// <summary>
        /// Get a current Mode of the optimizer
        /// </summary>
        /// <returns></returns>
        public static StateMode GetMode()
        {
            return Instance._currentMode;
        }

        /// <summary>
        /// the ModeEvent of specific mode
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="call"></param>
        /// <returns></returns>
        public static UnityEvent ModeEvent(StateMode mode)
        {
            return Instance.GetSMode(mode).ModeEvent;
        }

        #endregion

    }
}

