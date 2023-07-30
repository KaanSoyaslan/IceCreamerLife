using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IceCreamMachine : MonoBehaviour
{

    [Header("============= Collect =============")]
    public GameObject[] Fruits;
    public GameObject  MilkObject;
    public int FruitAmount;


    [Header("============= Check =============")]
    public bool MilkCome = false;
    public bool FruitsCome = false;
    public bool isAvailable = true;


    [Header("============= Give =============")]
    public Transform IceCreamSpawnPosition;
    public GameObject IceCream;


    [Header("============= UI =============")]
    public Image LoadingImage;


    [Header("============= Others =============")]
    public Animator anim;


    public void TakeItem(int Id)
    {
        if(Id != -2)
        {
            Fruits[Id].SetActive(true);
        }
        else
        {
            MilkObject.SetActive(true);

        }
       
        if (FruitAmount == 3 && Fruits[0].activeInHierarchy && Fruits[1].activeInHierarchy && Fruits[2].activeInHierarchy && MilkObject.activeInHierarchy)
        {
            isAvailable = false;
            FruitAmount -= 3;
            MilkCome = false;
            FruitsCome = false;
            StartCoroutine(WorkMachine());
        }

    }
    IEnumerator WorkMachine()
    {
        

        anim.SetInteger("MachinePhase", 1); //load
        yield return new WaitForSeconds(0.5f);
        //delete loaded items

        for (int i = 0; i < Fruits.Length; i++)
        {
            Fruits[i].SetActive(false);
        }
        MilkObject.SetActive(false);

        anim.SetInteger("MachinePhase", 2); //work


       
        for (int i = 0; i < 50; i++) //Ice Cream IMG
        {
            yield return new WaitForSeconds(0.1f);
            LoadingImage.fillAmount += 0.02f;         
        }


        LoadingImage.fillAmount = 1f;

        anim.SetInteger("MachinePhase", 3); //release
                                            //item spawn
        SoundManager.PlaySound("MachineSFX");
        GameObject Ic = Instantiate(IceCream, IceCreamSpawnPosition.position, Quaternion.identity);
        Ic.transform.SetParent(IceCreamSpawnPosition.transform);

     
    }
    public void IceCreamGone()
    {       
        StartCoroutine(IceCreamGoneWaiter());
    }
    IEnumerator IceCreamGoneWaiter()
    {
        LoadingImage.fillAmount = 0f;
        anim.SetInteger("MachinePhase", 4); //Door closed
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("MachinePhase", 0); //Go Idle
        yield return new WaitForSeconds(0.5f);
        isAvailable = true;
    }
}