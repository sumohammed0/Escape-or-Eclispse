using UnityEngine;

public class Puzzle2SandClockManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject RoomLight;
    [SerializeField] AKFlashLight flashLigth;
    private float rotationDuration = 1f; // Duration of the flip in seconds
    bool Fliped = false;

    void Start()
    {
        if (!RoomLight)
            RoomLight = GameObject.Find("RoomLight");
        bool hasLightComponent = RoomLight.GetComponent<Light>() != null;
        if (!hasLightComponent)
            throw new System.Exception("RoomLight does not have a Light component.");
    }
    public void TestIsSolved()
    {
        if (RoomLight.activeSelf == false && flashLigth.isOn && Fliped)
        {
            Debug.Log(" Puzzle 2 sandclock clue revealed");
            gameObject.SetActive(false);
        }
    }
    System.Collections.IEnumerator Filpp()
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(180, 0f, 0f);
        //Quaternion endRotation = Quaternion.Euler(targetY, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.localRotation = endRotation;
        Fliped = true;
        TestIsSolved();
    }

}
