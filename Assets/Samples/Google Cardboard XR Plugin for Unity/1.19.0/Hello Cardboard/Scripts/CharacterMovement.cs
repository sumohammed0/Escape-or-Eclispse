using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Import the Photon Unity Networking namespace

public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    CharacterController charCntrl;
    [Tooltip("The speed at which the character will move.")]
    public float speed = 5f;
    [Tooltip("The camera representing where the character is looking.")]
    public GameObject cameraObj;
    [Tooltip("Should be checked if using the Bluetooth Controller to move. If using keyboard, leave this unchecked.")]
    public bool joyStickMode;
    PhotonView view; // Reference to the PhotonView component

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>(); // Get the PhotonView component attached to this GameObject
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if (view.IsMine) // Check if this PhotonView belongs to the local player
        {
            // Get horizontal and vertical movement
            float horComp = Input.GetAxis("Horizontal");
            float vertComp = Input.GetAxis("Vertical");

            if (joyStickMode)
            {
                horComp = Input.GetAxis("Vertical");
                vertComp = Input.GetAxis("Horizontal") * -1;
            }

            Vector3 moveVect = Vector3.zero;

            // Get camera-based directions
            Vector3 cameraLook = cameraObj.transform.forward;
            cameraLook.y = 0f;
            cameraLook = cameraLook.normalized;

            Vector3 forwardVect = cameraLook;
            Vector3 rightVect = Vector3.Cross(forwardVect, Vector3.up).normalized * -1;

            moveVect += rightVect * horComp;
            moveVect += forwardVect * vertComp;

            // Final movement vector
            moveVect *= speed;

            // Move character
            charCntrl.SimpleMove(moveVect);

            // Update animator speed based on movement magnitude
            float animationSpeed = moveVect.magnitude / speed; // Normalized value between 0–1
            animator.SetFloat("Speed", animationSpeed);
        }
    }
}
