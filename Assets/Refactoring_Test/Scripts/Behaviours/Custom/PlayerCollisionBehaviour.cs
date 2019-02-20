using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerCollisionBehaviour : BaseBehaviour
{

    #region Serialized Fields

    [SerializeField] UnityBoolEvent OnCollisionBelowStateChange;

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
                OnCollisionBelowStateChange.Invoke(_below);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
        {
            Below = true;
        }
        else
            Below = false;
    }

    public void ResetCollisionValues()
    {
        Below = false;
    }

}
