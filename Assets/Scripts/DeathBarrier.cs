using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.RegisterDeath(true, other.transform.position);
            other.gameObject.SetActive(false);
            GameManager.Instance?.GameOver();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }

}
