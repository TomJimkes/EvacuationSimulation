using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EvacuationSimulation;

public class GameManager : MonoBehaviour
{
    public FloorGrid RealFloorGrid;
    public FireGrid RealFireGrid;
    public List<Firefighter> Firemen = new List<Firefighter>();
    public List<Civilian> Civilians = new List<Civilian>();
    public static GameManager Instance;

	// Use this for initialization
	void Awake ()
	{
	    Instance = this.gameObject.GetComponent<GameManager>();

        //load floor grid
	    this.gameObject.AddComponent<CentralFloor>();

	    //spawn initial fire somewhere


	    //spawn agents somewhere
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerable<Agent> Agents
    {
        get { return Firemen.Select(x => x as Agent).Concat(Civilians.Select(x => x as Agent)); }
        
    } 
}
