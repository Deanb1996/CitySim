using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Component_Based_Game_Engine.Objects;
using Component_Based_Game_Engine.Managers;
using Component_Based_Game_Engine.Systems;
using Component_Based_Game_Engine.Components;
using OpenTK;

namespace Transport_Simulation_Game
{
    public static class MapLoader
    {
        public static mEntity entityManager = MainEntry.entityManager;
        public static Node[,] map;
        public static int mapX;
        public static int mapY;

        public static void loadMap(string mapName)
        {
            oEntity newEntity;

            string line;
            int rowCount = 0;

            try
            {
                FileStream fin = File.OpenRead(mapName);
                StreamReader sr = new StreamReader(fin);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    string[] values = line.Split(',');

                    //Gets x value of map dimension
                    if (values[0].StartsWith("X_LENGTH"))
                    {
                        mapX = int.Parse(values[0].Remove(0, "X_LENGTH".Length));
                        continue;
                    }

                    //Gets y value of map dimension
                    if (values[0].StartsWith("Y_LENGTH"))
                    {
                        mapY = int.Parse(values[0].Remove(0, "Y_LENGTH".Length));
                        map = new Node[mapX, mapY];
                        rowCount = mapY - 1;
                        continue;
                    }

                    for (int i = 0; i < values.Count(); i++)
                    {
                        //Creates Node for tile on the map grid and assigns it a location
                        map[i, rowCount] = new Node();
                        map[i, rowCount].Location = new Vector3(i, 0f, rowCount);
                        map[i, rowCount].mapX = i;
                        map[i, rowCount].mapY = rowCount;

                        //Creates building tiles
                        if (values[i].Trim() == "b")
                        {
                            newEntity = new oEntity("Building Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/building3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates grass tiles
                        if (values[i].Trim() == "g")
                        {
                            newEntity = new oEntity("Grass Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/grass.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        #region Pavement Edge Tiles
                        //Creates left side pavement tile
                        if (values[i].Trim() == "lp")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top side pavement tile
                        if (values[i].Trim() == "tp")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates right side pavement tile
                        if (values[i].Trim() == "rp")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom side pavement tile
                        if (values[i].Trim() == "bp")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        //Creates up/down side pavement tile
                        if (values[i].Trim() == "udp")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEntrance.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }
                        //Creates up/down side pavement tile
                        if (values[i].Trim() == "udp2")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEntrance.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }
                        //Creates left/right side pavement tile
                        if (values[i].Trim() == "lrp")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEntrance.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }
                        //Creates left/right side pavement tile
                        if (values[i].Trim() == "lrp2")
                        {
                            newEntity = new oEntity("Pavement Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEntrance.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }
                        #endregion 

                        #region Pavement Corner Tiles
                        //Creates top right pavement corner tile
                        if (values[i].Trim() == "trpc")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom right pavement corner tile
                        if (values[i].Trim() == "brpc")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom left pavement corner tile
                        if (values[i].Trim() == "blpc")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top left pavement corner tile
                        if (values[i].Trim() == "tlpc")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom right outer pavement corner
                        if (values[i].Trim() == "brpc2")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top right outer pavement corner
                        if (values[i].Trim() == "trpc2")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top left outer pavement corner
                        if (values[i].Trim() == "tlpc2")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom left outer pavement corner
                        if (values[i].Trim() == "blpc2")
                        {
                            newEntity = new oEntity("Pavement Corner Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Pavement3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                        #region Single Direction Road Tiles  
                        //Creates road tile with right moving direction
                        if (values[i].Trim() == "r")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Right T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates road tile with left moving direction
                        if (values[i].Trim() == "l")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Left T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates road tile with up moving direction
                        if (values[i].Trim() == "u")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Left T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }

                        //Creates road tile with down moving direction
                        if (values[i].Trim() == "d")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Right T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }
                        #endregion

                        #region Multi Direction Road Tiles
                        //Creates road tile with down right moving direction
                        if (values[i].Trim() == "dr")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates road tile with down left moving direction
                        if (values[i].Trim() == "dl")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates road tile with up right moving direction
                        if (values[i].Trim() == "ur")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates road tile with up left moving direction
                        if (values[i].Trim() == "ul")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates road tile with down left right moving direction
                        if (values[i].Trim() == "dlr")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Left T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates road tile with down up left moving direction
                        if (values[i].Trim() == "dul")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Left T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }

                        //Creates road tile with up left right moving direction
                        if (values[i].Trim() == "ulr")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Right T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates road tile with down up right moving direction
                        if (values[i].Trim() == "dur")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Road Right T.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }
                        #endregion

                        #region Intersection Road Tiles
                        //Creates intersection road tile with right moving direction
                        if (values[i].Trim() == "ir")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates intersection road tile with left moving direction
                        if (values[i].Trim() == "il")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates intersection road tile with up moving direction
                        if (values[i].Trim() == "iu")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }

                        //Creates intersection road tile with down moving direction
                        if (values[i].Trim() == "id")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/IntersectionTextured.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }
                        #endregion

                        #region Road End Up and Down Road Tiles
                        //Creates right side road ending tile with down moving direction (no rotation)
                        if (values[i].Trim() == "rerd")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }

                        //Creates left side road ending tile with up moving direction (no rotation)
                        if (values[i].Trim() == "relu")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }

                        //Creates right side road ending tile with down moving direction (180 degree rotation)
                        if (values[i].Trim() == "rerd2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }

                        //Creates left side road ending tile with up moving direction (180 degree rotation)
                        if (values[i].Trim() == "relu2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }

                        //Creates left side road ending tile with down moving direction (no rotation)
                        if (values[i].Trim() == "reld")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }

                        //Creates right side road ending tile with up moving direction (no rotation)
                        if (values[i].Trim() == "reru")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }

                        //Creates left side road ending tile with down moving direction (180 degree rotation)
                        if (values[i].Trim() == "reld2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }

                        //Creates right side road ending tile with up moving direction (180 degree rotation)
                        if (values[i].Trim() == "reru2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 3.14159f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }
                        #endregion

                        #region Road End Right and Left Road Tiles
                        //Creates right side road ending tile with right moving direction (negative rotation)
                        if (values[i].Trim() == "rerr")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates left side road ending tile with left moving direction (negative rotation)
                        if (values[i].Trim() == "rell")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates right side road ending tile with right moving direction (positive rotation)
                        if (values[i].Trim() == "rerr2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates left side road ending tile with left moving direction (positive rotation)
                        if (values[i].Trim() == "rell2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates left side road ending tile with right moving direction (positive rotation)
                        if (values[i].Trim() == "relr")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates right side road ending tile with left moving direction (positive rotation)
                        if (values[i].Trim() == "rerl")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates left side road ending tile with right moving direction  (negative rotation)
                        if (values[i].Trim() == "relr2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndRightT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates right side road ending tile with left moving direction (negative rotation)
                        if (values[i].Trim() == "rerl2")
                        {
                            newEntity = new oEntity("Road Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -1.5708f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/RoadEndLeftT.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }
                        #endregion

                        #region Farm "Factory" Tiles
                        //Creates top left farm tile
                        if (values[i].Trim() == "tlf")
                        {
                            List<cStock.StockType> stockTypes = new List<cStock.StockType>();
                            stockTypes.Add(cStock.StockType.RAW_FOOD);
                            stockTypes.Add(cStock.StockType.RAW_MEDICAL);

                            newEntity = new oEntity("Farm Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Farm1.png"));
                            newEntity.AddComponent(new cShader());
                            newEntity.AddComponent(new cStock(cStock.StockClass.DELIVERY, stockTypes));
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Left);
                        }

                        //Creates top right farm tile
                        if (values[i].Trim() == "trf")
                        {
                            newEntity = new oEntity("Farm Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Farm2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom right farm tile
                        if (values[i].Trim() == "brf")
                        {
                            newEntity = new oEntity("Farm Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Farm4.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom left farm tile
                        if (values[i].Trim() == "blf")
                        {
                            newEntity = new oEntity("Farm Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Farm3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                        #region Food Factory Tiles
                        //Creates top left food tile
                        if (values[i].Trim() == "tlff")
                        {
                            newEntity = new oEntity("Food Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/FFactoryTL.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top right food tile
                        if (values[i].Trim() == "trff")
                        {
                            List<cStock.StockType> stockTypes = new List<cStock.StockType>();
                            stockTypes.Add(cStock.StockType.RAW_FOOD);

                            newEntity = new oEntity("Food Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/FFactoryTR.png"));
                            newEntity.AddComponent(new cShader());
                            newEntity.AddComponent(new cStock(cStock.StockClass.PRODUCTION, stockTypes));
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates bottom right food tile
                        if (values[i].Trim() == "brff")
                        {
                            newEntity = new oEntity("Food Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/FFactoryBR.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom left food tile
                        if (values[i].Trim() == "blff")
                        {
                            newEntity = new oEntity("Food Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/FFactoryBL.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                        #region Medical Factory Tiles
                        //Creates top left medical tile
                        if (values[i].Trim() == "tlmf")
                        {
                            List<cStock.StockType> stockTypes = new List<cStock.StockType>();
                            stockTypes.Add(cStock.StockType.RAW_MEDICAL);

                            newEntity = new oEntity("Medical Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital 1.png"));
                            newEntity.AddComponent(new cShader());
                            newEntity.AddComponent(new cStock(cStock.StockClass.PRODUCTION, stockTypes));
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Up);
                        }

                        //Creates top right medical tile
                        if (values[i].Trim() == "trmf")
                        {
                            newEntity = new oEntity("Medical Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital 2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom right medical tile
                        if (values[i].Trim() == "brmf")
                        {
                            newEntity = new oEntity("Medical Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital 3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom left medical tile
                        if (values[i].Trim() == "blmf")
                        {
                            newEntity = new oEntity("Medical Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital 4.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                        #region Electronics Factory Tiles
                        //Creates top left electronics tile
                        if (values[i].Trim() == "tlef")
                        {
                            newEntity = new oEntity("Electronics Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/electronics1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top right electronics tile
                        if (values[i].Trim() == "tref")
                        {
                            List<cStock.StockType> stockTypes = new List<cStock.StockType>();
                            stockTypes.Add(cStock.StockType.RAW_OIL);

                            newEntity = new oEntity("Electronics Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/electronics2.png"));
                            newEntity.AddComponent(new cShader());
                            newEntity.AddComponent(new cStock(cStock.StockClass.PRODUCTION, stockTypes));
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Right);
                        }

                        //Creates bottom right electronics tile
                        if (values[i].Trim() == "bref")
                        {
                            newEntity = new oEntity("Electronics Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/electronics4.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom left electronics tile
                        if (values[i].Trim() == "blef")
                        {
                            newEntity = new oEntity("Electronics Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/electronics3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                        #region Oil Factory Tiles
                        //Creates top left oil tile
                        if (values[i].Trim() == "tlof")
                        {
                            newEntity = new oEntity("Oil Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Oil1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top right oil tile
                        if (values[i].Trim() == "trof")
                        {
                            newEntity = new oEntity("Oil Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Oil2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom right oil tile
                        if (values[i].Trim() == "brof")
                        {
                            List<cStock.StockType> stockTypes = new List<cStock.StockType>();
                            stockTypes.Add(cStock.StockType.RAW_OIL);

                            newEntity = new oEntity("Oil Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Oil3.png"));
                            newEntity.AddComponent(new cShader());
                            newEntity.AddComponent(new cStock(cStock.StockClass.DELIVERY, stockTypes));
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }

                        //Creates bottom left oil tile
                        if (values[i].Trim() == "blof")
                        {
                            newEntity = new oEntity("Oil Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, -0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Oil4.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                        #region Wood Factory Tiles
                        //Creates top left Wood tile
                        if (values[i].Trim() == "tlwf")
                        {
                            newEntity = new oEntity("Wood Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Woodfactory 1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top right Wood tile
                        if (values[i].Trim() == "trwf")
                        {
                            newEntity = new oEntity("Wood Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Woodfactory 2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom right Wood tile
                        if (values[i].Trim() == "brwf")
                        {
                            newEntity = new oEntity("Wood Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Woodfactory 3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom left Wood tile
                        if (values[i].Trim() == "blwf")
                        {
                            List<cStock.StockType> stockTypes = new List<cStock.StockType>();
                            stockTypes.Add(cStock.StockType.RAW_WOOD);

                            newEntity = new oEntity("Wood Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Woodfactory 4.png"));
                            newEntity.AddComponent(new cShader());
                            newEntity.AddComponent(new cStock(cStock.StockClass.DELIVERY, stockTypes));
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }
                        #endregion

                        #region Housing Factory Tiles
                        //Creates top left Housing tile
                        if (values[i].Trim() == "tlhf")
                        {
                            newEntity = new oEntity("Housing Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/furniture1.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates top right Housing tile
                        if (values[i].Trim() == "trhf")
                        { 
                            newEntity = new oEntity("Housing Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/furniture2.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Creates bottom right Housing tile
                        if (values[i].Trim() == "brhf")
                        {
                            List<cStock.StockType> stockTypes = new List<cStock.StockType>();
                            stockTypes.Add(cStock.StockType.RAW_WOOD);

                            newEntity = new oEntity("Housing Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/furniture4.png"));
                            newEntity.AddComponent(new cShader());
                            newEntity.AddComponent(new cStock(cStock.StockClass.PRODUCTION, stockTypes));
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = true;
                            map[i, rowCount].Directions.Add(NodeDirection.Down);
                        }

                        //Creates bottom left Housing tile
                        if (values[i].Trim() == "blhf")
                        {
                            newEntity = new oEntity("Housing Factory Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/furniture3.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                        #region Town Hall Tiles
                        //Top left town hall tile
                        if (values[i].Trim() == "tlth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Top middle town hall tile
                        if (values[i].Trim() == "tmth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Top right town hall tile
                        if (values[i].Trim() == "trth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Bottom right town hall tile
                        if (values[i].Trim() == "brth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Bottom middle town hall tile
                        if (values[i].Trim() == "bmth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Bottom left town hall tile
                        if (values[i].Trim() == "blth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Middle left town hall tile
                        if (values[i].Trim() == "mlth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Middle town hall tile
                        if (values[i].Trim() == "mth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }

                        //Middle right town hall tile
                        if (values[i].Trim() == "mrth")
                        {
                            newEntity = new oEntity("Town Hall Tile");
                            newEntity.AddComponent(new cTransform(new Vector3(i, 0f, rowCount), new Vector3(1.5708f, 0.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)));
                            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                            newEntity.AddComponent(new cTexture("Sprite Textures/Hospital.png"));
                            newEntity.AddComponent(new cShader());
                            entityManager.AddEntity(newEntity);
                            map[i, rowCount].Passable = false;
                        }
                        #endregion

                    }
                    rowCount--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
