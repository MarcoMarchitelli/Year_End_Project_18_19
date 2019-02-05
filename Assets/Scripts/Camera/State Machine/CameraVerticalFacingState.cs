using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVerticalFacingState : CameraState
{
	public AnimationCurve transitionAnimation;
	public float yPosition;

	protected float yStarting;
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
		yStarting = cameraTarget.localPosition.y;
	}

	protected override void Tick()
	{
		if (timer < maxEvaluationTimer)
		{
			timer += Time.deltaTime;
			actCameraTargetPosition.x = cameraTarget.localPosition.x;
			actCameraTargetPosition.y = Mathf.Lerp(yStarting, yPosition, transitionAnimation.Evaluate(timer));
			actCameraTargetPosition.z = cameraTarget.localPosition.z;
			cameraTarget.localPosition = actCameraTargetPosition;
		}
	}
}
