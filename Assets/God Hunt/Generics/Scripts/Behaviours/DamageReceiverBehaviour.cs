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
    /// Returns true if damage was inflicted.
    /// </summary>
    /// <param name="_value">health amount to add (remove if negative)</param>
    public bool SetHealth(int _value, bool _goesThroughInvulnerability = false)
    {
        if (!IsSetupped && !_goesThroughInvulnerability)
        {
            Debug.LogWarning(name + "'s damage receiver is not setupped!");
            return false;
        }

        int tempHealth = CurrentHealth;
        tempHealth += _value;

        if (tempHealth <= 0)
        {
            tempHealth = 0;
        }
        else if (tempHealth > maxHealth)
        {
            tempHealth = maxHealth;
        }

        CurrentHealth = tempHealth;

        return true;
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        OnHealthChanged.Invoke(_currentHealth);
    }
}