using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private CoinManager myCoinManager;

    // Use this for initialization
    void Start()
    {
        myCoinManager = FindObjectOfType<CoinManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myCoinManager.AddScore(1);
        Destroy(gameObject);
    }
}