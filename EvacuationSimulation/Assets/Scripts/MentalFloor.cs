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
                        fGraph.AddNode(destNode, new List<int>(), extGraph.Nodes[destNode].Certainty, extGraph.Nodes[destNode].Live);

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
                fGraph.AddNode(pair.Value.Id, incident, pair.Value.Certainty, pair.Value.Live);
            }

            //Add edges
            foreach (KeyValuePair<int, FloorGraphEdge> pair in extGraph.Edges)
            {
                fGraph.AddEdge(pair.Value.Id, pair.Value.Room, pair.Value.GetOrigin, pair.Value.GetDestination, pair.Value.Certainty, pair.Value.Live);
            }
        }

        //Search in the central floor object to find out what is behind a certain node
        //TODO add stoachastic variable for observing
        public void Observe()
        {
            //TODO
        }
    }
}
