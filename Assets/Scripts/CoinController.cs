using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Rigidbody2D myRb;
    private CoinManager myCoinManager;

    // Use this for initialization
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myCoinManager = FindObjectOfType<CoinManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        myCoinManager.ChangeScore(1);
        Destroy(gameObject);
    }
}