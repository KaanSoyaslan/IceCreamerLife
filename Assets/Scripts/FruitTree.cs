using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    public int phase = 4;
    public GameObject Fruit;

    public Animator anim;
    public GameObject[] Fruits; //for position
    public GameObject TagObject; //for waiting

    void Start()
    {
        RandomMix(Fruits);
    }
    public static void RandomMix(GameObject[] dizi)
    {     
        for (int i = dizi.Length - 1; i > 0; i--)
        {
            int rastgeleIndex = Random.Range(0, i + 1);
            GameObject temp = dizi[i];
            dizi[i] = dizi[rastgeleIndex];
            dizi[rastgeleIndex] = temp;
        }
    }

    public void DropFruit()
    {
        if (phase >= 0)    
        {
            SoundManager.PlaySound("RockDownSFX");//hit sfx
            Instantiate(Fruit, Fruits[phase].transform.position, Quaternion.identity);
            Fruits[phase].SetActive(false);
            phase--;

            if (phase < 0)
            {
                TagObject.transform.localPosition = new Vector3(0, -150, 0);
                StartCoroutine(TreeReload());
            }
        }



    }
    IEnumerator TreeReload()
    {

        SoundManager.PlaySound("TreeSFX");
        
        anim.SetBool("isReloading", true);


        yield return new WaitForSeconds(3f);

        for (int i = 0; i < Fruits.Length; i++)
        {
            Fruits[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f); //waiting for grow fruits

        RandomMix(Fruits);

        phase = 4;

      
        anim.SetBool("isReloading", false);
        TagObject.transform.localPosition = new Vector3(0, 0, 0);

    }
}
