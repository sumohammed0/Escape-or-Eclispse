using UnityEngine;
using TMPro;
public class AK_DrawerInteraction : MonoBehaviour

{
    public AK_Drawer drawer;

    private bool isPlayerNearby = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            drawer.ShowOutline(true);
            isPlayerNearby = true;
            Debug.Log("Player is nearby");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            drawer.ShowOutline(false);
            isPlayerNearby = false;
            Debug.Log("Player is out of boundry");

        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("jsX_mine") || Input.GetButtonDown("jsX_partner"))
        {
            drawer.ToggleDrawer();
        }
    }
}
