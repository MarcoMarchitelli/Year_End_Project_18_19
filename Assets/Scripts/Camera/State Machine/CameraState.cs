using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class CameraState : PlayerStateBase
{

	protected CinemachineVirtualCamera mainVcam;

	protected virtual void Awake()
	{
		mainVcam = GameObject.FindGameObjectWithTag("MainVcam").GetComponent<CinemachineVirtualCamera>();
	}

}
