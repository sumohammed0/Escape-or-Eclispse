using UnityEngine;
using TMPro;
using Photon.Pun;

public class AKPuzzleOneLockerManager : MonoBehaviourPun
{
    public int maxDigits = 8;
    public string correctSequence = "21678543";
    public GameObject lockedObject;
    private string currentInput = "";

    [SerializeField] private TMP_Text displayText;
    [SerializeField] private AudioSource pressAudioSource;
    [SerializeField] private GameObject DrawerOneLocker;
    [SerializeField] private GameObject DialPad;

    public CharacterMovement characterMovement;
    public bool isPlayingPuzzle1 = false;
    public orbHandler orbHandlerScript;

    private bool puzzleAlreadySolved = false;

    public void Start()
    {
        displayText.text = "";
        if (correctSequence.Length != maxDigits)
            throw new System.Exception("Correct sequence length mismatch.");
        foreach (char c in correctSequence)
            if (!char.IsDigit(c))
                throw new System.Exception("Correct sequence must be digits.");
    }

    public void Update()
    {
        if (!isPlayingPuzzle1 || puzzleAlreadySolved) return;

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("jsmenu_mine") || Input.GetButtonDown("jsmenu_partner"))
        {
            ExitPuzzle();
        }

        currentInput = displayText.text;
        CheckSequence(currentInput);
    }

    public void AddDigit(int digit)
    {
        if (currentInput.Length < maxDigits)
        {
            currentInput += digit.ToString();
            UpdateDisplay();
            if (pressAudioSource)
                pressAudioSource.Play();
        }
    }

    public void DeleteLastDigit()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }

    public void CheckSequence(string enteredSequence)
    {
        if (enteredSequence == correctSequence && !puzzleAlreadySolved)
        {
            puzzleAlreadySolved = true;
            photonView.RPC("RPC_PuzzleOneSolved", RpcTarget.AllBuffered);
        }
    }

    private void ExitPuzzle()
    {
        DrawerOneLocker.SetActive(true);
        DialPad.SetActive(false);
        characterMovement.enabled = true;
        isPlayingPuzzle1 = false;
    }

    [PunRPC]
    public void RPC_PuzzleOneSolved()
    {
        Debug.Log("Puzzle 1 solved. Syncing to all players.");
        if (lockedObject != null)
            lockedObject.layer = LayerMask.NameToLayer("Interactable");

        orbHandlerScript.isPuzzle1Solved = true;

        // Call local inventory update
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
        if (inventory != null)
            inventory.RPC_AddPuzzle1ItemsToInventory();

        ExitPuzzle();
        gameObject.SetActive(false);
    }
}
