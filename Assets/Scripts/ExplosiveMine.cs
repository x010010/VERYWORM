using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosiveMine : MonoBehaviour
{
    public float explosionRadius = 3f;
    public LayerMask wormLayer; 
    public GameObject explosionEffectPrefab; 
    public AudioClip explosionSound; 
    public float explosionVolume = 1.0f;

    private bool exploded = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!exploded && other.gameObject.CompareTag("Worm"))
        {
    
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);

            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, wormLayer);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Worm"))
                {
                    Destroy(collider.gameObject);
                }
            }

            exploded = true;

            StartCoroutine(DelaySceneChange());
        }
    }

    IEnumerator DelaySceneChange()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Lose");
    }
}
