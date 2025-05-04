using UnityEngine;

public class OrbInteraction : MonoBehaviour
{
    public GameObject orb;
    public bool isOrbAboveChild = false;
    public SolvedPuzzleManager solvedPuzzleManagerScript;
    public float acceptableRange = 2.5f;
    public Transform targetTransform;
    public RaycastHandler raycastHandlerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Debug.Log("Start called for OrbInteraction script");
        solvedPuzzleManagerScript = GameObject.FindGameObjectWithTag("SolveManager").GetComponent<SolvedPuzzleManager>();
        targetTransform = this.transform.GetChild(1).transform;
        isOrbAboveChild = false;
        // Debug.Log("white image position: " + targetTransform.position);
    }

    // void Awake() 
    // {
    //     Debug.Log("Awake called for OrbInteraction script");
    //     if (raycastHandlerScript == null)
    //     {
    //         raycastHandlerScript = GameObject.FindGameObjectWithTag("RaycastHandler").GetComponent<RaycastHandler>();
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        if (raycastHandlerScript == null)
        {
            raycastHandlerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastHandler>();
        }
        float distance = Vector3.Distance(orb.transform.position, targetTransform.position);
        // Debug.Log("Distance between orb and target: " + distance + " | Acceptable range: " + acceptableRange);
        // Debug.Log("grabbing = " + raycastHandlerScript.isGrabbing);

        if (distance <= acceptableRange && !raycastHandlerScript.isGrabbing)
        {
            Debug.Log("Sphere is placed within the acceptable range.");
            isOrbAboveChild = true;
            solvedPuzzleManagerScript.solvedPuzzle1();
        }

    }
}
