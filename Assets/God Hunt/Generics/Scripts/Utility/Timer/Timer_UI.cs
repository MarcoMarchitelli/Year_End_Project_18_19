using UnityEngine;
using UnityEngine.UI;
using Deirin.Utility;
using TMPro;

public class Timer_UI : MonoBehaviour
{
    [Header("Target")]
    public Timer timer;

    [Header("References")]
    public Slider slider;
    public TextMeshProUGUI text;

    [Header("Behaviours")]
    public bool refreshEachFrame = true;

    private void Awake()
    {
        if (slider == null)
        {
            Debug.LogError(name + " is missing a slider reference!");
        }
        else
        {
            slider.maxValue = 0;
        }
    }

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        slider.minValue = -timer.time;
        slider.value = -(timer.time - timer.timer);
        if (slider.value == slider.minValue)
            text.text = "";
        else
            text.text = Mathf.Abs(slider.value).ToString().Substring(0,1);
    }

}
