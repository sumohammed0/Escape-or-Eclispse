using UnityEngine;
using System.Collections;


public class SolvedPuzzleManager : MonoBehaviour
{
    public bool puzzle1Solved = false;
    public bool puzzle2Solved = false;
    public bool puzzle4Solved = false;
    private GameObject puzzle1Canvas;
    private GameObject puzzle2Canvas;
    private GameObject puzzle4Canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puzzle1Solved = false;
        puzzle1Canvas = GameObject.FindWithTag("Puzzle1Canvas");
        puzzle2Canvas = GameObject.FindWithTag("Puzzle2Canvas");
        puzzle4Canvas = GameObject.FindWithTag("Puzzle4Canvas");
        puzzle1Canvas.SetActive(true);
        puzzle2Canvas.SetActive(false);
        puzzle4Canvas.SetActive(false);
        Debug.Log("Puzzle Manager Initialized");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void solvedPuzzle1() {
        Debug.Log("Puzzle 1 Solved");
        puzzle1Solved = true;
        puzzle1Canvas.SetActive(false);
        puzzle2Canvas.SetActive(true);

    }

    public void solvedPuzzle2() {
        Debug.Log("Puzzle 2 Solved");
        puzzle2Solved = true;
        puzzle2Canvas.SetActive(false);
        puzzle4Canvas.SetActive(true);
    }

    public void solvedPuzzle4() {
        Debug.Log("Puzzle 4 Solved");
        puzzle4Solved = true;
        puzzle4Canvas.SetActive(false);

    }
}
