using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip CollectSFX, CowMilkSFX, GiveSFX, MachineSFX, MoneySFX, PurchasedSFX, TreeSFX, PunchSFX,StackSFX,HitSFX;
    static AudioSource audioSrc;
    void Start()
    {
       
        CollectSFX = Resources.Load<AudioClip>("CollectSFX");
        CowMilkSFX = Resources.Load<AudioClip>("CowMilkSFX");
        GiveSFX = Resources.Load<AudioClip>("GiveSFX");
        MachineSFX = Resources.Load<AudioClip>("MachineSFX");
        MoneySFX = Resources.Load<AudioClip>("MoneySFX");      
        PurchasedSFX = Resources.Load<AudioClip>("PurchasedSFX");
        TreeSFX = Resources.Load<AudioClip>("TreeSFX");
        PunchSFX = Resources.Load<AudioClip>("PunchSFX");
        StackSFX = Resources.Load<AudioClip>("StackSFX");
        HitSFX = Resources.Load<AudioClip>("HitSFX");



        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
        
            case "CollectSFX":
                audioSrc.PlayOneShot(CollectSFX);
                break;
            case "CowMilkSFX":
                audioSrc.PlayOneShot(CowMilkSFX);
                break;
            case "GiveSFX":
                audioSrc.PlayOneShot(GiveSFX);
                break;
            case "MachineSFX":
                audioSrc.PlayOneShot(MachineSFX);
                break;
            case "MoneySFX":
                audioSrc.PlayOneShot(MoneySFX);
                break;      
            case "PurchasedSFX":
                audioSrc.PlayOneShot(PurchasedSFX);
                break;
            case "TreeSFX":
                audioSrc.PlayOneShot(TreeSFX);
                break;
            case "PunchSFX":
                audioSrc.PlayOneShot(PunchSFX);
                break;
            case "StackSFX":
                audioSrc.PlayOneShot(StackSFX);
                break;
            case "HitSFX":
                audioSrc.PlayOneShot(HitSFX);
                break;
        }

    }
}
