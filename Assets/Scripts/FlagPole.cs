using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;
    public GameObject winUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            StartCoroutine(LevelCompleteSequence(player));
        }
    }

    private IEnumerator LevelCompleteSequence(Player player)
    {
        player.movement.enabled = false;

        if (flag != null && poleBottom != null)
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
        }

        yield return MoveTo(player.transform, castle.position);

        player.gameObject.SetActive(false);

        if (winUI != null)
        {
            WinUIController winController = winUI.GetComponent<WinUIController>();
            if (winController != null)
            {
                winController.ConfigureNextLevel(nextWorld, nextStage);
            }

            winUI.SetActive(true);
        }
        else
        {
            GameManager.Instance.LoadLevel(nextWorld, nextStage);
        }
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }

}
