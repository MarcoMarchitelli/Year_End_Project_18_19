using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIX_MaterialEnemy : MonoBehaviour
{

    public Material mat;
    public GameObject graphic;

    private void Awake()
    {
        foreach(MeshRenderer mr in graphic.GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = mat;
        }
    }



}
