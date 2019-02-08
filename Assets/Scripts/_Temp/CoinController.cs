using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private CoinManager myCoinManager;
    private Animator myAnim;

    // Use this for initialization
    void Start()
    {
        myCoinManager = FindObjectOfType<CoinManager>();
        myAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myCoinManager.AddScore(1);
        myAnim.Play("Open");
    }
}