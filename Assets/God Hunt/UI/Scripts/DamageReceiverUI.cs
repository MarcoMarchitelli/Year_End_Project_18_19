using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DamageReceiverUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject healthChunkPrefab;
    [SerializeField] DamageReceiverBehaviour damageReceiver;

    [Header("Options")]
    [SerializeField] bool setupOnAwake = false;

    GameObject[] healthChunks;
    HorizontalLayoutGroup layoutGroup;

    public void SetUp(DamageReceiverBehaviour _drb = null)
    {
        if (!damageReceiver && _drb)
            damageReceiver = _drb;

        damageReceiver.OnHealthChanged.AddListener(UpdateUI);
        damageReceiver.OnHealthUpgraded.AddListener(UpgradeUI);

        layoutGroup = GetComponent<HorizontalLayoutGroup>();

        StartCoroutine(LayoutSetup(layoutGroup));
    }

    public void UpdateUI(int _newHealth)
    {      
        for (int i = 0; i < healthChunks.Length; i++)
        {
            if (i > _newHealth - 1)
            {
                healthChunks[i].SetActive(false);
            }
            else
            {
                healthChunks[i].SetActive(true);
            }
        }
    }

    public void UpgradeUI()
    {
        StartCoroutine(LayoutSetup(layoutGroup));
    }

    IEnumerator LayoutSetup(HorizontalLayoutGroup _l, bool _updateUI = false)
    {
        healthChunks = new GameObject[damageReceiver.MaxHealth];

        _l.childControlWidth = true;
        _l.childControlHeight = true;
        _l.childForceExpandWidth = true;
        _l.childForceExpandHeight = true;
        for (int i = 0; i < healthChunks.Length; i++)
        {
            healthChunks[i] = Instantiate(healthChunkPrefab, transform);
        }
        yield return null;
        _l.childControlWidth = false;
        _l.childControlHeight = false;
        _l.childForceExpandWidth = false;
        _l.childForceExpandHeight = false;

        if (_updateUI)
        {
            UpdateUI(damageReceiver.CurrentHealth);
        }
    }

    private void Start()
    {
        if(setupOnAwake)
            SetUp();
    }

}