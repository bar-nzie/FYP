using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private LayerMask blockingLayers;
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, blockingLayers);

        if(colliders.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
