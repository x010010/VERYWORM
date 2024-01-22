using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Worm"))
        {
            SceneManager.LoadScene("Lose");
        }
    }
}
