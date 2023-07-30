using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Language : MonoBehaviour
{
    public TMP_Text[] AllTXTS;
    public TMP_Text[] UpgradeTXTS;

    public TMP_Text[] ExpandTXTS;

    void Start()
    {
        if (PlayerPrefs.GetInt("Language") == 0 && Application.systemLanguage == SystemLanguage.Turkish) //first language set   1tr   2en 
        {
            PlayerPrefs.SetInt("Language", 1);

        }
        else if (PlayerPrefs.GetInt("Language") == 0)
        {
            PlayerPrefs.SetInt("Language", 2);
        }
        SetTXTS();
    }


    public void SetTXTS()
    {
        if (PlayerPrefs.GetInt("Language") == 1)
        {

            for (int i = 0; i < UpgradeTXTS.Length; i++)
            {
                UpgradeTXTS[i].text = "Geliþtir";
            }
            for (int i = 0; i < ExpandTXTS.Length; i++)
            {
                ExpandTXTS[i].text = "Geniþlet";
            }

            AllTXTS[0].text = "Dondurma";
            AllTXTS[1].text = "Dondurma";
            AllTXTS[2].text = "Dondurma"; //font
            AllTXTS[3].text = "Meþhur\nDondurma";
            AllTXTS[4].text = "Meþhur\nDondurma";
         
        }
        else
        {
            for (int i = 0; i < UpgradeTXTS.Length; i++)
            {
                UpgradeTXTS[i].text = "Upgrade";
            }
            for (int i = 0; i < ExpandTXTS.Length; i++)
            {
                ExpandTXTS[i].text = "Expand";
            }

            AllTXTS[0].text = "Ice Cream";
            AllTXTS[1].text = "Ice Cream";
            AllTXTS[2].text = "Ice Cream"; //font
            AllTXTS[3].text = "Famous\nIce Cream";
            AllTXTS[4].text = "Famous\nIce Cream";
          
          
        }
    }
    public void SelectLanguageBTN(int LangNum)
    {
        PlayerPrefs.SetInt("Language", LangNum);
        SetTXTS();
    }
}
