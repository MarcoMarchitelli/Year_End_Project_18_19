using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public CoinController[] CoinList;
    public int TotalCoins;

    public int Score;

    private void Start()
    {
        CoinList = FindObjectsOfType<CoinController>();
        TotalCoins = CoinList.Length;
    }

    public void ChangeScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }
}