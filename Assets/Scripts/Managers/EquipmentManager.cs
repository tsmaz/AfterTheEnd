using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager Instance { get; set; }

	public GameObject equipVisualSlot;
	public GameObject equippedItem;

	private void Awake()
	{
		if ( Instance != null && Instance != this )
		{
			Destroy( gameObject );
		}
		else
		{
			Instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		equippedItem = null;
	}

	// Update is called once per frame
	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.R ) )
		{
			UnequipItem();
		}
	}

	public void EquipItem( GameObject itemToEquip )
	{
		// Check if the player has the required item in their inventory
		bool hasRequiredItem = true; // TODO: Replace with actual inventory check

		if ( hasRequiredItem && equippedItem == null )
		{
			// Instantiate the equipped item at the visualslot's position and rotation
			if ( itemToEquip != null && equipVisualSlot != null )
			{
				equippedItem = Instantiate( itemToEquip, equipVisualSlot.transform );
			}
			else
			{
				Debug.LogWarning( "Missing EquippedItemPrefab or EquipVisualSlot reference!" );
			}
		}
	}

	public void UnequipItem()
	{
		if ( equippedItem != null )
		{
			Destroy( equippedItem );
		}
	}
}