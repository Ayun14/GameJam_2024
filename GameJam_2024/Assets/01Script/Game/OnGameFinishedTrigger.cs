using UnityEngine;

public class OnGameFinishedTrigger : MonoBehaviour
{
    private void Awake()
    {
        Game.GameFinished = false;
    }
    public void SetGameToFinished()
    {
        Game.GameFinished = true;
    }
}
