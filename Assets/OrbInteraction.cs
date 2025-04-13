using UnityEngine;

public class OrbInteraction : MonoBehaviour
{
    public GameObject orb;
    public bool isOrbAboveChild = false;
    public SolvedPuzzleManager solvedPuzzleManagerScript;
    public float acceptableRange = 0.3f;
    public Transform targetTransform;
    public RaycastHandler raycastHandlerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solvedPuzzleManagerScript = GameObject.FindGameObjectWithTag("SolveManager").GetComponent<SolvedPuzzleManager>();
        targetTransform = this.transform.GetChild(0).GetChild(0);
        Debug.Log("white image position: " + targetTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(orb.transform.position, targetTransform.position);

        if (distance <= acceptableRange && !raycastHandlerScript.isGrabbing)
        {
            Debug.Log("Sphere is placed within the acceptable range.");
            // You can trigger logic here, e.g., set a flag or play an animation
            isOrbAboveChild = true;
            solvedPuzzleManagerScript.solvedPuzzle1();
        }

    }
}
