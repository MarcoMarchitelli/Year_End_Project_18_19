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
    [SerializeField] DamageReceiverBehaviour damageReceiver;
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

        HPChunks = new List<Image>();

        for (int i = 0; i < damageReceiver.MaxHealth; i++)
        {
            Image hp = Instantiate(HPChunkPrefab, transform);
            hp.SetNativeSize();
            HPChunks.Add(hp);
        }

        instHpEnd = Instantiate(HPEnd, transform);
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
}