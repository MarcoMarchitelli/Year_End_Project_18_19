using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHorizontalFacingState : CameraState
{

	public AnimationCurve transitionAnimation;
	public float xPosition;

	protected float xStarting;
	protected Transform cameraTarget;
	protected float timer;
	protected Vector3 actCameraTargetPosition;

	private float maxEvaluationTimer = 60f;

	protected override void Awake()
	{
		base.Awake();
		cameraTarget = mainVcam.Follow;
	}

	protected override void Enter()
	{
		timer = 0;
		actCameraTargetPosition = cameraTarget.localPosition;
		xStarting = cameraTarget.localPosition.x;
	}

	protected override void Tick()
	{
		if (timer < maxEvaluationTimer)
		{
			timer += Time.deltaTime;
			actCameraTargetPosition.x = Mathf.Lerp(xStarting, xPosition, transitionAnimation.Evaluate(timer));
			actCameraTargetPosition.y = cameraTarget.localPosition.y;
			actCameraTargetPosition.z = cameraTarget.localPosition.z;
			cameraTarget.localPosition = actCameraTargetPosition;
		}
	}

}
