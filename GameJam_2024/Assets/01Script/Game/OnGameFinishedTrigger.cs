using UnityEngine;

public class OnGameFinishedTrigger : MonoBehaviour
{
    public void SetGameToFinished()
    {
        Game.GameFinished = true;
    }
}
