using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EvacuationSimulation;

namespace Assets.Scripts
{
    class StraightLinePath
    {
        private bool[,] grid;
        private List<Object> objects;

        public StraightLinePath( bool[ , ] Grid, List<Object> Objects)
        {
            grid = Grid;
            objects = Objects;
        }

        public List<Line> CreatePath(List<PathFinderNode> Nodes, int squareSize)
        {
            List<Line> result = new List<Line>();

            PathFinderNode current = Nodes.First();
            PathFinderNode LastOption;
            PathFinderNode goal = Nodes.Last( );
            Nodes.Remove( current );
            //Nodes.Remove( goal );

            while ( Nodes.Count != 0 )
            {
                if ( false ) //!lineCurrentToFirstIntersectionWithObjects(Current, Nodes.First, objects))
                {
                    //LastOption is Nodes.First;
                    //Nodes.Remove(Nodes.First);
                }
                else
                {
                    //result.Add(new Line(Current, LastOption));
                    //current = LastOption;
                }

                //result.Add(new Line(Current, LastOption));
            }
            //make line from current to goal

            return result;
        }

        private bool LineSegmentIntersection( Line A, Line B )
        {
            //check whether 2 lines intersect
            return false;
        }
    }

    struct Line
    {
        public Point A, B;

        public Line( Point a, Point b )
        {
            A = a;
            B = b;
        } 
    }
}
