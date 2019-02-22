using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerCollisionBehaviour : BaseBehaviour
{

    #region Serialized Fields

    [SerializeField] UnityVoidEvent OnCollisionBelowTrue;
    [SerializeField] UnityVoidEvent OnCollisionBelowFalse;

    #endregion

    bool _below;
    /// <summary>
    /// Checks the collision state below the collider. Calls the relative event.
    /// </summary>
    public bool Below
    {
        get { return _below; }
        set
        {
            if (value != _below)
            {
                _below = value;
                if (_below)
                {
                    OnCollisionBelowTrue.Invoke();
                }
                else
                {
                    OnCollisionBelowFalse.Invoke();
                }
            }
        }
    }

    GameObject objectBelow;

    /// <summary>
    /// Checks the firs contact point and if its normal is faceing up it registers the object hit as the object below.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
        {
            objectBelow = collision.collider.gameObject;
            Below = true;
        }
    }

    /// <summary>
    /// Every time we exit a collision it checks if it was the object below and sets the collision state accordingly.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject == objectBelow)
        {
            Below = false;
        }
    }

    public void ResetCollisionValues()
    {
        Below = false;
    }

}