using UnityEngine;

public class AKPuzzelOneStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject puzzleOnelocker;// Reference to the locker manager object

    // Update is called once per frame
    public void startPuzzleOne()
    {
        puzzleOnelocker.SetActive(true);// Enable the locker manager
        gameObject.SetActive(false);// Disable the current game object
    }
}
