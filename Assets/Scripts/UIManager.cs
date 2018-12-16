using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class UIManager : MonoBehaviour {

    [SerializeField]
    /// <summary>
    /// Testo della salute del player
    /// </summary>
    private TextMeshProUGUI HealthText;

    [SerializeField]
    /// <summary>
    /// Testo della gravità
    /// </summary>
    private TextMeshProUGUI GravityText;
	
    public void ChangeHealthText(int healthToDisplay)
    {
        HealthText.text = ("Health: ") + healthToDisplay.ToString();
    }

    public void ChangeGravityText(float gravityToDisplay)
    {
        GravityText.text = ("Gravity: ") + gravityToDisplay.ToString();
    }
}
