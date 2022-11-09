using System;
using script.DamageCalculate;
using script.Spaceship.Part;
using UnityEngine;

namespace script.Spaceship
{
	// [AddComponentMenu("MyGame/Enemy")]
	public class Enemy : Spaceship
	{
		

		//移动速度
		//旋转速度
		public float rotSpeed=30;

		public GameObject attackTarget;


		public Action<Enemy> OnEnemyDead; 
		
		// protected virtual void UpdateMove ()
		// {
		// 	//左右移动
		// 	var rx = Mathf.Sin (Time.time)*Time.deltaTime;
		// 	//前进
		// 	m_transform.Translate (new Vector3(rx,0,-(float)speed.ResultVal*Time.deltaTime));
		// }
		public void OnEnable()
		{
			WeaponsController = new EnemyWeaponController(transform);
		}
		

		public override AfterDamageStatus BeingDamaged(int damage)
		{
			
			// Debug.Log($"{name} receive damage [{damage}], now hp is [{health}]");
			var stat= base.BeingDamaged(damage);
			if (stat.IsDead)
			{
				OnEnemyDead?.Invoke(this);
			}
			return stat;
		}
	}
}
