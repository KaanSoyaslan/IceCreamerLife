using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMaterials : MonoBehaviour
{
    [Header("============= Materials =============")]
    public Material[] skinColors;
    public Material[] hairColors;
    public Material[] clothesUpColors;
    public Material[] clothesDownColors;
    public Material[] extraColors;
    public Material[] shoeColors;

    public Material SkinColor()
    {
        return skinColors[Random.Range(0, skinColors.Length)];
    }
    public Material HairColor()
    {
        return hairColors[Random.Range(0, hairColors.Length)];
    }
    public Material ClothesUpColor()
    {
        return clothesUpColors[Random.Range(0, clothesUpColors.Length)];
    }
    public Material ClothesDownColor()
    {
        return clothesDownColors[Random.Range(0, clothesDownColors.Length)];
    }
    public Material ExtraColor()
    {
        return extraColors[Random.Range(0, extraColors.Length)];
    }
    public Material ShoeColor()
    {
        return shoeColors[Random.Range(0, shoeColors.Length)];
    }

}
