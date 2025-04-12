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

    void PlaceOnEngraving(Transform engraving)
    {
        if (!photonView.IsMine) return;

        // Anchor the moonstone to the engraving
        transform.position = engraving.position;
        transform.rotation = engraving.rotation;
        transform.SetParent(engraving);

        // Notify other players
        photonView.RPC("SyncPlacement", RpcTarget.AllBuffered, engraving.position, engraving.rotation, engraving.GetComponent<PhotonView>().ViewID);

        // Notify the door (NETWORKED)
        Door door = FindObjectOfType<Door>(); // Or use a direct reference
        if (door != null) 
        {
            door.photonView.RPC("RPC_NotifyMoonstonePlaced", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void SyncPlacement(Vector3 position, Quaternion rotation, int engravingViewID)
    {
        PhotonView engravingPV = PhotonView.Find(engravingViewID);
        if (engravingPV != null)
        {
            Transform engravingTransform = engravingPV.transform;
            transform.position = position;
            transform.rotation = rotation;
            transform.SetParent(engravingTransform);

            // Disable physics (if applicable)
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;

            Collider col = GetComponent<Collider>();
            if (col) col.enabled = false;
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
