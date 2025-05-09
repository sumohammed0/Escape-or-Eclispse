using UnityEngine;
using TMPro;

public class AKPuzzle2Manager : MonoBehaviour
{
    public GameObject CuberIndicator;
    [SerializeField] private AKPuzzle2DiaPlaceHolders[] Dials;
    [SerializeField] private int[] LockCode = { 1, 3, 4 };
    [SerializeField] private float rotationDuration = 0.1f;
    private int totalNumberOfDials = 3; // Number of dials
    private AKPuzzle2DiaPlaceHolders activeDial;
    private int currentDial;
    private bool isRotating = false;
    public bool isplaingPuzzle2 = false;
    public bool IsSolved = false;
    public TextMeshProUGUI ButtonDescriptionss;
    public GameObject moonstone;
    public GameObject sunStone;
    public SolvedPuzzleManager solvedPuzzleManagerScript; // Add this line

    public void Start()
    {
        sunStone.SetActive(false);
        moonstone.SetActive(false);
        currentDial =  0;
        activeDial = Dials[currentDial];
        Vector3 tempPos = activeDial.transform.position;
        CuberIndicator.transform.position = new Vector3(tempPos.x, tempPos.y , tempPos.z);
    }
    void Update()
    {
        if (!isplaingPuzzle2) return;
        //ButtonDescriptionss.color = Color.yellow;
        ButtonDescriptionss.fontSize = 20; 
        ButtonDescriptionss.text = "\t X : rotate ++\t \n A: move right    Y: move left \n \tB: rotate--";
        isUnlocked();
        handleInput();

    }
    private void handleInput()
    {
        if (isRotating) return;
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetButtonDown("jsA_mine") || Input.GetButtonDown("jsA_partner"))
        {
            RotateDialSmooth(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetButtonDown("jsY_mine") || Input.GetButtonDown("jsY_partner"))
        {
            currentDial = (currentDial + 1) % totalNumberOfDials;
            activeDial = Dials[currentDial];
        }
        // if (Input.GetKeyDown(KeyCode.Keypad4))
        // {
        //     currentDial = (currentDial - 1 + totalNumberOfDials) % totalNumberOfDials;
        //     activeDial = Dials[currentDial];
        // }
        CuberIndicator.transform.position = activeDial.transform.position;
    }
    private void isUnlocked()
    {
        for (int i = 0; i < Dials.Length; i++)
        {
            if (Dials[i].dialCodes[Dials[i].currentIndex] != LockCode[i])
            {
                return;
            }
        }
        Debug.Log("Unlocked");
        IsSolved = true;
        isplaingPuzzle2 = false;
        StartCoroutine(Unlock());
    }
    void RotateDialSmooth(int direction)
    {
        int length_ = activeDial.dialCodes.Length;
        if (direction == 1)
            activeDial.currentIndex = (activeDial.currentIndex + 1) % length_;
        else if (direction == -1)
            activeDial.currentIndex = (activeDial.currentIndex - 1 + length_) % length_;
        float targetAngle = (360f / length_) * -direction;
        StartCoroutine(RotateOverTime(activeDial.transform, targetAngle));
    }

    System.Collections.IEnumerator RotateOverTime(Transform dialTransform, float targetY)
    {
        isRotating = true;
        Quaternion startRotation = dialTransform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(targetY, 0f, 0f);
        //Quaternion endRotation = Quaternion.Euler(targetY, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            dialTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        dialTransform.localRotation = endRotation;
        isRotating = false;
    }

    System.Collections.IEnumerator Unlock()
    {
        Debug.Log("Correct sequence entered. Unlocking...");
        yield return new WaitForSeconds(1f);
        sunStone.SetActive(true);
        moonstone.SetActive(true);
        solvedPuzzleManagerScript.solvedPuzzle2();
        gameObject.SetActive(false);
    }
}
