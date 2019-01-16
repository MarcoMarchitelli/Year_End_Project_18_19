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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        myCoinManager.ChangeScore(1);
        Destroy(gameObject);
    }
}