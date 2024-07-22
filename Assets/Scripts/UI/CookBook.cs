using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookBook : MonoBehaviour
{
    public GameObject[] recipes;
    public Button[] buttons;

    public void EnableRecipeButton(int id)
    {
        buttons[id-1].interactable=true;
    }
    public void EnableRecipe(int id)
    {
        for (int i = 0; i < recipes.Length; i++)
        { recipes[i].SetActive(false); }
        recipes[id-1].SetActive(true);
    }

}
