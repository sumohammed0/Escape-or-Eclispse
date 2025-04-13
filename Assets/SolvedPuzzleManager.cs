using UnityEngine;

public class SolvedPuzzleManager : MonoBehaviour
{
    public bool puzzle1Solved = true;
    public bool puzzle2Solved = false;
    public bool puzzle4Solved = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puzzle1Solved = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void solvedPuzzle1() {
        Debug.Log("Puzzle 1 Solved");
        puzzle1Solved = true;
    }

    public void solvedPuzzle2() {
        Debug.Log("Puzzle 2 Solved");
        puzzle2Solved = true;
    }

    public void solvedPuzzle4() {
        Debug.Log("Puzzle 4 Solved");
        puzzle4Solved = true;
    }
}
