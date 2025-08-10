using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InteractableObject : MonoBehaviour {
	public bool playerInRange;
	public string itemName;
 
	public string GetItemName()
	{
		return itemName;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player"))
		{
			playerInRange = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player"))
		{
			playerInRange = false;
		}
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.E) && playerInRange && SelectionManager.Instance.currentTarget == this) {
			// print debug string and destroy gameobject
			InventorySystem.Instance.AddToInventory(itemName);
			Destroy(gameObject);
		}
	}
}