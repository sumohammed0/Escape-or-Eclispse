using UnityEngine;

public class OrbInteraction : MonoBehaviour
{
    public GameObject orb;
    public bool isOrbAboveChild = false;
    public SolvedPuzzleManager solvedPuzzleManagerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solvedPuzzleManagerScript = GameObject.FindGameObjectWithTag("SolveManager").GetComponent<SolvedPuzzleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the child of the child of the current GameObject
        Transform childOfChild = transform.GetChild(0).GetChild(0);

        // Check if the orb is directly above the child of the child
        if (Mathf.Approximately(orb.transform.position.x, childOfChild.position.x) &&
            Mathf.Approximately(orb.transform.position.z, childOfChild.position.z) &&
            orb.transform.position.y > childOfChild.position.y)
        {
            isOrbAboveChild = true;
            Debug.Log("Orb is above the child of the child.");
            solvedPuzzleManagerScript.solvedPuzzle1();
        }
        else
        {
            isOrbAboveChild = false;
        }

        if (orb.transform.position.y < 0)
        {
            orb.transform.position = new Vector3(orb.transform.position.x, 0, orb.transform.position.z);
        }

    }
}
