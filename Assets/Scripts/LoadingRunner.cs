using UnityEngine;

public class LoadingRunner : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float horizontalMargin = 1f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float pickupRadius = 0.35f;
    [SerializeField] private Transform[] coins;

    private float startX;
    private float endX;
    private bool initialized;

    public bool Completed { get; private set; }

    private void Start()
    {
        InitializePath();
        ResetRun();
    }

    private void Update()
    {
        if (Completed || !initialized)
        {
            return;
        }

        Vector3 position = transform.position;
        position.x = Mathf.MoveTowards(position.x, endX, speed * Time.deltaTime);
        transform.position = position;

        CollectCoins(position);

        if (Mathf.Abs(position.x - endX) <= 0.01f)
        {
            Completed = true;
        }
    }

    public void ResetRun()
    {
        InitializePath();

        Vector3 position = transform.position;
        position.x = startX;
        transform.position = position;

        Completed = false;
        ResetCoins();
    }

    private void InitializePath()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
        {
            initialized = false;
            return;
        }

        Vector3 left = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, cam.nearClipPlane));
        Vector3 right = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, cam.nearClipPlane));

        startX = left.x + horizontalMargin;
        endX = right.x - horizontalMargin;
        initialized = true;
    }

    private void CollectCoins(Vector3 position)
    {
        if (coins == null)
        {
            return;
        }

        for (int i = 0; i < coins.Length; i++)
        {
            Transform coin = coins[i];
            if (coin == null || !coin.gameObject.activeSelf)
            {
                continue;
            }

            if (Vector3.Distance(position, coin.position) <= pickupRadius)
            {
                coin.gameObject.SetActive(false);
            }
        }
    }

    private void ResetCoins()
    {
        if (coins == null)
        {
            return;
        }

        for (int i = 0; i < coins.Length; i++)
        {
            if (coins[i] != null)
            {
                coins[i].gameObject.SetActive(true);
            }
        }
    }
}
