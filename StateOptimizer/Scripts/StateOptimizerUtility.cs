using UnityEngine;
using UnityEngine.Rendering;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
namespace SOptimizer
{
    public static class StateOptimizerUtility
    {

        public static Vector3 GetMousePos()
        {
            #if ENABLE_LEGACY_INPUT_MANAGER
            return Input.mousePosition;
            #else
            return Mouse.current.position.value;
            #endif
        }

        public static bool IsAnyButtonPressed()
        {
            #if ENABLE_LEGACY_INPUT_MANAGER
            return Input.anyKey;
            #else
            return Keyboard.current.anyKey.isPressed; 
            #endif
        }

        public static bool IsURP()
        {
            var pipeline = GraphicsSettings.defaultRenderPipeline;
            if (pipeline == null)
            {
                // Built-in
                return false;
            }
            else
            {
                var name = pipeline.name;
                if (name.Contains("UniversalRP") || name.Contains("URP"))
                {
                    // URP
                    return true;
                }
                else
                {
                    // Something else
                    return false;
                }
            }

        }

        public static bool IsStateOptimizerPrefab(GameObject gameObject) 
        {
            return gameObject.GetComponent<FPSCounter>() != null && gameObject.GetComponent<DynamicRenderScale>() != null && gameObject.name.Contains("StateOptimizer");
        }
    }
}
