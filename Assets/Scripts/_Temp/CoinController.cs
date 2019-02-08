using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private CoinManager myCoinManager;
    private Animator myAnim;
    private ParticleSystem myParticle;

    // Use this for initialization
    void Start()
    {
        myCoinManager = FindObjectOfType<CoinManager>();
        myAnim = GetComponent<Animator>();
        myParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myCoinManager.AddScore(1);
        myAnim.Play("Open");
        Destroy(this);
        myParticle.Play();
    }
}