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

    public void SetUp(DamageReceiverBehaviour _drb = null)
    {
        if (!damageReceiver && _drb)
            damageReceiver = _drb;

        damageReceiver.OnHealthChanged.AddListener(UpdateUI);

        HorizontalLayoutGroup layoutGroup = GetComponent<HorizontalLayoutGroup>();

        StartCoroutine(LayoutSetup(layoutGroup));
        print(name + "ui setup done!");
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

    IEnumerator LayoutSetup(HorizontalLayoutGroup _l)
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
    }

    private void Start()
    {
        if(setupOnAwake)
            SetUp();
    }

}