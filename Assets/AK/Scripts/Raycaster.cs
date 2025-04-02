using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public float maxRaylength = 20f; //
    [SerializeField] private LineRenderer ray;
    private Vector3 endPosition;
    private RaycastHit hit;
    public GameObject startPosition;
    public GameObject endPositionObject; 
    [SerializeField]private ItemHanlder menuAndItemHandler;

    void Update()
    {
        updateRay();
    }

    void updateRay()
    {

        if (ray == null) return;
        Physics.Raycast(transform.position, transform.forward, out hit, maxRaylength);
        ShowEndPointTransfer(hit);
        //endPositionObject.transform.position = endPosition;
        endPositionObject.transform.position = endPosition;
        ray.SetPosition(0, transform.position);
        ray.SetPosition(1, endPosition);
    }
    void ShowEndPointTransfer(RaycastHit hit)
    {
        if (hit.collider)
        {
            endPosition = hit.point;
            if (!menuAndItemHandler.isGrabbing)
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    if (Input.GetButton("js3"))
                        transform.parent.parent.parent.position = new Vector3(hit.point.x,
                                                                            transform.parent.parent.parent.position.y,
                                                                            hit.point.z);
                }
            }
        }
        else
            endPosition = transform.position + transform.forward * maxRaylength;
    }


}