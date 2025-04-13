using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviourPun
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        private static int placedMoonstones = 0; // Reverted to static
        private static bool isInteractable = false; // Reverted to static

        private new PhotonView photonView;

        void Start()
        {
            photonView = GetComponent<PhotonView>();
            asource = GetComponent<AudioSource>();
        }

        void Update()
        {
            //Debug.Log("the number of MOON STONES PLACED"+ placedMoonstones);
            if (open)
            {
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            }
            else
            {
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
            }
        }

        [PunRPC]
        public void increaseNumberOfMoonstones()
        {
            placedMoonstones++;
            Debug.Log($"Placed Moonstones: {placedMoonstones} (IsMine: {photonView.IsMine})");
            if (placedMoonstones >= 2 && !isInteractable)
            {
                isInteractable = true;
                Debug.Log("Door is now interactable!");
            }
        }

        [PunRPC]
        public void RPC_NotifyMoonstonePlaced()
        {
            increaseNumberOfMoonstones();
        }

        public void NotifyMoonstonePlaced()
        {
            photonView.RPC("increaseNumberOfMoonstones", RpcTarget.AllBuffered);
        }

        public int PlacedMoonstonesCount => placedMoonstones; // Getter for placedMoonstones count

        public void OpenDoor()
        {
            if (isInteractable)
            {
                open = !open;
                asource.clip = open ? openDoor : closeDoor;
                asource.Play();
            }
        }
    }
}