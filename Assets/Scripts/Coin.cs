using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            Destroy(gameObject);
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(coinPickupSound, transform.position);
        }
    }
}
