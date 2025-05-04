using UnityEngine;

public class orbHandler : MonoBehaviour
{
    GameObject orb;
    public bool isPuzzle1Solved = false; // Flag to check if the puzzle is solved
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orb = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPuzzle1Solved)
        {
            Debug.Log("Puzzle 1 is solved, activating orb");
            // Check if the orb is not already active
            orb.SetActive(true);
            Debug.Log("Orb activated");
            this.enabled = false; // Disable this script to prevent further updates
        }
    }
}
