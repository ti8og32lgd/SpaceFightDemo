using System;
using script.Buffer;
using script.consts;
using script.DamageCalculate;
using UnityEngine;

namespace script
{
	[AddComponentMenu("MyGame/Bullet")]
	public class Bullet : MonoBehaviour {

		[Header("basic attribute")]
		//子弹飞行速度
		public BufferValue speed =10;
		//生存时间
		public float liveTime=3;
		//初始威力
		public BufferValue damage=25;
		
		
		public event Action<AfterDamageStatus> AfterDamageCallback;
		public EntityType ammoOwner;
		
		protected Transform MTransform;

		public void SetBullet(BufferValue power,BufferValue _speed, EntityType owner,Action<AfterDamageStatus>afterDamage)
		{
			this.speed = _speed;
			this.damage = power;
			this.ammoOwner = owner;
			this.AfterDamageCallback = afterDamage;
		}
		
		// Use this for initialization
		void Start () { 
			MTransform = this.transform;
			Destroy (this.gameObject,liveTime);
		}

		 void Update()
		{
			MTranslateUpdate();
		}


		 public Action MTranslateUpdate;
		 
		
		public virtual void HitOtherEntity(IDamageRecipient damageReceiver,  int damage)
		{
			var afterStat = damageReceiver.BeingDamaged(damage);
			AfterDamageCallback?.Invoke(afterStat);
			//disappear
			Destroy(gameObject);
		}
		public virtual int GiveDamage()
		{
			return (int) damage.ResultVal;
		}
		
		protected void ManualInvokeAfterDamageCallback(AfterDamageStatus afterStat){			
			AfterDamageCallback?.Invoke(afterStat);
			
		}

		///子弹最后消失
		public  void OnDestroy()
		{
		}
	}
}
