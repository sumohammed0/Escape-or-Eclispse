using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody), typeof(PhotonView))]
public class EnhancedPhotonRigidbodySync : MonoBehaviourPun, IPunObservable
{
    private Rigidbody rb;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Vector3 networkVelocity;
    private Vector3 networkAngularVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this object: send the others our data
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.linearVelocity);
            stream.SendNext(rb.angularVelocity);
            stream.SendNext(rb.isKinematic);
        }
        else
        {
            // Network player, receive data
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            networkVelocity = (Vector3)stream.ReceiveNext();
            networkAngularVelocity = (Vector3)stream.ReceiveNext();
            rb.isKinematic = (bool)stream.ReceiveNext();

            // Only apply if we don't own the object
            if (!photonView.IsMine)
            {
                rb.position = networkPosition;
                rb.rotation = networkRotation;
                rb.linearVelocity = networkVelocity;
                rb.angularVelocity = networkAngularVelocity;
            }
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            // Smooth interpolation for non-owned objects
            rb.position = Vector3.Lerp(rb.position, networkPosition, Time.deltaTime * 10);
            rb.rotation = Quaternion.Lerp(rb.rotation, networkRotation, Time.deltaTime * 10);
        }
    }
}