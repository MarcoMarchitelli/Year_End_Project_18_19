using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDayScript : MonoBehaviour
{

    public GameObject canvasOpenDay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        canvasOpenDay.SetActive(true);
    }
}
