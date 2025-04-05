using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement_Script : MonoBehaviour
{
    public bool isSelected = false;
    public bool clikedOnMe = false;

    private void Start()
    {

    }



     void OnMouseDown()
    {
        if (isSelected == false)
        {
            SelectUnit();
        }
        clikedOnMe = true;
        Debug.Log("Unit have been clicked");
    }

    private void OnMouseUp()
    {
        clikedOnMe = false;
        Debug.Log("Mouse up");
    }

    private  void SelectUnit()
    {
        MouseClickHandeler.onLeftClick += DeSelectUnit;
        isSelected = true;
    }

    private void DeSelectUnit()
    {
        if (isSelected == true && clikedOnMe == false)
        {
            Debug.Log("De-selected");
            isSelected = false;
            MouseClickHandeler.onLeftClick -= DeSelectUnit;
        }
    }

 


    //OnMouseDawn() not on mesh{
    //
    //   isSelected = false;
    //   unsebscribe from delegate
    //}

}
