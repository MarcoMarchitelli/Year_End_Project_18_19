using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVerticalFacingState : CameraState
{
	public AnimationCurve transitionAnimation;
	public float yPosition;

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
			actCameraTargetPosition.x = cameraTarget.localPosition.x;
			actCameraTargetPosition.y = Mathf.Lerp(cameraTarget.localPosition.y, yPosition, transitionAnimation.Evaluate(timer));
			actCameraTargetPosition.z = cameraTarget.localPosition.z;
			cameraTarget.localPosition = actCameraTargetPosition;
		}
	}
}
