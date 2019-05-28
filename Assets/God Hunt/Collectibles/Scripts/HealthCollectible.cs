using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    public Vector3 TranslationUp = new Vector3(0,1,0);
    public Vector3 TranslationDown =  new Vector3(0,-1,0);
    public float WaitTime;
    public Vector3 speed;
    private bool Up;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
        if (Up == true)
        {
            StartCoroutine(Floatup());
        }else
        {
            StartCoroutine(Floatdown());;
        }
        
    }

    IEnumerator Floatup() {
        transform.Translate(TranslationUp * Time.deltaTime);
        yield return new WaitForSeconds(WaitTime);
        Up = false;
    }
    IEnumerator Floatdown() {
        transform.Translate(-TranslationDown * Time.deltaTime);
        yield return new WaitForSeconds(WaitTime);
        Up = true;
    }
}
