
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using script;
using script.consts;
using script.DamageCalculate;
using script.input;
using script.PlayerControl;
using script.Scene;
using script.Spaceship;
using script.Spaceship.Ability;
using script.Spaceship.Ability.Equip;
using script.Spaceship.Part;
using script.SpaceshipPart;
using UnityEngine.InputSystem;
using Color = UnityEngine.Color;

[DefaultExecutionOrder(150)]
public class Player : Spaceship
{
	//鼠标射线碰撞层
	// Use this for initialization
	// void Start()
	// {
	// 	
	// 	Debug.Log("start spaceship "+name);
	//
	// }
	
	// Update is called once per frame
	void Update ()
	{
	}

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		WeaponsController = new PlayerWeaponsController(transform);
	}
	

	public void EnableInput()
	{
		var action = PlayerInputHandle.Instance.m_playerTouchControl.Touch.TouchPress;
		action.Enable();
		GetComponent<Move>().speed = speed;
		GetComponent<Fire>().weaponsController = WeaponsController;
		GetComponent<Fire>().SetCoolDownTime(fireCoolDown);
		GetComponent<Move>().ListenPressAction(action);
		GetComponent<Fire>().ListenFireAction(action);
	}

	public void DisableInput()
	{
		if (PlayerInputHandle.Instance == null)
		{
			return;
		}
		var action = PlayerInputHandle.Instance.m_playerTouchControl.Touch.TouchPress;
		action.Disable();
	}
	
		
	public override  AfterDamageStatus BeingDamaged(int damage)
	{
		Debug.Log("Player being damage");
		nowHp -= damage;
		InvokeOnBeingDamaged(damage);
		Debug.Log("Player InvokeOnBeingDamaged ");

		InvokeOnLifeChanged();
		Debug.Log("Player InvokeOnLifeChanged ");

		IndicateLifeChange(-damage);
		Debug.Log("Player IndicateLifeChange ");

		if (nowHp <= 0)
		{
			//TODO  受伤特效
			//爆炸特效
			Instantiate(explosionFX, m_transform.position, Quaternion.identity);
			GameManager.Gameover();
			gameObject.SetActive(false);
			
			// Destroy(this.gameObject);
		}

		return new AfterDamageStatus();//TODO
	}

	public override void EquipSpaceshipAbility(SpaceshipAbilityEquip abilityEquip)
	{
		abilityEquip.Do(this);
		_abilities.Add(abilityEquip);
	}
	

}
