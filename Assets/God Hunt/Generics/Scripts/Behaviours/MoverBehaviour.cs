using UnityEngine;

public class MoverBehaviour : BaseBehaviour
{
    public bool startMovingOnAwake = true;
    public float movementSpeed;
    public Vector3 forwardDirection;
    public Space spaceType;

    bool canMove;

    protected override void CustomSetup()
    {
        if (startMovingOnAwake)
            Move(true);
    }

    public override void OnUpdate()
    {
        if (!canMove)
            return;

        transform.Translate(forwardDirection.normalized * movementSpeed * Time.deltaTime, spaceType);
    }

    public void Move(bool _value)
    {
        canMove = _value;
    }
}