using UnityEngine;
using System.Collections.Generic;
using System.Net.NetworkInformation;
//using UnityEngine.UIElements;
public class AKPuzzle2DialsInitializer : MonoBehaviour
{
    public Sprite[] padsSprites;
    public Dictionary<int, Sprite> dialsMap;
    public AKPuzzle2DiaPlaceHolders[] DialHolders;
    public void Start()
    {
        InstanciateSpritesMap();
        //AssignSpritesRandomToDialsImageComponent();
    }
    private void InstanciateSpritesMap()
    {
        dialsMap = new Dictionary<int, Sprite>();
        for (int i = 0; i < padsSprites.Length; i++)
        {
            dialsMap.Add(i, padsSprites[i]);
        }
    }
    private void  AssignSpritesRandomToDialsImageComponent()
    {
    foreach ( var dial in DialHolders)
        {
            if (dial != null)
            {
                // Get all children (including inactive) recursively and check for Image
                List<int> randomIndexes = new List<int>(dialsMap.Keys);
                for (int i = 0; i < dial.ImageComponentArray.Length; i++)
                {
                    int randomIndex = Random.Range(0, randomIndexes.Count);
                    int selectedKey = randomIndexes[randomIndex];
                    dial.ImageComponentArray[i].sprite = dialsMap[selectedKey];
                    dial.dialCodes[i] = selectedKey;
                    randomIndexes.RemoveAt(randomIndex);
                }
            }
        }
    }
}

