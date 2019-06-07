using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(EventTrigger))]
public class CustomButton : MonoBehaviour
{
    public UnityVoidEvent OnSelect, OnDeselect, OnClick;

    public void Select()
    {
        OnSelect.Invoke();
    }

    public void Deselect()
    {
        OnDeselect.Invoke();
    }

    public void Click()
    {
        OnClick.Invoke();
    }
}