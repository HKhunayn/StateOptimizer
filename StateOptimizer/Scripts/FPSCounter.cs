using System.Collections.Generic;
using UnityEngine;

namespace SOptimizer
{
    public class FPSCounter : MonoBehaviour
    {

        private List<int> _fps;
        private int _fpsLength;
        private int _fpsSum;
        private int _currentFPS;
        private float _checkTime;
        private float _lastCheckTime;

        private void Awake()
        {
            _fps = new List<int>();
            _fpsLength = 10;
            _lastCheckTime = int.MinValue;
        }

        public void Init(float checkTime)
        {
            _checkTime = checkTime;
        }

        private void Update()
        {
            _currentFPS++;

            if (Time.time - _checkTime >= _lastCheckTime)
            {
                AddFPS(Mathf.RoundToInt(_currentFPS * (1f / _checkTime)));
                _currentFPS = 0;
                _lastCheckTime = Time.time;
            }
        }

        private void AddFPS(int currentFPS)
        {
            _fps.Add(currentFPS);
            _fpsSum += currentFPS;
            if (_fps.Count > +_fpsLength)
                RemoveOneFPS();
        }

        private void RemoveOneFPS()
        {
            _fpsSum -= _fps[0];
            _fps.RemoveAt(0);
        }

        public float GetFPSAVG()
        {
            if (_fps.Count == 0)
                return Application.targetFrameRate;
            return _fpsSum / _fps.Count;
        }
        public void ResetFPSList()
        {
            _fps = new List<int>();
            _fpsSum = 0;
        }

    }
}
