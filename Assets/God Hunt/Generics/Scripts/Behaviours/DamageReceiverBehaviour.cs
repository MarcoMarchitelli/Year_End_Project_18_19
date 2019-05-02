using UnityEngine;

public class DamageReceiverBehaviour : BaseBehaviour
{
    [SerializeField] bool camShake = true;
    [SerializeField] float frequency = 1.5f, amplitude = .4f, duration = .2f;
    [SerializeField] bool freezeFrames = true;
    [SerializeField] float time = .1f;

    #region Events
    public UnityIntEvent OnHealthChanged;
    public UnityVoidEvent OnHealthDepleated;
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
                if (camShake)
                    CameraManager.Instance.CameraShake(1.5f, .4f, .2f);
                if (freezeFrames)
                    GameManager.Instance.FreezeFrames(.1f);
            }
        }
    }

    /// <summary>
    /// Health setter. Returns true if damage was dealt succesfully.
    /// </summary>
    /// <param name="_value">Health to add (subtract if negative)/param>
    public bool SetHealth(int _value, bool _deal_through_invulnerability = false)
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
        return true;
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        OnHealthChanged.Invoke(_currentHealth);
    }
}