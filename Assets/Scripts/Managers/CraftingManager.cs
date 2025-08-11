using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingManager : MonoBehaviour
{
    
    public GameObject craftingScreenUI;
    public GameObject craftingToolsSubmenuUI;
    [FormerlySerializedAs("isOpen")] public bool mainMenuIsOpen;
    public bool subMenuIsOpen;
    // Start is called before the first frame update



    public static CraftingManager Instance { get; set; }
    
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }
    void Start()
    {
        craftingScreenUI.SetActive(false);
        craftingToolsSubmenuUI.SetActive(false);
        mainMenuIsOpen = false;
        subMenuIsOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !mainMenuIsOpen) {
            Debug.Log("c is pressed");
            craftingScreenUI.SetActive(true);
            craftingToolsSubmenuUI.SetActive(false);
            mainMenuIsOpen = true;
            subMenuIsOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.C) && mainMenuIsOpen) {
            craftingScreenUI.SetActive(false);
            mainMenuIsOpen = false;
            craftingToolsSubmenuUI.SetActive(false);
            subMenuIsOpen = false;
        }
        
    }

    public void TryCraftRecipe(string recipeName) {
        
        InventorySystem playerInventory = FindObjectOfType<InventorySystem>();
        RecipeBase recipe = Resources.Load<RecipeBase>("Recipes/" + recipeName);
        bool removingItemsSucceeded = false;

        if (playerInventory.isFull) {
            PopupSpawner.Instance.SpawnPopup(new Vector3(0, 0, 0), "Craft failed: inventory is full.");
            return;
        }
        
        if (playerInventory.HasRequiredItems(recipe) == true)
        {
            foreach (var item in recipe.requiredItems) {
                removingItemsSucceeded = playerInventory.RemoveFromInventory(item);
            }

            if (removingItemsSucceeded) {
                playerInventory.AddToInventory(recipe.resultingItem);
                PopupSpawner.Instance.SpawnPopup(new Vector3(0, 0, 0), "Crafting successful: " + recipe.resultingItem);
            }
            else {
                Debug.Log("TryCraftRecipe: Crafting failed: Could not remove required items from inventory.");
            }
        }
        else {
            PopupSpawner.Instance.SpawnPopup(new Vector3(0, 0, 0), "Craft failed: Missing required items.");
        }
    }
    
    public void OpenCraftingToolsSubmenu() {
        craftingToolsSubmenuUI.SetActive(true);
        mainMenuIsOpen = false;
        subMenuIsOpen = true;
    }
    
}
