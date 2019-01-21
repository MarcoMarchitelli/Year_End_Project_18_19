using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FindCameraTarget : MonoBehaviour
{

	private string tagName = "CameraTarget";

	// Start is called before the first frame update
	void Awake()
    {
		GameObject tmpPointer = GameObject.FindGameObjectWithTag(tagName);
		if(tmpPointer) {
			GetComponent<CinemachineVirtualCamera>().Follow = tmpPointer.transform;
		}
	}

}
