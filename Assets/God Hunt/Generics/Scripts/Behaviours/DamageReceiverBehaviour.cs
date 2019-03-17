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
                print(name + " health has changed from " + _currentHealth + " to " + value);
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
    /// Funzione che aggiunge o sottrae salute
    /// </summary>
    /// <param name="_value">la salute da aggiungere o sottrarre</param>
    public void SetHealth(int _value)
    {
        if (!IsSetupped)
            return;
        int tempHealth = CurrentHealth;
        tempHealth += _value;
        if (tempHealth <= 0)
            tempHealth = 0;
        if (tempHealth > maxHealth)
            tempHealth = maxHealth;
        CurrentHealth = tempHealth;
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        OnHealthChanged.Invoke(_currentHealth);
    }
}