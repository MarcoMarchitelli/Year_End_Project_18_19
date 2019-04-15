using UnityEngine;

public class PlayerAttacksBehaviour : BaseBehaviour
{
    public void SetDirection(Vector2 _direction)
    {
        if (_direction.x > 0)
            TurnRight();
        else if (_direction.x < 0)
            TurnLeft();
    }

    void TurnRight()
    {
        transform.rotation = Quaternion.Euler(Vector2.zero);
    }

    void TurnLeft()
    {
        transform.rotation = Quaternion.Euler(Vector2.up * 180);
    }

}