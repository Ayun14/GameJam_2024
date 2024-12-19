using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameFinishedTrigger : MonoBehaviour
{
    public void SetGameToFinished()
    {
        Game.GameFinished = false;
    }
}
