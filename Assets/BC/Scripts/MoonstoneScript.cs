using UnityEngine;
using Photon.Pun;
using DoorScript;

public class MoonstoneScript : MonoBehaviourPun
{
    public Transform nearbyEngraving { get; set; } // Changed setter to public for external access

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Engraving"))
        {
            nearbyEngraving = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Engraving"))
        {
            nearbyEngraving = null;
        }
    }

    void OnDrawGizmos()
    {
        if (nearbyEngraving != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, nearbyEngraving.position);
            Gizmos.DrawSphere(nearbyEngraving.position, 0.05f);
        }
    }
}
