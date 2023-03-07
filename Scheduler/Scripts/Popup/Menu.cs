using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : Popup
{
    public void OnClick(int number)
    {
        switch (number)
        {
            case 0:
                {
                    Application.OpenURL("https://myar.tistory.com/26");
                }
                break;

            case 1:
                {
                    Application.OpenURL("https://docs.google.com/spreadsheets/d/1vhST2Amhp8MUSXbA0wdGayUM51LWfluzmYmLv3PMq0U/edit?usp=sharing");
                }
                break;

            case 2:
                {
                    Application.OpenURL("https://loa.icepeng.com/imprinting");
                }
                break;
        }
    }
}
