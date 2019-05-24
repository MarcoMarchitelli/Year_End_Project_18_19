using UnityEngine;

public class RotatorBehaviour : BaseBehaviour
{
    public Vector3 rotationEuler;
    public Space rotationSpace;
    public bool startRotatingOnAwake;

    bool canRotate;

    protected override void CustomSetup()
    {
        if (startRotatingOnAwake)
            Rotate(true);
    }

    public override void OnUpdate()
    {
        if (!canRotate)
            return;

        transform.Rotate(rotationEuler * Time.deltaTime, rotationSpace);
    }

    public void Rotate(bool _value)
    {
        canRotate = true;
    }
}