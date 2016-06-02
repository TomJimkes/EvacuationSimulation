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

	// Use this for initialization
	void Start () {
	    
        //load floor grid

        //spawn initial fire somewhere
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerable<Agent> Agents
    {
        get { return Firemen.Select(x => x as Agent).Concat(Civilians.Select(x => x as Agent)); }
        
    } 
}
