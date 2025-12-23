using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;
    private int i;
    private Rigidbody2D rb;
    private Vector2 platformDelta;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (points == null || points.Length == 0)
        {
            return;
        }

        Vector2 startPosition = points[0].position;
        if (rb != null)
        {
            rb.position = startPosition;
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private void FixedUpdate()
    {
        if (points == null || points.Length == 0)
        {
            platformDelta = Vector2.zero;
            return;
        }

        Vector2 current = rb != null ? rb.position : (Vector2)transform.position;
        if (Vector2.Distance(current, points[i].position) < 0.1f)
        {
            i = (i + 1) % points.Length;
        }

        Vector2 next = Vector2.MoveTowards(current, points[i].position, speed * Time.fixedDeltaTime);
        platformDelta = next - current;

        if (rb != null && rb.bodyType != RigidbodyType2D.Static)
        {
            rb.MovePosition(next);
        }
        else
        {
            transform.position = next;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (platformDelta == Vector2.zero)
            {
                return;
            }

            if (!collision.transform.DotTest(transform, Vector2.down))
            {
                return;
            }

            Rigidbody2D playerRb = collision.rigidbody;
            if (playerRb != null && !playerRb.isKinematic)
            {
                playerRb.MovePosition(playerRb.position + platformDelta);
            }
            else
            {
                collision.transform.position += (Vector3)platformDelta;
            }
        }
    }
}
