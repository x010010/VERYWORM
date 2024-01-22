using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WormMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float segmentFollowSpeed = 0.5f;
    private float remainingTime = 10.0f;
    public TMP_Text timerText;
    public TMP_Text appleText;

    public GameObject wormSegmentPrefab;
    private Vector2 direction = Vector2.up;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private List<Transform> segments = new List<Transform>();
    private bool isGrowing = false;

    public int initialSize = 1;
    private int applesCollected = 0;

    private AudioSource soundManager;
    public AudioClip appleEatSound;

    private bool gameFrozen = true;
    public GameObject instructionCanvas;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        soundManager = GameObject.Find("SoundManager").GetComponent<AudioSource>();

        ResetState();

        gameFrozen = true;
        Time.timeScale = 0;
        StartCoroutine(ShowInstruction());
    }

    IEnumerator ShowInstruction()
    {
        instructionCanvas.SetActive(true);
        yield return new WaitForSecondsRealtime(2); //2 seconds
        instructionCanvas.SetActive(false);
        gameFrozen = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (!gameFrozen)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && direction != Vector2.up)
            {
                direction = Vector2.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && direction != Vector2.down)
            {
                direction = Vector2.down;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && direction != Vector2.left)
            {
                direction = Vector2.left;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && direction != Vector2.right)
            {
                direction = Vector2.right;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }

            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                remainingTime = Mathf.Max(remainingTime, 0);
            }
            else
            {
                SceneManager.LoadScene("Lose");
            }

            timerText.text = "" + remainingTime.ToString("F1");
            appleText.text = "Apples: " + applesCollected.ToString();
        }
    }

    void FixedUpdate()
    {
        if (!gameFrozen)
        {
            Vector2 headPosition = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(headPosition);
            UpdateSegments();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            StartCoroutine(GrowWorm());
            Destroy(other.gameObject);
            FindObjectOfType<AppleSpawner>().SpawnApple();

            if (soundManager != null && appleEatSound != null)
            {
                soundManager.PlayOneShot(appleEatSound);
            }

            applesCollected++;

            if (applesCollected >= 5)
            {
                SceneManager.LoadScene("Win");
            }
        }
    }

    IEnumerator GrowWorm()
    {
        int segmentsToAdd = 10;
        Vector2 spawnPosition = segments.Count > 0 ? segments[segments.Count - 1].position : (Vector2)transform.position - direction;

        for (int i = 0; i < segmentsToAdd; i++)
        {
            Transform segment = Instantiate(wormSegmentPrefab, spawnPosition, Quaternion.identity).transform;
            segments.Add(segment);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateSegments()
    {
        Vector2 previousSegmentPosition = transform.position;

        foreach (Transform segment in segments)
        {
            Vector2 newPosition = Vector2.Lerp(segment.position, previousSegmentPosition, segmentFollowSpeed * Time.deltaTime);
            segment.position = newPosition;
            previousSegmentPosition = segment.position;
        }
    }

    private void ResetState()
    {
        direction = Vector2.up;
        spriteRenderer.flipX = false;
        spriteRenderer.flipY = false;

        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(this.transform);

        for (int i = 1; i < initialSize; i++)
        {
            segments.Add(Instantiate(wormSegmentPrefab).transform);
        }
    }
}
