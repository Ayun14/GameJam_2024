using UnityEngine;

public class Tutorial_TrapAnimation : MonoBehaviour
{
    public void OnEnter()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        print("animationEvent");
    }
}
