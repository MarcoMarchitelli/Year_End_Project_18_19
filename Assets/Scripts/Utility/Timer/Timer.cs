using UnityEngine;
using UnityEngine.Events;

namespace Deirin.Utility
{
    public class Timer : MonoBehaviour
    {
        [Multiline] public string description;
        public float time;
        public bool startCountingOnAwake = false;
        public bool repeat = true;

        public UnityEvent OnTimerStart, OnTimerEnd;

        bool countTime = false;
        [HideInInspector] public float timer;

        #region MonoBehaviour methods
        private void OnEnable()
        {
            if (repeat)
                OnTimerEnd.AddListener(ResetTimer);
            else
                OnTimerEnd.AddListener(StopTimer);
        }

        private void OnDisable()
        {
            if (repeat)
                OnTimerEnd.RemoveListener(ResetTimer);
            else
                OnTimerEnd.RemoveListener(StopTimer);
        }

        private void Awake()
        {
            if (startCountingOnAwake)
                StartTimer(time);
        }

        void Update()
        {
            if (countTime)
                timer += Time.deltaTime;
            if (timer >= time && countTime)
            {
                OnTimerEnd.Invoke();
            }
        }
        #endregion

        #region API
        public void StartTimer(float _time)
        {
            time = _time;
            timer = 0;
            countTime = true;
            OnTimerStart.Invoke();
        }

        public void StartTimer()
        {
            timer = 0;
            countTime = true;
            OnTimerStart.Invoke();
        }

        public void StopTimer()
        {
            countTime = false;
        }

        public void EndTimer()
        {
            countTime = false;
            OnTimerEnd.Invoke();
        }

        public void PauseTimer()
        {
            countTime = false;
        }

        public void ResumeTimer()
        {
            countTime = true;
        }

        public void ResetTimer()
        {
            timer = 0;
            if (repeat)
                countTime = true;
            else
                countTime = false;
        }

        public void Say(string _msg)
        {
            Debug.Log(_msg);
        }
        #endregion
    }
}