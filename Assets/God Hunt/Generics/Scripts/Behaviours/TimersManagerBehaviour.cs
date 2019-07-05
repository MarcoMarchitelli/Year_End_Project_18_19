using System.Collections.Generic;

public class TimersManagerBehaviour : BaseBehaviour
{
    List<TimerBehaviour> timerBehaviours;

    protected override void CustomSetup()
    {
        timerBehaviours = Entity.GetBehaviours<TimerBehaviour>();
    }

    public void ResetAllTimers()
    {
        foreach (TimerBehaviour timer in timerBehaviours)
        {
            timer.ResetTimer();
        }
    }

    public void ResetAndStopAllTimers()
    {
        foreach (TimerBehaviour timer in timerBehaviours)
        {
            timer.StopTimer();
        }
    }
}