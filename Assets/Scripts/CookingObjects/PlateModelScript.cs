using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateModelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ingredientsInPlate;
    [SerializeField]
    private GameObject[] spicesInPlate;

    private void Start()
    {
        //EmptyPlate();
    }

    public bool CanPlaceObjectInPlate(int ID)
    {
        //Por ahora solo checa que no este ya ese mismo ingrediente, pero quiza luego haya que limitar el numero de ingredientes que se le pueden poner al plato
        if(ID >= 200 && ID < 300)
        {
            if(ingredientsInPlate[ID-200].activeSelf == true)
            {
                return false;
            }
            else
            {
                ingredientsInPlate[ID - 200].SetActive(true);
                return true;
            }
        }
        else if(ID >= 300 && ID < 400)
        {
            if (spicesInPlate[ID - 300].activeSelf == true)
            {
                return false;
            }
            else
            {
                spicesInPlate[ID - 300].SetActive(true);
                return true;
            }
        }
        return false;
    }

    public void EmptyPlate()
    {
        foreach (GameObject ingredient in ingredientsInPlate)
        {
            ingredient.SetActive(false);
        }
        foreach (GameObject spice in spicesInPlate)
        {
            spice.SetActive(false);
        }
    }

    public  int GetIngredientID()
    {
        //por ahora estas solo te retornan el primer ingrediente que encuentra, esto porque la idea es que solo pueda tener 1 activo a la vez, pero esto puede cambiar
        for (int i = 0; i < ingredientsInPlate.Length; i++)
        {
            if (ingredientsInPlate[i].activeSelf == true)
            {
                return i + 200;
            }
        }
        return -1;
    }

    public int GetSpiceID()
    {
        for (int i = 0; i < spicesInPlate.Length; i++)
        {
            if (spicesInPlate[i].activeSelf == true)
            {
                return i + 300;
            }
        }
        return -1;
    }
}
