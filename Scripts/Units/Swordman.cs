using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : Unit
{
	#region Fields
	Animator animationCtrl;
	#endregion

	#region Properties
	public Animator AnimationCtrl { get => animationCtrl; }
	#endregion

	#region Methods
	public override void Initialise()
	{
		base.Initialise();
		animationCtrl = this.transform.GetChild(0).GetComponent<Animator>();
		Move move = this.gameObject.GetComponent<Move>();
		if(move != null)
		{
			skills.Add(move);
		}
		else
		{
			skills.Add(this.gameObject.AddComponent<Move>());
		}
		Strike strike = this.gameObject.GetComponent<Strike>();
		if (strike != null)
		{
			skills.Add(strike);
		}
		else
		{
			skills.Add(this.gameObject.AddComponent<Strike>());
		}
		skills[1].AddListener(PlayAttackAnimaton);
	}

	protected override void Update()
	{
		base.Update();
		if (MyTurn)
		{
			HandleInput();
		}
		float currentSpeedProcent = Agent.velocity.magnitude / Agent.speed;
		animationCtrl.SetFloat("speed", currentSpeedProcent , .1f , Time.deltaTime);
	}

	void HandleInput()
	{
		for (int i = 1; i < skills.Count + 1; i++)
		{
			if (Input.GetKeyDown("" + i))
			{
				if (skills[i -1].Active)
				{
					skills[i -1].Active = false;
				}
				else
				{
					skills[i -1].Active = true;
				}
			}
		}
	}

	void PlayAttackAnimaton()
	{
		animationCtrl.SetTrigger("attack");
	}

	public override void ReceiveDamage(float Amount, Vector3 From)
	{
		AnimationCtrl.SetTrigger("gettingHit");
		base.ReceiveDamage(Amount, From);
	}
	#endregion
}
