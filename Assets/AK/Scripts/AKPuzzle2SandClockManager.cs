using UnityEngine;

public class AKPuzzle2SandClockManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Light RoomLight;
    [SerializeField] AKFlashLight flashLigth;
    [SerializeField] AKPuz2SandClockClueUIManager ClueManager;
    private float rotationDuration = 1f; // Duration of the flip in seconds
    bool Fliped = false;
    bool IsSolved = false;

    void Start()
    {
        if (!RoomLight)
            throw new System.Exception("Plesae Assign the light related to puzzle 2 to RoomLight.");
    }
    public void CheckSolver()
    {
        if (!RoomLight.enabled && flashLigth.isOn && !IsSolved)
        {
            Debug.Log(" Puzzle 2 sandclock clue revealed");
            IsSolved = true;
            ClueManager.StartFadeSequence();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(" Puzzle 2 sandclock clue not revealed");
        }
    }
    public void FlipSandClock()
    {
        if (!IsSolved)
        {
            StartCoroutine(FilppCortuine());
        }
    }
    private System.Collections.IEnumerator FilppCortuine()
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, 180f);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }
        transform.localRotation = endRotation;
        CheckSolver();
    }

}
