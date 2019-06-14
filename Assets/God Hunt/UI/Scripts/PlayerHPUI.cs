using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool setupOnStart;
    [Header("Prefabs")]
    [SerializeField] Image HPChunkPrefab;
    [SerializeField] Sprite[] HPChunksFull;
    [SerializeField] Sprite HPChunkEmpty;
    [Header("References")]
    public DamageReceiverBehaviour damageReceiver;

    List<Image> HPChunks;

    public void Setup()
    {
        damageReceiver.OnHealthChanged.AddListener(UpdateUI);
        damageReceiver.OnHealthDepleated.AddListener(UpdateUI);
        damageReceiver.OnHealthUpgraded.AddListener(InstantiateChunks);

        InstantiateChunks();
    }

    void InstantiateChunks()
    {       
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        HPChunks = new List<Image>();

        for (int i = 0; i < damageReceiver.MaxHealth; i++)
        {
            Image hp = Instantiate(HPChunkPrefab, transform);
            hp.sprite = HPChunksFull[i];
            hp.SetNativeSize();
            HPChunks.Add(hp);
        }

        UpdateUIAuto();
    }

    public void UpdateUIAuto()
    {
        UpdateUI(damageReceiver.CurrentHealth);
    }

    void UpdateUI(int _hp_value)
    {
        for (int i = 0; i < damageReceiver.MaxHealth; i++)
        {
            //full hp
            if( i < _hp_value)
            {
                HPChunks[i].sprite = HPChunksFull[i];
            }
            else
            {
                HPChunks[i].sprite = HPChunkEmpty;
            }
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < damageReceiver.MaxHealth; i++)
        {
            //full hp
            if (i < 0)
            {
                HPChunks[i].sprite = HPChunksFull[i];
            }
            else
            {
                HPChunks[i].sprite = HPChunkEmpty;
            }
        }
    }
}