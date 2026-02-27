using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Playermovement playerMovement;
    private Health enemyHealth;

    private void Update()
    {
        transform.Translate(Vector3.forward * 30f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit");
            enemyHealth = other.GetComponent<Health>();
            enemyHealth.TakeDamage(20f);
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit");    
            playerMovement = other.GetComponent<Playermovement>();
            playerMovement.TakeDamage(20f);
        }
        Debug.Log("Collision Detected");
        Destroy(this.gameObject);
    }
}
