using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component_Based_Game_Engine.Objects;
using Component_Based_Game_Engine.Components;
using OpenTK;

namespace Component_Based_Game_Engine.Systems
{
    public class sAI : ISystem
    {
        const ComponentMasks MASK = (ComponentMasks.COMPONENT_AI);

        private List<oEntity> entityList;
        private Node[,] map;
        private int rows;
        private int columns;
        private Random rnd = new Random();

        public sAI(Node[,] mapIn, int rowsIn, int columnsIn)
        {
            entityList = new List<oEntity>();
            map = mapIn;
            rows = rowsIn;
            columns = columnsIn;
        }

        public string Name
        {
            get { return "SystemAI"; }
        }

        public void AssignEntity(oEntity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                entityList.Add(entity);
            }
        }

        public void DestroyEntity(oEntity entity)
        {
            entityList.Remove(entity);
        }

        public void OnAction()
        {
            foreach(oEntity entity in entityList)
            {
                //Retrieves list of components for current entity
                List<IComponent> components = entity.Components;

                //Retrieves ai component for current entity
                IComponent aiComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_AI;
                });

                //Checks if ai has a target set and sets it a random target if it doesn't have one 
                //OR finds a path to the target if already set
                if(!((cAI)aiComponent).TargetSet)
                {
                    SetTargetNode(((cAI)aiComponent));
                    SetRandomPath(((cAI)aiComponent));
                }
                else if(!((cAI)aiComponent).PathSet)
                {
                    SetRandomPath(((cAI)aiComponent));
                }

                //Checks if next node is set to the next node in path, and if not sets next node to the next node in the path
                if(!((cAI)aiComponent).NextSet)
                {
                    SetNextNode(((cAI)aiComponent));
                }

                //Checks if the entity has reached its target, assigns a new target and calulates a path to the new target 
                //OR checks if entity has reached its next node and sets its next node to the next node in the path
                if(((cAI)aiComponent).CurrentNode == ((cAI)aiComponent).TargetNode)
                {
                    if(((cAI)aiComponent).RandomlyPath == true)
                    {
                        SetTargetNode(((cAI)aiComponent));
                        SetRandomPath(((cAI)aiComponent));
                    }
                    else
                    {
                        ((cAI)aiComponent).DestinationReached = true;
                    }
                }
                else if(((cAI)aiComponent).CurrentNode == ((cAI)aiComponent).NextNode)
                {
                    SetNextNode(((cAI)aiComponent));
                    ((cAI)aiComponent).PathNodesTraversed++;
                }
            }
        }

        private void SetTargetNode(cAI ai)
        {
            bool targetSet = false;
            int randomX = 0;
            int randomY = 0;

            //Randomly chooses a target node that is passable on the map grid
            while (!targetSet)
            {
                randomX = rnd.Next(1, rows);
                randomY = rnd.Next(1, columns);

                //Ensures that the chosen target node is passable, then sets new target node
                if (map[randomX, randomY].Passable == true)
                {
                    ai.TargetNode = new Vector2(randomX, randomY);
                    ai.TargetNodeLocation = map[randomX, randomY].Location;
                    targetSet = true;
                    ai.TargetSet = true;
                    ai.PathNodesTraversed = 0;
                }
            }
        }

        private void SetNextNode(cAI ai)
        {
            //Updates current node location then finds next node in the path and sets it as the next node
            ai.CurrentNodeLocation = ai.NextNodeLocation;

            //Ensures path has at least more than one value in it or finds another target and path
            if (ai.Path.Count() > 1)
            {
                ai.NextNode = ai.Path[ai.PathNodesTraversed];
                ai.NextNodeLocation = map[(int)ai.Path[ai.PathNodesTraversed].X, (int)ai.Path[ai.PathNodesTraversed].Y].Location;
                ai.NextSet = true;
            }
            else
            {
                SetTargetNode(ai);
                SetRandomPath(ai);
            }
        }

        private void SetRandomPath(cAI ai)
        {
            Node currentNode = map[(int)ai.CurrentNode.X, (int)ai.CurrentNode.Y];
            ai.Path.Clear();

            //Calculates total direct distance between nodes and target node
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (!(i == ai.CurrentNode.X && j == ai.CurrentNode.Y))
                    {
                        map[i, j].H = new Vector3(map[i, j].Location - ai.TargetNodeLocation).Length;
                    }
                    map[i, j].ParentNode = null;
                    map[i, j].State = NodeState.Untested;
                }
            }

            //Returns true if a valid path to the target node can be found
            bool pathFound = SearchForPath(ai, currentNode);

            //If path is found, back tracks through the nodes in the path using the parent nodes to retrieve the complete path and then stores in a list from starting node to target node
            if (pathFound)
            {
                Node node = map[(int)ai.TargetNode.X, (int)ai.TargetNode.Y];
                while (node.ParentNode != null)
                {
                    ai.Path.Add(new Vector2(node.mapX, node.mapY));
                    node = node.ParentNode;
                }
                ai.Path.Reverse();
                ai.PathSet = true;
            }

            //Prints out path in console for debugging purposes
            foreach (Vector2 node in ai.Path)
            {
                //Console.WriteLine(node.X + ", " + node.Y);
            }
        }

        private bool SearchForPath(cAI ai, Node currentNode)
        {
            //Gets list of passable neighbours and then sorts them by the estimated distance traveled if taking this node on the path (F) and sets current node to closed
            currentNode.State = NodeState.Closed;
            List<Node> passableNeighbours = GetPassableNeighbours(ai, currentNode);
            passableNeighbours.Sort((node1, node2) => node1.F.CompareTo(node2.F));

            //Recursively searches through each neighbour until the shortest path is found if a valid path is available
            foreach (Node passableNeighbour in passableNeighbours)
            {
                if (passableNeighbour.Location == ai.TargetNodeLocation)
                {
                    return true;
                }
                else
                {
                    if (SearchForPath(ai, passableNeighbour))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<Node> GetPassableNeighbours(cAI ai, Node currentNode)
        {
            List<Node> passableNeighbours = new List<Node>();

            //Gets all neighbouring nodes of current node
            List<Node> neighbours = GetNeighbours(ai, currentNode);

            //Gets all walkable nodes in the neighbouring nodes that are more efficient nodes to use in the path than the currently chosen node
            foreach (Node neighbour in neighbours)
            {
                //Ignores closed nodes
                if (neighbour.State == NodeState.Closed)
                {
                    continue;
                }

                //Ignores unpassable nodes
                if (!neighbour.Passable)
                {
                    continue;
                }

                //If node has already been tested, tests to see if traversing the neighbouring node will be more efficient from this node than its parents node, then adds it to passable neighbours if it is
                if (neighbour.State == NodeState.Open)
                {
                    float gDistance = new Vector3(neighbour.Location - neighbour.ParentNode.Location).Length;
                    float tempGDistance = currentNode.G + gDistance;
                    if (tempGDistance < neighbour.G)
                    {
                        neighbour.ParentNode = currentNode;
                        passableNeighbours.Add(neighbour);
                    }
                }
                //If node is currently untested, adds it to the passable neighbours list and sets it to Open State
                else
                {
                    neighbour.ParentNode = currentNode;
                    neighbour.State = NodeState.Open;
                    passableNeighbours.Add(neighbour);
                }
            }
            return passableNeighbours;
        }

        private List<Node> GetNeighbours(cAI ai, Node currentNode)
        {
            List<Node> neighbours = new List<Node>();

            //Gets neighbour above current node
            if(currentNode.Directions.Contains(NodeDirection.Up))
            {
                neighbours.Add(map[(int)currentNode.mapX, (int)currentNode.mapY + 1]);
            }
            //Gets neighbour below current node
            if (currentNode.Directions.Contains(NodeDirection.Down))
            {
                neighbours.Add(map[(int)currentNode.mapX, (int)currentNode.mapY - 1]);
            }
            //Gets neighbour right of current node
            if (currentNode.Directions.Contains(NodeDirection.Right))
            {
                neighbours.Add(map[(int)currentNode.mapX + 1, (int)currentNode.mapY]);
            }
            //Gets neighbour left of current node
            if (currentNode.Directions.Contains(NodeDirection.Left))
            {
                neighbours.Add(map[(int)currentNode.mapX - 1, (int)currentNode.mapY]);
            }

            //Diagonal movement
            //Top Right
            //neighbours.Add(map[(int)currentNode.mapX + 1, (int)currentNode.mapY + 1]);
            //Bottom Right
            //neighbours.Add(map[(int)currentNode.mapX + 1, (int)currentNode.mapY - 1]);
            //Top Left
            //neighbours.Add(map[(int)currentNode.mapX - 1, (int)currentNode.mapY + 1]);
            //Bottom Left
            //neighbours.Add(map[(int)currentNode.mapX - 1, (int)currentNode.mapY - 1]);

            return neighbours;
        }
    }

    public class Node
    {
        private Node parentNode;
        private List<NodeDirection> directions = new List<NodeDirection>();

        public bool Passable { get; set; }
        public Vector3 Location { get; set; }
        public Node ParentNode
        {
            get { return parentNode; }
            set
            {
                parentNode = value;
                if (parentNode != null)
                {
                    G = parentNode.G + new Vector3(Location - parentNode.Location).Length; //Calculates new G value based on the new parent nodes G value
                }
            }
        }
        public NodeState State { get; set; } //Untested means not yet tested, Open means tested and still open for consideration for the path, 
        //Closed means tested and eliminated from consideration or already added to a path
        public float G { get; set; } //Total length of path from start node to this node
        public float H { get; set; } //Distance from node to target node
        public float F { get { return G + H; } } //Estimated total distance traveled if taking this node in the path
        public float mapX { get; set; } //X Location on map grid
        public float mapY { get; set; } //Y Location on map grid
        public List<NodeDirection> Directions { get { return directions; } } //Directions in which the ai can move from the node
    }

    public enum NodeState
    {
        Untested,
        Open,
        Closed
    }

    public enum NodeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
