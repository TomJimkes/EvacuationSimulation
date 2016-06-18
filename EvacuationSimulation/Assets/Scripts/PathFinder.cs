//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
//  REMAINS UNCHANGED.
//
//  Email:  gustavo_franco@hotmail.com
//
//  Copyright (C) 2006 Franco, Gustavo 
//
#define DEBUGON

using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EvacuationSimulation
{
    #region Structs
    [Author("Franco, Gustavo")]
    public struct PathFinderNode
    {
        #region Variables Declaration
        public float   F;
        public float   G;
        public float   H;  // f = gone + heuristic
        public int     X;
        public int     Y;
        public int     PX; // Parent
        public int     PY;

        #endregion
    }

    public struct Point
    {
        public Point( int x, int y )
        {
            X = x;
            Y = y;
        }

        public int X, Y;
    }
    #endregion

    #region Enum
    [Author("Franco, Gustavo")]
    public enum PathFinderNodeType
    {
        Start   = 1,
        End     = 2,
        Open    = 4,
        Close   = 8,
        Current = 16,
        Path    = 32
    }

    public enum HeuristicFormula
    {
        Manhattan           = 1,
        MaxDXDY             = 2,
        DiagonalShortCut    = 3,
        Euclidean           = 4,
        EuclideanNoSQR      = 5,
        Custom1             = 6
    }
    #endregion

    #region Delegates
    public delegate void PathFinderDebugHandler(int fromX, int fromY, int x, int y, PathFinderNodeType type, int totalCost, int cost);
    #endregion

    [Author("Franco, Gustavo")]
    public class PathFinder : IPathFinder
    {
        //[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint="RtlZeroMemory")]
        //public unsafe static extern bool ZeroMemory(byte* destination, int length);

        #region Events
        public event PathFinderDebugHandler PathFinderDebug;
        #endregion

        #region Variables Declaration
        public FloorGraph 
            mGraph                  = null;
        public bool[,]                          mGrid                   = null;
        private PriorityQueueB<PathFinderNode>  mOpen                   = new PriorityQueueB<PathFinderNode>(new ComparePFNode());
        private List<PathFinderNode>            mClose                  = new List<PathFinderNode>();
        private PriorityQueueB<FloorGraphNode>  mGraphOpen              = new PriorityQueueB<FloorGraphNode>(new CompareGraphPFNode());
        private List<FloorGraphNode>            mGraphClose             = new List<FloorGraphNode>();
        private bool                            mStop                   = false;
        private bool                            mStopped                = true;
        private int                             mHoriz                  = 0;
        private HeuristicFormula                mFormula                = HeuristicFormula.Manhattan;
        private bool                            mDiagonals              = true;
        private int                             mHEstimate              = 2;
        private bool                            mPunishChangeDirection  = false;
        private bool                            mTieBreaker             = false;
        private bool                            mHeavyDiagonals         = false;
        private int                             mSearchLimit            = 8000;
        private double                          mCompletedTime          = 0;
        private bool                            mDebugProgress          = false;
        private bool                            mDebugFoundPath         = false;
        #endregion

        #region Constructors
        public PathFinder(bool[,] grid)
        {
            if (grid == null)
                throw new Exception("Grid cannot be null");

            mGrid = grid;
        }

        public PathFinder(FloorGraph graph)
        {
            if (graph == null)
                throw new Exception("Graph cannot be null");

            mGraph = graph;
        }
        #endregion

        #region Properties
        public bool Stopped
        {
            get { return mStopped; }
        }

        public HeuristicFormula Formula
        {
            get { return mFormula; }
            set { mFormula = value; }
        }

        public bool Diagonals
        {
            get { return mDiagonals; }
            set { mDiagonals = value; }
        }

        public bool HeavyDiagonals
        {
            get { return mHeavyDiagonals; }
            set { mHeavyDiagonals = value; }
        }

        public int HeuristicEstimate
        {
            get { return mHEstimate; }
            set { mHEstimate = value; }
        }

        public bool PunishChangeDirection
        {
            get { return mPunishChangeDirection; }
            set { mPunishChangeDirection = value; }
        }

        public bool TieBreaker
        {
            get { return mTieBreaker; }
            set { mTieBreaker = value; }
        }

        public int SearchLimit
        {
            get { return mSearchLimit; }
            set { mSearchLimit = value; }
        }

        public double CompletedTime
        {
            get { return mCompletedTime; }
            set { mCompletedTime = value; }
        }

        public bool DebugProgress
        {
            get { return mDebugProgress; }
            set { mDebugProgress = value; }
        }

        public bool DebugFoundPath
        {
            get { return mDebugFoundPath; }
            set { mDebugFoundPath = value; }
        }
        #endregion

        #region Methods
        public void FindPathStop()
        {
            mStop = true;
        }

        public List<FloorGraphNode> FindPathGraph(FloorGraphNode start, FloorGraphNode end)
        {
            FloorGraphNode parentNode = start;
            bool found = false;
            mStop = false;
            mStopped = false;
            mOpen.Clear();
            mClose.Clear();

            parentNode.G = 0;
            parentNode.H = mHEstimate;
            parentNode.F = parentNode.G + parentNode.H;
            parentNode.parent = parentNode.id;

            mGraphOpen.Push(parentNode);
            while ( mGraphOpen.Count > 0 && !mStop )
            {
                parentNode = mGraphOpen.Pop();

                if (parentNode == end)
                {
                    mGraphClose.Add(parentNode);
                    found = true;
                    break;
                }

                if (mClose.Count > mSearchLimit)
                {

                    mStopped = true;
                    return null;
                }

                for (int i = 0; i < parentNode.GetIncident.Count; i++)
                {
                    FloorGraphEdge newEdge = mGraph.GetFloorGraphEdge(parentNode.GetIncident[i]);
                    FloorGraphNode newNode = mGraph.GetFloorGraphNode(newEdge.OtherSide(parentNode.id));

                    float newG = parentNode.G + newEdge.expectedLength;

                    if (newG == parentNode.G)
                    {
                        //Unbrekeable
                        continue;
                    }

                    int foundInOpenIndex = -1;
                    for (int j = 0; j < mGraphOpen.Count; j++)
                    {
                        if (mGraphOpen[j] == newNode)
                        {
                            foundInOpenIndex = j;
                            break;
                        }
                    }
                    if (foundInOpenIndex != -1 && mGraphOpen[foundInOpenIndex].G <= newG)
                        continue;

                    int foundInCloseIndex = -1;
                    for (int j = 0; j < mGraphClose.Count; j++)
                    {
                        if (mGraphClose[j] == newNode )
                        {
                            foundInCloseIndex = j;
                            break;
                        }
                    }
                    if (foundInCloseIndex != -1 && mGraphClose[foundInCloseIndex].G <= newG)
                        continue;

                    newNode.parent = parentNode.id;
                    newNode.G = newG;

                    newNode.H = mHEstimate * (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                    newNode.F = newNode.G + newNode.H;

                    mGraphOpen.Push(newNode);
                }

                mGraphClose.Add(parentNode);
            }

            mCompletedTime = HighResolutionTime.GetTime();
            if (found)
            {
                FloorGraphNode fNode = mGraphClose[mGraphClose.Count - 1];
                for (int i = mGraphClose.Count - 1; i >= 0; i--)
                {
                    if (fNode.parent == mGraphClose[i].id || i == mGraphClose.Count - 1)
                    {
                        fNode = mGraphClose[i];
                    }
                    else
                        mGraphClose.RemoveAt(i);
                }
                mStopped = true;
                return mGraphClose;
            }
            mStopped = true;
            return null;

        }

        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            HighResolutionTime.Start();

            PathFinderNode parentNode;
            bool found  = false;
            int  gridX  = mGrid.GetUpperBound(0);
            int  gridY  = mGrid.GetUpperBound(1);

            mStop       = false;
            mStopped    = false;
            mOpen.Clear();
            mClose.Clear();

            bool[,] discovered = new bool[gridX+1, gridY+1];

            #if DEBUGON
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, start.X, start.Y, PathFinderNodeType.Start, -1, -1);
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, end.X, end.Y, PathFinderNodeType.End, -1, -1);
            #endif

            sbyte[,] direction;
            //if (mDiagonals)
                //direction = new sbyte[8,2]{ {0,-1} , {1,0}, {0,1}, {-1,0}, {1,-1}, {1,1}, {-1,1}, {-1,-1}};
            //else
                direction = new sbyte[4,2]{ {0,-1} , {1,0}, {0,1}, {-1,0}};

            parentNode.G         = 0;
            parentNode.H         = mHEstimate;
            parentNode.F         = parentNode.G + parentNode.H;
            parentNode.X         = start.X;
            parentNode.Y         = start.Y;
            parentNode.PX        = parentNode.X;
            parentNode.PY        = parentNode.Y;


            mOpen.Push(parentNode);
            while(mOpen.Count > 0 && !mStop)
            {
                parentNode = mOpen.Pop();

                #if DEBUGON
                if (mDebugProgress && PathFinderDebug != null)
                    PathFinderDebug(0, 0, parentNode.X, parentNode.Y, PathFinderNodeType.Current, -1, -1);
                #endif

                if (parentNode.X == end.X && parentNode.Y == end.Y)
                {
                    mClose.Add(parentNode);
                    found = true;
                    break;
                }

                if (mClose.Count > mSearchLimit)
                {
                    mStopped = true;
                    return null;
                }

                //Lets calculate all successors
                //for (int i=0; i<(mDiagonals ? 8 : 4); i++)
                for (int i = 0; i < 4; i++)
                {
                    PathFinderNode newNode;
                    newNode.X = parentNode.X + direction[i,0];
                    newNode.Y = parentNode.Y + direction[i,1];

                    if ( newNode.X < 0 || newNode.Y < 0 || newNode.X > gridX || newNode.Y > gridY || !mGrid[newNode.X, newNode.Y])
                       continue;
                    if (discovered[newNode.X, newNode.Y])
                        continue;

                    float newG;
                    if (i>3)
                        newG = parentNode.G + 1.41f;
                    else
                        newG = parentNode.G + 1;


                    if (newG == parentNode.G)
                    {
                        //Unbrekeable
                        continue;
                    }

                    if (mPunishChangeDirection)
                    {
                        if ((newNode.X - parentNode.X) != 0)
                        {
                            if (mHoriz == 0)
                                newG += 20;
                        }
                        if ((newNode.Y - parentNode.Y) != 0)
                        {
                            if (mHoriz != 0)
                                newG += 20;

                        }
                    }
                   
                    newNode.PX      = parentNode.X;
                    newNode.PY      = parentNode.Y;
                    newNode.G       = newG;
                    newNode.H       = mHEstimate * (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));

                    if (mTieBreaker)
                    {
                        int dx1 = parentNode.X - end.X;
                        int dy1 = parentNode.Y - end.Y;
                        int dx2 = start.X - end.X;
                        int dy2 = start.Y - end.Y;
                        int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                        newNode.H = (int) (newNode.H + cross * 0.001);
                    }
                    newNode.F       = newNode.G + newNode.H;

                    discovered[ newNode.X, newNode.Y ] = true;
                    mOpen.Push(newNode);
                }

                mClose.Add(parentNode);
            }

            mCompletedTime = HighResolutionTime.GetTime();
            if (found)
            {
                PathFinderNode fNode = mClose[mClose.Count - 1];
                for(int i=mClose.Count - 1; i>=0; i--)
                {
                    if (fNode.PX == mClose[i].X && fNode.PY == mClose[i].Y || i == mClose.Count - 1)
                    {
                        /*
                        #if DEBUGON
                        if (mDebugFoundPath && PathFinderDebug != null)
                            PathFinderDebug(fNode.X, fNode.Y, mClose[i].X, mClose[i].Y, PathFinderNodeType.Path, mClose[i].F, mClose[i].G);
                        #endif
                        */
                        fNode = mClose[i];
                    }
                    else
                        mClose.RemoveAt(i);
                }
                mStopped = true;
                return mClose;
            }
            mStopped = true;
            return null;
        }
        #endregion

        #region Inner Classes
        [Author("Franco, Gustavo")]
        internal class ComparePFNode : IComparer<PathFinderNode>
        {
            #region IComparer Members
            public int Compare(PathFinderNode x, PathFinderNode y)
            {
                if (x.F > y.F)
                    return 1;
                else if (x.F < y.F)
                    return -1;
                return 0;
            }
            #endregion
        }

        internal class CompareGraphPFNode : IComparer<FloorGraphNode>
        {
            #region IComparer Members
            public int Compare(FloorGraphNode x, FloorGraphNode y)
            {
                if (x.F > y.F)
                    return 1;
                else if (x.F < y.F)
                    return -1;
                return 0;
            }
            #endregion
        }
        #endregion
    }
}
