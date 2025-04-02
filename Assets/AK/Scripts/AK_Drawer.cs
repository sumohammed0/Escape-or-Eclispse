using UnityEngine;
using System.Collections;
public class AK_Drawer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform startPoint;
    public Transform endPoint;

    public float moveSpeed = 2f;

    private bool isOpen = false;
    private bool isMoving = false;
    private Outline outline;

    private void Start()
    {
        transform.position = startPoint.position;
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }

    public void ToggleDrawer()
    {
        if (!isMoving)
            StartCoroutine(MoveDrawer());
    }

    private IEnumerator MoveDrawer()
    {
        isMoving = true;

        Vector3 targetPos = isOpen ? startPoint.position : endPoint.position;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isOpen = !isOpen;
        isMoving = false;
    }

    public void ShowOutline(bool show)
    {
        if (outline != null)
            outline.enabled = show;
    }
}
