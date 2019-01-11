using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CameraZoom : MonoBehaviour {

	public Camera Mc;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha9)) {
			Mc.transform.position = new Vector3(Mc.transform.position.x, Mc.transform.position.y, Mathf.Clamp(Mc.transform.position.z +1, -30, -2));
		} else if(Input.GetKeyDown(KeyCode.Alpha0)) {
			Mc.transform.position = new Vector3(Mc.transform.position.x, Mc.transform.position.y, Mathf.Clamp(Mc.transform.position.z - 1, -30, -2));
		}
	}
}
