using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Component_Based_Game_Engine.Components
{
    public class cAI : IComponent
    {
        private List<Vector2> path;
        private Vector2 currentNode;
        private Vector3 currentNodeLocation;
        private Vector2 targetNode;
        private Vector3 targetNodeLocation;
        private Vector2 nextNode;
        private Vector3 nextNodeLocation;
        private int pathNodesTraversed;
        private bool targetSet;
        private bool nextSet;
        private bool pathSet;
        private bool randomlyPath;
        private bool destinationReached;

        public cAI(Vector2 currentNodeIn, Vector3 currentNodeLocationIn)
        {
            currentNode = currentNodeIn;
            currentNodeLocation = currentNodeLocationIn;
            path = new List<Vector2>();
            pathNodesTraversed = 0;
            targetSet = false;
            nextSet = false;
            randomlyPath = true;
            destinationReached = false;
        }

        public cAI(Vector2 currentNodeIn, Vector3 currentNodeLocationIn, Vector2 targetNodeIn, Vector3 targetNodeLocationIn)
        {
            currentNode = currentNodeIn;
            currentNodeLocation = currentNodeLocationIn;
            targetNode = targetNodeIn;
            targetNodeLocation = targetNodeLocationIn;
            path = new List<Vector2>();
            pathNodesTraversed = 0;
            targetSet = true;
            nextSet = false;
            pathSet = false;
            randomlyPath = false;
            destinationReached = false;
        }

        public ComponentMasks ComponentMask
        {
            get { return ComponentMasks.COMPONENT_AI; }
        }

        public List<Vector2> Path
        {
            get { return path; }
            set { path = value; }
        }

        public Vector2 CurrentNode
        {
            get { return currentNode; }
            set { currentNode = value; }
        }

        public Vector3 CurrentNodeLocation
        {
            get { return currentNodeLocation; }
            set { currentNodeLocation = value; }
        }

        public Vector2 TargetNode
        {
            get { return targetNode; }
            set { targetNode = value; }
        }

        public Vector3 TargetNodeLocation
        {
            get { return targetNodeLocation; }
            set { targetNodeLocation = value; }
        }

        public Vector2 NextNode
        {
            get { return nextNode; }
            set { nextNode = value; }
        }

        public Vector3 NextNodeLocation
        {
            get { return nextNodeLocation; }
            set { nextNodeLocation = value; }
        }

        public int PathNodesTraversed
        {
            get { return pathNodesTraversed; }
            set { pathNodesTraversed = value; }
        }

        public bool TargetSet
        {
            get { return targetSet; }
            set { targetSet = value; }
        }

        public bool NextSet
        {
            get { return nextSet; }
            set { nextSet = value; }
        }

        public bool PathSet
        {
            get { return pathSet; }
            set { pathSet = value; }
        }

        public bool RandomlyPath
        {
            get { return randomlyPath; }
            set { randomlyPath = value; }
        }

        public bool DestinationReached
        {
            get { return destinationReached; }
            set { destinationReached = value; }
        }
    }
}