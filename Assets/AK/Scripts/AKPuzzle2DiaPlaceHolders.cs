using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

public class AKPuzzle2DiaPlaceHolders : MonoBehaviour
{
    public GameObject[] PadsPlaceHolderArrays;
    public int[] dialCodes;
    public Image[] ImageComponentArray;
    public int currentIndex = 0;

    //void Start()
    //{
    //    dialCodes = new int[PadsPlaceHolderArrays.Length];
    //    List<Image> collectedImages = new List<Image>();

    //    foreach (GameObject pad in PadsPlaceHolderArrays)
    //    {
    //        if (pad != null)
    //        {
    //            // Get all children (including inactive) recursively and check for Image
    //            Image[] imagesInChildren = pad.GetComponentsInChildren<Image>(true);
    //            collectedImages.AddRange(imagesInChildren);
    //        }
    //    }
    //    ImageComponentArray = collectedImages.ToArray();
    //}
}
