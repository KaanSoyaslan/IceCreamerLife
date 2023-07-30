using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_apparence : MonoBehaviour
{

    [Header("============= Material IDs =============")]
    public int skinColorID;
    public int hairColorID;
    public int clothesUpColorID;
    public int clothesDownColorID;
    public int extraColorID;
    public int shoeColorID;

    void Start()
    {
 
        GameObject NpcMaterialHolder = GameObject.FindGameObjectWithTag("NpcMaterialHolder");

        SkinnedMeshRenderer renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        Material[] mats = renderer.materials;

        mats[skinColorID] = NpcMaterialHolder.GetComponent<NpcMaterials>().SkinColor();
        mats[hairColorID] = NpcMaterialHolder.GetComponent<NpcMaterials>().HairColor();
        mats[clothesUpColorID] = NpcMaterialHolder.GetComponent<NpcMaterials>().ClothesUpColor();
        mats[clothesDownColorID] = NpcMaterialHolder.GetComponent<NpcMaterials>().ClothesDownColor();
        mats[extraColorID] = NpcMaterialHolder.GetComponent<NpcMaterials>().ExtraColor();
        mats[shoeColorID] = NpcMaterialHolder.GetComponent<NpcMaterials>().ShoeColor();



        renderer.materials = mats;
    }

}
