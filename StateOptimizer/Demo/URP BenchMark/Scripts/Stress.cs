using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SOptimizer.Demo
{
    public class Stress : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] int _numberOfObjects = 20;
        [SerializeField] List<Vector3> _pos;
        [SerializeField] float _speed = 0.1f;
        [SerializeField] float _delay = 0.05f;
        [SerializeField] MotionMethod _motion;
        [SerializeField] float _timeToChangeMotion = 3;
        static GameObject _parent;
        List<GameObject> _objects;
        Camera _camera;
        enum MotionMethod { Slerp, Lerp }
        Coroutine motionCorotine;
        Coroutine changeMotionCorotine;
        void OnEnable()
        {
            if (_parent == null)
            {
                _parent = new GameObject();
            }
            _parent.transform.name = "Junks";
            _objects = new List<GameObject>();
            _camera = Camera.main;
            SpawnAllObjs();
            motionCorotine = StartCoroutine(Motion());
            changeMotionCorotine = StartCoroutine(ChangeMotionCorotine());
        }
        private void OnDisable()
        {
            if (_parent != null)
            {
                Destroy(_parent);
                _parent = null;

                StopCoroutine(motionCorotine);
                motionCorotine = null;

                StopCoroutine(changeMotionCorotine);
                changeMotionCorotine = null;
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
        IEnumerator Motion()
        {
            yield return new WaitForSecondsRealtime(_delay);

            foreach (var obj in _objects)
            {
                StartCoroutine(Motion(obj));
                yield return new WaitForSecondsRealtime(_delay);
            }

        }



        IEnumerator ChangeMotionCorotine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(_timeToChangeMotion);
                if (_motion == MotionMethod.Lerp)
                    _motion = MotionMethod.Slerp;
                else
                    _motion = MotionMethod.Lerp;
            }
        }
        IEnumerator Motion(GameObject g)
        {
            int index = 0;
            while (true)
            {
                while (Vector3.Distance(g.transform.position, _pos[index]) > 0.5f)
                {
                    if (_motion == MotionMethod.Slerp)
                        g.transform.position = Vector3.Slerp(g.transform.position, _pos[index], _speed);
                    else
                        g.transform.position = Vector3.Lerp(g.transform.position, _pos[index], _speed);
                    //g.transform.LookAt(_camera.transform);
                    yield return new WaitForSecondsRealtime(_delay);

                }
                index++;
                if (index >= _pos.Count)
                    index = 0;
            }
        }

        void SpawnAllObjs()
        {
            for (int i = 0; i < _numberOfObjects; i++)
            {
                SpawnObj();
            }
        }

        void SpawnObj()
        {
            GameObject g = Instantiate(_prefab, _pos[0], Quaternion.identity);
            g.transform.parent = _parent.transform;
            g.transform.name = "C:" + _objects.Count;
            _objects.Add(g);

        }
    }
}
