using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using System.Reflection;

namespace SOptimizer
{
    public class DynamicRenderScale : MonoBehaviour
    {
        public static DynamicRenderScale Instance { get; private set; }

        float _originalScale;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;
        }
        private void OnEnable()
        {
            _originalScale = GetRenderScale();
        }
        private void OnDisable()
        {
            SetRenderScale(_originalScale);
        }

        private float GetRenderScale()
        {
            if (StateOptimizerUtility.IsURP())
            {
                var prop = GraphicsSettings.renderPipelineAsset.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(it => it.Name == "renderScale");
                return (float)prop.GetValue(GraphicsSettings.renderPipelineAsset);
            }

            else
                return 1.0f;

        }

        public static void SetRenderScale(float scale)
        {
            if (StateOptimizerUtility.IsURP())
            {
                var prop = GraphicsSettings.renderPipelineAsset.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(it => it.Name == "renderScale");
                prop.SetValue(GraphicsSettings.renderPipelineAsset, scale);
            }

        }

    }
}
