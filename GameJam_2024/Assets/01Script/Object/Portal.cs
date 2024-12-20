using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void Start()
    {
        SoundController.Instance.PlayBGM(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
