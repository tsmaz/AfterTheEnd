using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
	public static PlayerStatus Instance { get; set; }

	// Player Stats
	public float currentHealth;
	public float maxHealth;

	public float currentHunger;
	public float maxHunger;

	public float currentHydrationLevel;
	public float maxHydrationLevel;

	public GameObject player;

	private float distanceTraveled = 0;
	private Vector3 lastPosition;

	// Damage settings
	public float damageIntervalTime = 10f;
	public float lowThreshold = 10f;
	public float healthDecreaseAmount = 5f;

	private Coroutine damageCoroutine;

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

	private void Start()
	{
		currentHealth = maxHealth;
		currentHunger = maxHunger;
		currentHydrationLevel = maxHydrationLevel;

		lastPosition = player.transform.position;

		StartCoroutine( decreaseHydration() );
	}

	IEnumerator decreaseHydration()
	{
		while ( true )
		{
			currentHydrationLevel -= 10;
			currentHydrationLevel = Mathf.Max( currentHydrationLevel, 0 );
			yield return new WaitForSeconds( 2 );
		}
	}

	IEnumerator DamageOverTime()
	{
		while ( true )
		{
			// If either is low, apply damage
			if ( currentHunger <= lowThreshold || currentHydrationLevel <= lowThreshold )
			{
				currentHealth -= healthDecreaseAmount;
				currentHealth = Mathf.Max( currentHealth, 0 );
			}
			else
			{
				// Both are now fine � stop damaging
				damageCoroutine = null;
				yield break;
			}

			yield return new WaitForSeconds( damageIntervalTime );
		}
	}

	void Update()
	{
		// Hunger decreases by distance moved
		distanceTraveled += Vector3.Distance( player.transform.position, lastPosition );
		lastPosition = player.transform.position;

		if ( distanceTraveled >= 5 )
		{
			distanceTraveled = 0;
			currentHunger -= 1;
			currentHunger = Mathf.Max( currentHunger, 0 );
		}

		// Start health damage if conditions are met and coroutine isn't already running
		if ( ( currentHunger <= lowThreshold || currentHydrationLevel <= lowThreshold ) && damageCoroutine == null )
		{
			damageCoroutine = StartCoroutine( DamageOverTime() );
		}

		// Manual health test key
		if ( Input.GetKeyDown( KeyCode.N ) )
		{
			currentHealth -= 10;
			currentHealth = Mathf.Max( currentHealth, 0 );
		}
	}
	
	private void HealPlayer( float amount )
	{
		currentHealth += amount;
		currentHealth = Mathf.Min( currentHealth, maxHealth );
	}
	
	private void FeedPlayer( float amount )
	{
		currentHunger += amount;
		currentHunger = Mathf.Min( currentHunger, maxHunger );
	}
	
	private void HydratePlayer( float amount )
	{
		currentHydrationLevel += amount;
		currentHydrationLevel = Mathf.Min( currentHydrationLevel, maxHydrationLevel );
	}

	public void ConsumeItem( Consumable item )
	{
		if ( item == null )
		{
			Debug.LogWarning( "Item is null, cannot consume." );
			return;
		}

		if ( item.hungerRestored != 0 )
		{
			FeedPlayer( item.hungerRestored );
		}
		
		if ( item.thirstRestored != 0 )
		{
			HydratePlayer( item.thirstRestored );
		}
		
		if ( item.healthRestored != 0 )
		{
			HealPlayer( item.healthRestored );
		}
		
		Destroy(item.gameObject);
	}
}