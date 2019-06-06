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

    [ContextMenu("Test Randomize")]
    public void RandomizeTest()
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