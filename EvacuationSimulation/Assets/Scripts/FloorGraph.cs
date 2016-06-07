using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EvacuationSimulation
{
    public class FloorGraph
    {
        Dictionary<int, FloorGraphNode> nodes;
        Dictionary<int, FloorGraphEdge> edges;

        //Constructor
        public FloorGraph()
        {
            nodes = new Dictionary<int, FloorGraphNode>();
            edges = new Dictionary<int, FloorGraphEdge>();
        }

        //Add a node to the graph
        public void AddNode(int id, List<int> incidentEdges)
        {
            nodes.Add(id, new FloorGraphNode(id, incidentEdges));
        }

        //returns node based on ID
        public FloorGraphNode GetFloorGraphNode( int id )
        {
            return nodes[ id ];
        }

        //returns edge based on ID
        public FloorGraphEdge GetFloorGraphEdge(int id)
        {
            return edges[id];
        }

        //Remove a node from the graph
        public void RemoveNode(int id)
        {
            //Remove the incident edges
            foreach(int i in nodes[id].GetIncident)
            {
                edges.Remove(i);
            }

            //Remove node from nodelist
            nodes.Remove(id);

        }
        
        //Add an edge to the list
        public void AddEdge(int id, int origin, int destination)
        {
            edges.Add(id, new FloorGraphEdge(id, origin, destination));
            nodes[origin].GetIncident.Add(id);
            nodes[destination].GetIncident.Add(id);
        }

        //Remove an edge from the list
        public void RemoveEdge(int id)
        {
            //Remove incident pointers
            nodes[edges[id].GetOrigin].GetIncident.Remove(id);
            nodes[edges[id].GetDestination].GetIncident.Remove(id);

            //Remove the edge
            edges.Remove(id);
        }
    }
}
