using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool setupOnStart;
    [Header("Prefabs")]
    [SerializeField] Image HPChunkPrefab;
    [SerializeField] Sprite HPChunkFull;
    [SerializeField] Sprite HPChunkEmpty;
    [SerializeField] Sprite PlayerIconFull;
    [SerializeField] Sprite PlayerIconDamaged;
    [Header("References")]
    public DamageReceiverBehaviour damageReceiver;
    [SerializeField] Image PlayerIcon;
    [SerializeField] Image HPEnd;

    List<Image> HPChunks;
    Image instHpEnd;

    private void Start()
    {
        if (setupOnStart)
            Setup();
    }

    void Setup()
    {
        damageReceiver.OnHealthChanged.AddListener(UpdateUI);
        damageReceiver.OnHealthDepleated.AddListener(UpdateUI);

        HPChunks = new List<Image>();

        for (int i = 0; i < damageReceiver.MaxHealth; i++)
        {
            Image hp = Instantiate(HPChunkPrefab, transform);
            hp.SetNativeSize();
            HPChunks.Add(hp);
        }

        instHpEnd = Instantiate(HPEnd, transform);
    }

    public void UpdateUIAuto()
    {
        UpdateUI(damageReceiver.CurrentHealth);
    }

    void UpdateUI(int _hp_value)
    {
        if(_hp_value < damageReceiver.MaxHealth)
        {
            PlayerIcon.sprite = PlayerIconDamaged;
        }
        else
        {
            PlayerIcon.sprite = PlayerIconFull;
        }

        for (int i = 0; i < damageReceiver.MaxHealth; i++)
        {
            //full hp
            if( i < _hp_value)
            {
                HPChunks[i].sprite = HPChunkFull;
            }
            else
            {
                HPChunks[i].sprite = HPChunkEmpty;
            }
        }
    }

    void UpdateUI()
    {
        if (0 < damageReceiver.MaxHealth)
        {
            PlayerIcon.sprite = PlayerIconDamaged;
        }
        else
        {
            PlayerIcon.sprite = PlayerIconFull;
        }

        for (int i = 0; i < damageReceiver.MaxHealth; i++)
        {
            //full hp
            if (i < 0)
            {
                HPChunks[i].sprite = HPChunkFull;
            }
            else
            {
                HPChunks[i].sprite = HPChunkEmpty;
            }
        }
    }
}