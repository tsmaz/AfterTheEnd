using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySystem : MonoBehaviour {
	public static InventorySystem Instance { get; set; }

	public GameObject inventoryScreenUI;
	public bool isOpen;
	public List<GameObject> slotList = new List<GameObject>();
	public List<string> itemList = new List<string>();
	private GameObject itemToAdd;
	private GameObject slotToEquip;
	public bool isFull;


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		}
		else {
			Instance = this;
		}
	}


	void Start() {
		isOpen = false;
		inventoryScreenUI.SetActive(false);

		PopulateSlotList();
	}


	void Update() {
		if (Input.GetKeyDown(KeyCode.I) && !isOpen) {
			Debug.Log("i is pressed");
			inventoryScreenUI.SetActive(true);
			isOpen = true;
		}
		else if (Input.GetKeyDown(KeyCode.I) && isOpen) {
			inventoryScreenUI.SetActive(false);
			isOpen = false;
		}
	}

	private void PopulateSlotList() {
		foreach (Transform child in inventoryScreenUI.transform) {
			if (child.CompareTag("Slot")) {
				slotList.Add(child.gameObject);
			}
		}
	}

	public void AddToInventory(string itemName) {
		if (checkIfFull()) {
			Debug.Log("Inventory full");
		}
		else {
			slotToEquip = findNextEmptySlot();
			itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), 
				slotToEquip.transform.position, slotToEquip.transform.rotation, slotToEquip.transform);
			itemList.Add(itemName);
		}
	}

	public void DropItem(GameObject item) {
		
	}

	private GameObject findNextEmptySlot() {
		foreach (GameObject slot in slotList) {
			if (slot.transform.childCount == 0) {
				return slot;
			}
		}

		Debug.Log("No Empty slot");
		return null;
	}

	private bool checkIfFull() {

		int counter = 0;

		foreach (GameObject slot in slotList) {
			if (slot.transform.childCount > 0)
			{
				counter++;
			}
		}
		
		if (counter == 55) {
			return true;
		}
		else {
			return false;
		}
	}
}