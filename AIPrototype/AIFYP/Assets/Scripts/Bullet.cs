using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Playermovement playerMovement;

    private void Update()
    {
        transform.Translate(Vector3.forward * 30f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit");
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit");    
            playerMovement = other.GetComponent<Playermovement>();
            playerMovement.TakeDamage(20f);
        }
        Destroy(this.gameObject);
    }
}
