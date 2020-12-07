﻿using DG.Tweening;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
	public int maxHealth = 10;
	public int currentHealth { get; private set; }

	public StatsUI statsUI;
	public PlayerController player;

	public int ammoAbility = 0;
	public int damage;
	public int armor = 0;

	[SerializeField]
	AudioClip[] audioClips;


	private void Start()
	{
		currentHealth = maxHealth - 3;
		statsUI.SetMaxHealth(maxHealth);
		statsUI.SetHealth(currentHealth);
		statsUI.SetAmmo(ammoAbility);
		statsUI.SetArmor(armor);
		GetComponentInChildren<Renderer>().sharedMaterial.color = Color.white;
	}


	public void TakeDamage(int damage)
	{

		if (armor - damage >= 0)
		{
			armor -= damage;
		}
		else
		{
			DungeonMaster.Instance.player.PlaySpeakSound(audioClips[Random.Range(0, audioClips.Length)]);
			damage -= armor;
			armor = 0;
			currentHealth -= damage;
		}

		statsUI.SetArmor(armor);
		statsUI.SetHealth(currentHealth);
		GetComponentInChildren<Renderer>().sharedMaterial.DOColor(Color.red, 0.1f);
		Invoke(nameof(ResetColor), 0.2f);


		if(armor <= 0)
		{
			// Check for Helmet
			// Destroy Helmet
			Destroy(GetComponent<PlayerController>().equippedArmor);
		}

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void ResetColor()
	{
		GetComponentInChildren<Renderer>().sharedMaterial.DOColor(Color.white, 0.1f);
	}

	public void Heal(int healingAmount)
	{
		if(healingAmount + currentHealth <= maxHealth)
		{
			currentHealth += healingAmount;
		}
		else
		{
			currentHealth = maxHealth;
		}

		statsUI.SetHealth(currentHealth);
	}

	public void Armor(int armorAmount)
	{
		if (armorAmount + armor <= maxHealth)
		{
			armor = armorAmount;
		}
		statsUI.SetArmor(armor);
	}

	public void UpgradeMaxHealth(int upgradeAmount)
	{
		maxHealth += upgradeAmount;
		statsUI.SetMaxHealth(maxHealth);
	}

	public void SetAbilityAmmo(int grenadeAmount)
	{
		ammoAbility += grenadeAmount;
		statsUI.SetAmmo(ammoAbility);
	}

	public void ResetAmmo()
	{
		ammoAbility = 0;
		statsUI.SetAmmo(ammoAbility);
	}

	public virtual void Die()
	{ 
		DungeonMaster.Instance.deathMenu.ShowMenu();
		currentHealth = 9;
		maxHealth = 12;
		ammoAbility = 0;
	}
}
