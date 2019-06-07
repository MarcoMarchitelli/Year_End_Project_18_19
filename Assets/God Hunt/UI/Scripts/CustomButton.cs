using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    Button _button;
    Button Button { get { if (!_button) _button = GetComponent<Button>(); return _button; } }

    public void Select()
    {
        Button.Select();
    }
}