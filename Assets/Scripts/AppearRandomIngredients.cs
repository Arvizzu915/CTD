using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearRandomIngredients : MonoBehaviour
{
    [SerializeField] Transform[] ingredientSpots;
    [SerializeField] GameObject[] ingredients;
    private int spotIndex = 0, ingredientsIndex = 0;
    [SerializeField] private float appearIngredientTime;
    private float appearIngredientTimer = 0;
    private bool AppearedIngredient = false;

    private void Update()
    {
        AppearIngredient();
    }

    void AppearIngredient()
    {
        if (AppearedIngredient)
        {
            appearIngredientTimer = Time.time;
            AppearedIngredient = false;

        }

        if(Time.time - appearIngredientTimer >= appearIngredientTime)
        {
            spotIndex = Random.Range(0, 11);
            ingredientsIndex = Random.Range(0, 5);
            Instantiate(ingredients[ingredientsIndex], ingredientSpots[spotIndex]);
            AppearedIngredient = true;
        }
    }
}
