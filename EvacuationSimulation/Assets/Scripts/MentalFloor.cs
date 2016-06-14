using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuationSimulation
{
    public class MentalFloor
    {
        FloorGraph fGraph;
        FloorGrid fGrid;
        //Agents and their last know locations
        //

        public MentalFloor()
        {

        }

        //Getters setters
        public FloorGraph Graph
        {
            get { return fGraph; }
        }

        public FloorGrid Grid
        {
            get { return fGrid; }
        }

        //Data manipulators
        public void ExpandMentalMap(MentalFloor floor)
        {

            //For each node, set certainty to highest of the two (same for edges)
            //Check for a common node, take that node to initiate the dfs algorithm
            int common = findCommonNode(floor.Graph);

            if (common > -1)
            {
                //DFS
                DFSExpand(floor.Graph, common);
            }
            else //If there is no common node
            {
                fullExpand(floor.Graph);
            }

            //Dont copy anything from the grid (this is not relevant for finding a way out)

        }

        //This function finds a common node if there is one, otherwise it will return -1
        private int findCommonNode(FloorGraph extGraph)
        {
            foreach (int key in fGraph.Nodes.Keys)
            {
                if (extGraph.Nodes.ContainsKey(key))
                    return key;
            }
            return -1;
        }

        //Use DFS to expand both mental maps
        private void DFSExpand(FloorGraph extGraph, int commonNode)
        {
            //Start at common node, travel through the tree preferring lower EDGE id's
            //Travel along the external graphs path, add nodes, edges and references to internal graph

            //visited edges
            List<int> visited = new List<int>();

            //Set the highest certainty and liveness
            if (fGraph.Nodes[commonNode].Certainty > extGraph.Nodes[commonNode].Certainty)
            {
                extGraph.Nodes[commonNode].Certainty = fGraph.Nodes[commonNode].Certainty;
                extGraph.Nodes[commonNode].Live = fGraph.Nodes[commonNode].Live;
            }
            else
            {
                fGraph.Nodes[commonNode].Certainty = extGraph.Nodes[commonNode].Certainty;
                fGraph.Nodes[commonNode].Live = extGraph.Nodes[commonNode].Live;
            }

            foreach (int e in extGraph.Nodes[commonNode].GetIncident)
            {
                //If edge has not been visited yet
                if (!(visited.Contains(e)))
                {
                    //Set edge to visited
                    visited.Add(e);

                    //If iNodes does not contain a refrence to the edge
                    if (!(fGraph.Nodes[commonNode].GetIncident.Contains(e)))
                    {
                        int destNode = extGraph.Edges[e].GetDestination;

                        //Add reference in commonnode
                        fGraph.Nodes[commonNode].GetIncident.Add(e);

                        //Copy edge to iEdges
                        extGraph.AddEdge(extGraph.Edges[e].Id, extGraph.Edges[e].Room ,extGraph.Edges[e].GetOrigin, extGraph.Edges[e].GetDestination, extGraph.Edges[e].Certainty, extGraph.Edges[e].Live);

                        //Add destination node to iNodes
                        fGraph.AddNode(destNode, new List<int>(), extGraph.Nodes[destNode].Exit, extGraph.Nodes[destNode].Certainty, extGraph.Nodes[destNode].Live);

                        //Recurse on destination
                        DFSExpand(extGraph, destNode);
                    }
                    else
                    {
                        //set certainty and liveness
                        if (fGraph.Edges[e].Certainty > extGraph.Edges[e].Certainty)
                        {
                            extGraph.Edges[e].Certainty = fGraph.Edges[e].Certainty;
                            extGraph.Edges[e].Live = fGraph.Edges[e].Live;
                        }
                        else
                        {
                            fGraph.Edges[e].Certainty = extGraph.Edges[e].Certainty;
                            fGraph.Edges[e].Live = extGraph.Edges[e].Live;
                        }

                        //Recurse on destination
                        DFSExpand(extGraph, fGraph.Edges[e].GetDestination);
                    }
                }
            }
        }

        //Adds all nodes from the external graph
        private void fullExpand(FloorGraph extGraph)
        {
            //Add nodes
            foreach (KeyValuePair<int, FloorGraphNode> pair in extGraph.Nodes)
            {
                List<int> incident = new List<int>();
                foreach (int i in pair.Value.GetIncident)
                {
                    incident.Add(i);
                }
                fGraph.AddNode(pair.Value.Id, incident, pair.Value.Exit, pair.Value.Certainty, pair.Value.Live);
            }

            //Add edges
            foreach (KeyValuePair<int, FloorGraphEdge> pair in extGraph.Edges)
            {
                fGraph.AddEdge(pair.Value.Id, pair.Value.Room, pair.Value.GetOrigin, pair.Value.GetDestination, pair.Value.Certainty, pair.Value.Live);
            }
        }

        //Search in the central floor object to find out what is behind a certain node
        //TODO add stoachastic variable for observing --> approximated edge length and 
        public void ObserveRoom(int nodeID, CentralFloor floor)
        {
            List<int> incident = floor.Graph.Node(nodeID).GetIncident;
            foreach(int i in incident)
            {
                FloorGraphEdge e = floor.Graph.Edge(i);
                //Add edge
                if(!(fGraph.Edges.ContainsKey(i)))
                {
                    fGraph.AddEdge(e.Id, e.HardCopy());
                    fGraph.Node(nodeID).GetIncident.Add(i);
                }

                //Add node
                int dest = fGraph.Edge(i).GetDestination;
                if(!fGraph.Nodes.ContainsKey(dest))
                {
                    fGraph.AddNode(dest, new List<int> { i });
                }

                //Add references
                if (!fGraph.Node(dest).GetIncident.Contains(i))
                    fGraph.Node(dest).GetIncident.Add(i);
            }


        }

        //TODO observe grid
        public void ObserveGrid(int x, int y, CentralFloor floor /* extra params for fear */)
        {
            int r = 0; //ViewRadius
            FloorGrid eGrid = floor.Grid;
            //Potentially simplify --> Observe a square


            //Get the grid from the centralfloor
            //Observe everything in a certain radius, while not going through walls
                //flood fill to a certain eucledian distance? --> allows the agent to look around corners
                //Square of width and height 2r, collision check for each squah (expensive??)
                //Create a spiral that either observes/does not observe if it hits a wall
            //Smoke decreases this radius
            //Panic decreases this radius

            //TODO: colision detection (walls) --> talk about this with timon
            //If a door is found --> perform collision detection for walls AND objects 
        }

    }
}
