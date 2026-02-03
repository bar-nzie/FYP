using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Playermovement playerMovement;
    private void Start()
    {
        Debug.Log("Spawned Bullet");
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<Playermovement>();
            playerMovement.TakeDamage(20f);
        }
        Destroy(this.gameObject);
    }
}
