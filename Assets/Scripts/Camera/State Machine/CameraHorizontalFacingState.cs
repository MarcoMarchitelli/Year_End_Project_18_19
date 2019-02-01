using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHorizontalFacingState : CameraState
{

	public AnimationCurve transitionAnimation;
	public float xPosition;

	protected Transform cameraTarget;
	protected float timer;
	protected Vector3 actCameraTargetPosition;

	private float maxEvaluationTimer = 5f;

	protected override void Awake()
	{
		base.Awake();
		cameraTarget = mainVcam.Follow;
	}

	protected override void Enter()
	{
		timer = 0;
		actCameraTargetPosition = cameraTarget.localPosition;
	}

	protected override void Tick()
	{
		if (timer < maxEvaluationTimer)
		{
			timer += Time.deltaTime;
			actCameraTargetPosition.x = Mathf.Lerp(cameraTarget.localPosition.x, xPosition, transitionAnimation.Evaluate(timer));
			actCameraTargetPosition.y = cameraTarget.localPosition.y;
			actCameraTargetPosition.z = cameraTarget.localPosition.z;
			cameraTarget.localPosition = actCameraTargetPosition;
		}
	}

}
