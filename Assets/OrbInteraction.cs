using UnityEngine;
using Photon.Pun;

public class OrbInteraction : MonoBehaviourPun
{
    public GameObject orb;
    public bool isOrbAboveChild = false;
    public SolvedPuzzleManager solvedPuzzleManagerScript;
    public float acceptableRange = 2.5f;
    public Transform targetTransform;
    public RaycastHandler raycastHandlerScript;

    private bool hasBeenPlaced = false; // Prevent multiple triggers

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solvedPuzzleManagerScript = GameObject.FindGameObjectWithTag("SolveManager").GetComponent<SolvedPuzzleManager>();
        targetTransform = this.transform.GetChild(1).transform;
        isOrbAboveChild = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (raycastHandlerScript == null)
        {
            raycastHandlerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastHandler>();
        }

        float distance = Vector3.Distance(orb.transform.position, targetTransform.position);

        if (distance <= acceptableRange && !raycastHandlerScript.isGrabbing && !hasBeenPlaced)
        {
            hasBeenPlaced = true;
            isOrbAboveChild = true;

            Debug.Log("Sphere is placed within the acceptable range. Sending RPC.");
            photonView.RPC("RPC_OrbPlaced", RpcTarget.AllBuffered); // Sync across all players
        }
    }

    [PunRPC]
    public void RPC_OrbPlaced()
    {
        Debug.Log("RPC_OrbPlaced called on all players.");
        isOrbAboveChild = true;
        solvedPuzzleManagerScript.solvedPuzzle1(); // Now this runs for all
    }
}
