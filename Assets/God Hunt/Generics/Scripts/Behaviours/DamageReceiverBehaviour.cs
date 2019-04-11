using UnityEngine;

public class DamageReceiverBehaviour : BaseBehaviour
{
    #region Events

    public UnityIntEvent OnHealthChanged;
    [SerializeField] UnityVoidEvent OnHealthDepleated;

    #endregion

    protected override void CustomSetup()
    {
        _currentHealth = maxHealth;
    }

    [SerializeField] int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
    }
    int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        private set
        {
            if (_currentHealth != value)
            {
                _currentHealth = value;
                if (_currentHealth == 0)
                {
                    OnHealthDepleated.Invoke();
                }
                else
                {
                    OnHealthChanged.Invoke(_currentHealth);
                }
            }
        }
    }

    /// <summary>
    /// Health setter. Returns true if damage was dealt succesfully.
    /// </summary>
    /// <param name="_value">Health to add (subtract if negative)/param>
    public bool SetHealth(int _value, bool _deal_through_invulnerability = false, bool _camShake = true)
    {
        if (!IsSetupped && !_deal_through_invulnerability)
        {
            Debug.LogWarning(name + "'s damage receiver is not setupped!");
            return false;
        }

        int tempHealth = CurrentHealth;
        tempHealth += _value;

        if (tempHealth <= 0)
            tempHealth = 0;
        else if (tempHealth > maxHealth)
            tempHealth = maxHealth;

        CurrentHealth = tempHealth;
        if (_camShake)
            CameraManager.Instance.CamShake();
        return true;
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        OnHealthChanged.Invoke(_currentHealth);
    }
}