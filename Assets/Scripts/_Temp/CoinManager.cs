using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public CoinController[] CoinList;
    public int TotalCoins;
    public Canvas VictoryCanvas;
    private bool hasWin;

    public int Score;

    private void Start()
    {
        CoinList = FindObjectsOfType<CoinController>();
        TotalCoins = CoinList.Length;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && hasWin)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        if (Score >= 2)
        {
            VictoryCanvas.enabled = true;
            hasWin = true;
        }
    }
}