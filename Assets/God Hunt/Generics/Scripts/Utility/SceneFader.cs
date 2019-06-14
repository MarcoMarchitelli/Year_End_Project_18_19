using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    public enum State { FadedOut, FadedIn }

    private State _currentState;

    public Image FadeImage;
    public State CurrentState { get => _currentState; set => _currentState = value; }

    public void StartFade(State _state, float _duration, System.Action _callback = null)
    {
        CurrentState = _state;
        switch (CurrentState)
        {
            case State.FadedOut:
                FadeImage.DOColor(Color.black, _duration).onComplete += () => _callback?.Invoke();
                break;
            case State.FadedIn:
                FadeImage.DOColor(Color.clear, _duration).onComplete += () => _callback?.Invoke();
                break;
        }
    }

    public void SetState(State _state)
    {
        CurrentState = _state;
        switch (CurrentState)
        {
            case State.FadedOut:
                FadeImage.color = Color.black;
                break;
            case State.FadedIn:
                FadeImage.color = Color.clear;
                break;
        }
    }
}