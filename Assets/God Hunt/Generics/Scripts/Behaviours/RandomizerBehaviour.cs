using UnityEngine;

public class RandomizerBehaviour : BaseBehaviour
{
    public Outcome[] possibleOutcomes;

    public void Randomize()
    {
        float rng = Random.value;
        float currentProb = 0;
        foreach (Outcome outcome in possibleOutcomes)
        {
            currentProb += outcome.percentage;
            if (rng <= currentProb)
            {
                outcome.onOutcomeRolled.Invoke();
                break;
            }
        }
    }

    [ContextMenu("Test Randomize with descriptions")]
    public void RandomizeTestDescriptions()
    {
        for (int i = 0; i < 100; i++)
        {
            float rng = Random.value;
            float currentProb = 0;
            foreach (Outcome outcome in possibleOutcomes)
            {
                currentProb += outcome.percentage;
                if (rng <= currentProb)
                {
                    outcome.onOutcomeRolled.Invoke();
                    print(outcome.description);
                    break;
                }
            } 
        }
    }

    [ContextMenu("Test Randomize with indexes")]
    public void RandomizeTestIndexes()
    {
        for (int i = 0; i < 100; i++)
        {
            float rng = Random.value;
            float currentProb = 0;
            for (int j = 0; j < possibleOutcomes.Length; j++)
            {
                currentProb += possibleOutcomes[j].percentage;
                if (rng <= currentProb)
                {
                    possibleOutcomes[j].onOutcomeRolled.Invoke();
                    print(j.ToString());
                    break;
                }
            }
        }
    }

    [System.Serializable]
    public class Outcome
    {
        [Multiline]
        public string description;
        [Range(0, 1)]
        public float percentage;
        public UnityVoidEvent onOutcomeRolled;
    }
}