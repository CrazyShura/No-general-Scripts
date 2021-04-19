using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : MonoBehaviour, IEventInterface
{
	#region Fields
	[SerializeField]
	int x;
	[SerializeField]
	int y;
	[SerializeField]
	float initiative = 100;
	[SerializeField]
	int size = 1;
	[SerializeField]
	float health = 100;

	float turnTimer = 0;
	GameObject graphics;
	List<Cell> occupation = new List<Cell>();
	bool myTurn = false;
	protected List<Skill> skills = new List<Skill>();
	NavMeshAgent agent;
	bool initialized = false;
	UnitEvent unitIsReady = new UnitEvent(); //Uses events manager
	UnityEvent turnIsOver = new UnityEvent(); //Uses IEventInterface

	float timeDelta = 0;
	bool busy = false;
	#endregion

	#region Properies
	public Cell BoardPosition
	{
		get
		{
			if (Occupation.Count != 0)
			{
				return Occupation[0];
			}
			else
			{
				Debug.LogError("Unit " + this.name + " has no set board position");
				return null;
			}
		}
	}

	public bool MyTurn
	{
		get { return myTurn; }
		set { myTurn = value; }
	}

	public float TurnTimer
	{
		get { return turnTimer; }
		set { turnTimer = value; }
	}

	public int Size
	{
		get { return size; }
	}

	public List<Cell> Occupation { get => occupation; }

	public NavMeshAgent Agent
	{
		get { return agent; }
	}

	public GameObject Graphics
	{
		get { return graphics; }
	}

	public bool Initialized { get => initialized; }
	public bool Busy { get => busy; set => busy = value; }
	#endregion

	#region Methods
	protected virtual void Awake()
	{
		if (!Initialized)
		{
			Initialise();
		}
	}

	public virtual void Initialise()
	{
		graphics = this.transform.GetChild(0).gameObject;
		agent = this.gameObject.GetComponent<NavMeshAgent>();
		initialized = true;
	}

	private void Start()
	{
		Cell temp = PublicStuff.GameMaster.Board.GetCell(x, y);
		this.transform.position = temp.transform.position + new Vector3((size - 1) * .5f, 0, (size - 1) * .5f);
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				temp = PublicStuff.GameMaster.Board.GetCell(x + i, j + y);
				Occupation.Add(temp);
				temp.Occupant = this;
			}
		}
		EventsManager.TimePassed.AddListener(TimePassed);
		EventsManager.UnitIsReady.AddInvoker(unitIsReady);
		PublicStuff.GameMaster.AddUnit(this);
	}

	protected virtual void Update()
	{
		if (myTurn && Input.GetKey(KeyCode.Alpha0))
		{
			timeDelta += Time.deltaTime;
		}
		else
		{
			timeDelta = 0;
		}
		if (timeDelta > .5f)
		{
			timeDelta = 0;
			turnIsOver.Invoke();
		}
	}

	public virtual void ReceiveDamage(float Amount, Vector3 From)
	{
		health -= Amount;
		Debug.Log(this.name + " received " + Amount + " points of damage.");
		if (health <= 0)
		{
			Die();
		}
	}

	protected virtual void Die()
	{
		foreach (Cell cell in occupation)
		{
			cell.Occupant = null;
		}
		PublicStuff.GameMaster.Board.ResetCellColor(Occupation);
		Destroy(this.gameObject);
	}

	void TimePassed()
	{
		turnTimer += initiative;
		if (turnTimer > 1000)
		{
			turnTimer -= 1000;
			unitIsReady.Invoke(this);
		}
	}

	public void ResetSkillsCells()
	{
		foreach (Skill skill in skills)
		{
			skill.ClearCells();
		}
	}
	#region IEventInterface implementation
	public void AddListener(UnityAction action)
	{
		turnIsOver.AddListener(action);
	}

	public void RemoveListener(UnityAction action)
	{
		turnIsOver.RemoveListener(action);
	}

	public void RemoveAllListeners()
	{
		turnIsOver.RemoveAllListeners();
	}
	#endregion

	#endregion
}
