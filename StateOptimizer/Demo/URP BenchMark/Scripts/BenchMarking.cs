using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SOptimizer.Demo
{
    public class BenchMarking : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        [SerializeField] int[] _framesToPrint = { 30, 60, 180, 300 };
        [SerializeField] GameObject _optimizerRef;
        [SerializeField] GameObject _stress;
        int _frames = 0;
        int[] _framesData;
        Coroutine[] _coroutins;
        void Start()
        {
            _framesData = new int[_framesToPrint.Length];
            _coroutins = new Coroutine[_framesToPrint.Length];

            StartCoroutine(Print(0, 0));
            SetCameraRender(false);
        }


        void Update()
        {
            _frames++;
        }

        IEnumerator Print(int afterTime, int index)
        {
            yield return new WaitForSecondsRealtime(afterTime);
            if (afterTime != 0)
                _framesData[index] = _frames;

            if (_framesToPrint.Length - 1 == index)
                SetCameraRender(false);

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _framesToPrint.Length; i++)
            {
                string value = _framesData[i] == 0 ? "??????" : _framesData[i].ToString("d6");
                stringBuilder.Append($"{_framesToPrint[i]}Sec: {value}\t");
            }
            _text.text = stringBuilder.ToString();
        }


        public void StartBenchmark()
        {
            _frames = 0;
            StopBenchmark();
            SetCameraRender(true);
            _stress.SetActive(true);
            for (int i = 0; i < _framesToPrint.Length; i++)
            {
                _coroutins[i] = StartCoroutine(Print(_framesToPrint[i], i));
            }
        }


        public void StopBenchmark()
        {
            SetCameraRender(false);
            _framesData = new int[_framesToPrint.Length];
            _stress.SetActive(false);
            for (int i = 0; i < _framesToPrint.Length; i++)
            {
                if (_coroutins[i] != null)
                {
                    StopCoroutine(_coroutins[i]);
                    _coroutins[i] = null;
                }
            }
            StartCoroutine(Print(0, 0));
        }

        public void SetPerformanceTo(Toggle toggle)
        {
            StopBenchmark();
            _optimizerRef.SetActive(toggle.isOn);
        }

        void SetCameraRender(bool state)
        {
            if (state)
                Camera.main.farClipPlane = 1000f;
            else
                Camera.main.farClipPlane = 1f;
        }
    }
}
