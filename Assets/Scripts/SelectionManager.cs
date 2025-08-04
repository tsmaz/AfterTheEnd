using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class SelectionManager : MonoBehaviour {

	public bool onTarget;
	public GameObject interaction_Info_UI;
	Text interaction_text;
	public static SelectionManager Instance { get; set; }
 
	private void Start()
	{
		interaction_text = interaction_Info_UI.GetComponent<Text>();
		onTarget = false;
	}

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		}
		else {
			Instance = this;
		}
			
	}
 
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			var selectionTransform = hit.transform;
 
			if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().playerInRange == true)
			{
				interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
				interaction_Info_UI.SetActive(true);
				onTarget = true;
			}
			else 
			{ 
				interaction_Info_UI.SetActive(false);
				onTarget = false;
			}
 
		}
		else {
			interaction_Info_UI.SetActive(false);
			onTarget = false;
		}
		
	}
}