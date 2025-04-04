using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public bool isOpen { get; private set; } // Expose isOpen as a public property
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + new Vector3(0.5f, 0, 0); // Shift 0.5 units on the X-axis
    }

    public void ToggleDrawer()
    {
        transform.localPosition = isOpen ? closedPosition : openPosition;
        isOpen = !isOpen;
    }
}
