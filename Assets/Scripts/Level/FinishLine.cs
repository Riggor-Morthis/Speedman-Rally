using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RaceCountdown rc;
        if ((rc = other.gameObject.GetComponent<RaceCountdown>()) != null)
            rc.StopTheClock();
    }
}
