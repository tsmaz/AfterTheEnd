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
}