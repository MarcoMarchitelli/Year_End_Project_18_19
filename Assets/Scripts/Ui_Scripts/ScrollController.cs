using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{

    Vector3 minScale = new Vector3(1,1,1);
    Vector3 maxScale = new Vector3(2, 2, 1);
    [Tooltip("Immagine della mappa")]
    public GameObject Map;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && Map.transform.localScale.x <= maxScale.x) {
            Map.transform.localScale = Map.transform.localScale + new Vector3(0.25f,0.25f,0);
        }
        if (Input.GetButtonDown("Dash") && Map.transform.localScale.x >= minScale.x)
        {
            Map.transform.localScale = Map.transform.localScale - new Vector3(0.25f,0.25f, 0);
        }

        if (Input.GetButton("Horizontal")) {
            float translation = Input.GetAxisRaw("Horizontal") * 10;
            Map.transform.Translate(-translation,0,0);
        }
        if (Input.GetButton("Vertical"))
        {
            float translation = Input.GetAxisRaw("Vertical") * 10;
            Map.transform.Translate(0, -translation, 0);
        }
    }
}
