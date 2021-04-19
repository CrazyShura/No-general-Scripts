using UnityEngine;

public class Imp : Unit
{
	#region Fields
	float maxSpeed;

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
		maxSpeed = Agent.speed;
		Move move = this.gameObject.GetComponent<Move>();
		if (move != null)
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
		float currentSpeedProcent = Agent.velocity.magnitude / maxSpeed;
		animationCtrl.SetFloat("speed", currentSpeedProcent, .1f, Time.deltaTime);
	}

	void HandleInput()
	{
		for (int i = 1; i < skills.Count + 1; i++)
		{
			if (Input.GetKeyDown("" + i))
			{
				if (skills[i - 1].Active)
				{
					skills[i - 1].Active = false;
				}
				else
				{
					foreach(Skill skill in skills)
					{
						if(skill.Active)
						{
							skill.Active = false;
						}
					}
					skills[i - 1].Active = true;
				}
			}
		}
	}

	void PlayAttackAnimaton()
	{
		animationCtrl.SetTrigger("melee" + Random.Range(1,4));
	}

	public override void ReceiveDamage(float Amount, Vector3 From)
	{
		Vector3 temp = From * -1;
		float f = Vector3.Angle(temp, Graphics.transform.forward);
		float r = Vector3.Angle(temp, Graphics.transform.right);
		float b = Vector3.Angle(temp, Graphics.transform.forward * -1);
		float l = Vector3.Angle(temp, Graphics.transform.right * -1);
		float x = Mathf.Min(f, r, b, l);
		if(x == f)
		{
			AnimationCtrl.SetTrigger("hitFront");
		}
		else if (x == r)
		{
			AnimationCtrl.SetTrigger("hitRight");
		}
		else if (x == b)
		{
			AnimationCtrl.SetTrigger("hitBack");
		}
		else
		{
			AnimationCtrl.SetTrigger("hitLeft");
		}
		base.ReceiveDamage(Amount, From);
	}
	#endregion
}
