using System;
using System.Collections;
using System.Collections.Generic;
using script.Buffer;
using script.consts;
using script.DamageCalculate;
using script.PlayerControl;
using script.Spaceship.Ability;
using script.Spaceship.Ability.Equip;
using script.Spaceship.Part;
using script.SpaceshipPart;
using script.UI;
using UnityEngine;

namespace script.Spaceship
{
	// [AddComponentMenu("MyGame/Player")]
	// [RequireComponent(typeof(WeaponsSystem),typeof(Fire))]
	public class Spaceship : MonoBehaviour,IDamageRecipient,IDamageGiver
	{

		[Header("bag")] 
		//飞机的技能
		protected List<SpaceshipAbilityEquip> _abilities=new();
		[Header("Value")]
		public int nowHp=300;
		public int maxHp = 300;
		public BufferValue speed = 1;
		public BufferValue shieldDamage=25;//直接碰撞伤害
		public BufferValue baseDamage = 25;//开火基准伤害
		public BufferValue bulletSpeed=15;

		public float fireCoolDown = 1.0f;
		public float timeSinceLastFire = 1.0f;

		public float hpChangeIndicateScaleFactor = 0.5f;
		[Header("tag")] public EntityType entityType;
		//飞机的部件  
		//主体： 生命值buff
		// public BodyCore BodyCore { get; set; }
		//机翼： 暂时无用
		// public Wing Wing { get; set; }
		// 能量核心： 移动速度buff
		// public EnergyCore EnergyCore { get; set; }
		
		//武器组件  ：控制发射子弹
		private WeaponsController mWeaponController;
		public WeaponsController WeaponsController
		{
			get
			{
				if (mWeaponController == null)
				{
					// Debug.Log("new WeaponsController(transform);");
					mWeaponController = new WeaponsController(transform);
				}

				return mWeaponController;
			}
			set => mWeaponController = value;
		}
		
	
	
		//some reference
		protected Transform m_transform;

		public Shield shield;
		//

		//爆炸特效  TODO  放在单独的函数中处理
		public Transform mExplosionFx;

		public Transform explosionFX
		{
			get
			{
				if (mExplosionFx == null)
				{
					return ProjectVariables.Instance.defaultExplosionFx;
				}

				return mExplosionFx;
			}	
		}


		#region Event

		public event Action<int> OnLifeChanged,OnBeingDamaged; 
		public event Action<AfterDamageStatus> OnCauseDamage;
		public event Action AfterStartEvent,UpdateEvent;
		public event Action OnSpaceshipDestroy;

		
		public void InvokeOnLifeChanged(){OnLifeChanged?.Invoke(Mathf.Clamp(nowHp,0,maxHp));}
		public void InvokeOnBeingDamaged(int d){OnBeingDamaged?.Invoke(d);}
		public void InvokeOnCauseDamage(AfterDamageStatus stat){OnCauseDamage?.Invoke(stat);}
		public void InvokeOnSpaceshipDestroy(){OnSpaceshipDestroy?.Invoke();}
		#endregion


		public List<SpaceshipAbilityEquip> GetAbilities()
		{
			return _abilities;
		}
		// Use this for initialization
		void Start ()
		{
			m_transform = this.transform;

			CreateShield();
			AfterStartEvent?.Invoke();
			// Debug.Log($"Start {name}");
		}
		
		
	
		// Update is called once per frame
		void Update ()
		{
			UpdateEvent?.Invoke();
		}


		

		public virtual void EquipSpaceshipAbility(SpaceshipAbilityEquip abilityEquip)
		{
			abilityEquip.Do(this);
			_abilities.Add(abilityEquip);
		}


		protected void IndicateLifeChange(int changedValue)
		{
			Color indicateColor=changedValue>0?Color.green:Color.red;
			FindObjectOfType<HpChangeDisplay>().Indicate(changedValue,indicateColor, transform.position,hpChangeIndicateScaleFactor);
		}

		public int GiveDamage()
		{
			throw new NotImplementedException();
		}
		public virtual  AfterDamageStatus BeingDamaged(int damage)
		{
			if (nowHp <= 0)//already dead
			{
				return new AfterDamageStatus();
			}
			
			nowHp -= damage;
			OnBeingDamaged?.Invoke(damage);
			OnLifeChanged?.Invoke(Mathf.Clamp(nowHp,0,nowHp));
			IndicateLifeChange(-damage);

			if (nowHp <= 0)
			{
				//TODO  受伤特效
			
				//爆炸特效
				Instantiate(explosionFX, m_transform.position, Quaternion.identity);
				// gameObject.SetActive(false);
				LagDestroySelf(0.3f);
			}
			return new AfterDamageStatus
			{
				IsDead = nowHp <= 0,
				RealDamage = damage
			};
		}

		public void DamagedCallback(AfterDamageStatus status)
		{
			InvokeOnCauseDamage(status);
		}
		public virtual bool BeingHealed(int healValue)
		{
			if (nowHp == maxHp) return false;
			nowHp = Mathf.Clamp(nowHp + healValue, 0, maxHp);
			InvokeOnLifeChanged();
			IndicateLifeChange(healValue);

			return true;

		}
		
		

		


		//创建一层shield进行自身碰撞检测
		public void CreateShield()
		{
			// Debug.Log($"{name} create shield {Time.time}");
			var shieldObj = new GameObject("shield");
			shieldObj.tag = Tags.Shield;
			shieldObj.transform.SetParent(transform);
			shieldObj.transform.localRotation=Quaternion.identity;
			
			shieldObj.transform.localPosition = Vector3.zero;
			shieldObj.transform.localScale=Vector3.one;
			
			shieldObj.AddComponent<Rigidbody>();
			shieldObj.GetComponent<Rigidbody>().useGravity = false;
			shieldObj.GetComponent<Rigidbody>().isKinematic = true;
			
			var meshCollider= shieldObj.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh;
			meshCollider.convex = true;
			meshCollider.isTrigger = true;

			shieldObj.AddComponent<MeshRenderer>();
			var meshFilter  =shieldObj.AddComponent<MeshFilter>();
			meshFilter.mesh= GetComponent<MeshFilter>().mesh;
			
			shieldObj.AddComponent<Shield>();
			shieldObj.GetComponent<Shield>().owner = this;

			this.shield = shieldObj.GetComponent<Shield>();;

		}



		private void LagDestroySelf(float time)
		{
			 IEnumerator _process()
			 {
				 yield return new WaitForSeconds(time);
				 Destroy(gameObject);
			 }

			 StartCoroutine(_process());
		}
		private void OnDestroy()
		{
			OnSpaceshipDestroy?.Invoke();
		}

		
	}
}
