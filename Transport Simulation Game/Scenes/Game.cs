using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Component_Based_Game_Engine.Components;
using Component_Based_Game_Engine.Systems;
using Component_Based_Game_Engine.Objects;
using Component_Based_Game_Engine.Managers;
using System.Drawing;
using OpenTK.Graphics;
using System.IO;
using Transport_Simulation_Game;

namespace Transport_Simulation_Game
{
    class Game : oScene, IScene
    {
        List<string> keyboardInput;
        List<oEntity> entities;
        List<oEntity> factories;
        List<oEntity> trucks;
        oCamera camera;
        oEntity selectedFactory;
        float food, medical, housing, electronics;
        float population, money, populationTemp;
        float pauseToggleTimer, upgradeToggleTimer;
        float tickRateTimer;
        bool paused;

        public Game(mScene sceneManager) : base(sceneManager)
        {
            //Set window title
            sceneManager.Title = "Transport Simulation Game";

            //Set the render and update delegates
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            entities = MainEntry.entityManager.Entities();
            keyboardInput = MainEntry.inputManager.Keyboard();

            //Setup camera position and view values
            camera = MainEntry.camera;
            camera.Position = new Vector3(12, -35, 12);
            camera.Direction = new Vector3(12, 1, 12);
            camera.UpDirection = new Vector3(0, 0, 1);
            camera.FOV = 45;
            camera.AspectRatio = (1920 / 1080f);
            camera.Near = 0.01f;
            camera.Far = 100f;

            CreateEntities();

            //Retrieves all entities with a stock component and stores them in a factory list for use in the game
            factories = new List<oEntity>();
            foreach (oEntity entity in entities)
            {
                if (entity.GetStock() != null)
                {
                    factories.Add(entity);
                }
            }

            trucks = new List<oEntity>();

            food = 0;
            medical = 0;
            housing = 0;
            electronics = 0;
            population = 100;
            populationTemp = 100;
            money = 0;

            paused = false;

            pauseToggleTimer = 0;
            upgradeToggleTimer = 0;
            tickRateTimer = 0;
        }

        private void CreateEntities()
        {
            oEntity newEntity;

            //Random number generators for x and y co-ordinates
            Random rndX = new Random();
            Random rndY = new Random();
            Random rndColour = new Random();
            int carCounter = 0;

            //Randomly spawns cars around the map
            while (carCounter < 20)
            {
                //Generates random x and y co-ordinates
                int x = rndX.Next(0, MapLoader.mapX);
                int y = rndY.Next(0, MapLoader.mapY);
                int colour = rndColour.Next(0, 4);
                string texture = "";

                switch (colour)
                {
                    case 0:
                        {
                            texture = "Sprite Textures/RedCar.fw.png";
                            break;
                        }
                    case 1:
                        {
                            texture = "Sprite Textures/GreenCar.fw.png";
                            break;
                        }
                    case 2:
                        {
                            texture = "Sprite Textures/BlueCar.fw.png";
                            break;
                        }
                    case 3:
                        {
                            texture = "Sprite Textures/YellowCar.fw.png";
                            break;
                        }
                }

                if (MapLoader.map[x, y].Passable == true)
                {
                    //Randomly pathing cars
                    newEntity = new oEntity("Car");
                    newEntity.AddComponent(new cTransform(new Vector3(x, 0, y), new Vector3(1.5708f, 0, 0), new Vector3(0.3f, 0.2f, 0.25f)));
                    newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                    newEntity.AddComponent(new cTexture(texture));
                    newEntity.AddComponent(new cShader());
                    newEntity.AddComponent(new cAI(new Vector2(x, y), new Vector3(x, 0, y)));
                    newEntity.AddComponent(new cSpeed(3.0f));
                    MainEntry.entityManager.AddEntity(newEntity);
                    MainEntry.systemManager.AssignNewEntity(newEntity);

                    carCounter++;
                }
            }
        }

        /// <summary>
        /// Spawns a truck at a given starting location, with a set target location as well as a type and amount of stock to carry
        /// </summary>
        /// <param name="startingX">X value of starting location</param>
        /// <param name="startingZ">Z value of starting location</param>
        /// <param name="targetX">X value of target location</param>
        /// <param name="targetZ">Z value of target location</param>
        /// <param name="stockType">Type of stock the truck is carrying</param>
        /// <param name="stockCount">Amount of stock the truck is carrying</param>
        public void SpawnTruck(float startingX, float startingZ, float targetX, float targetZ, cStock.StockType stockType, int stockCount)
        {
            oEntity newEntity;

            newEntity = new oEntity("Truck");
            newEntity.AddComponent(new cTransform(new Vector3(startingX, 0, startingZ), new Vector3(1.5708f, 0, 0), new Vector3(0.5f, 0.25f, 0.25f)));
            newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
            newEntity.AddComponent(new cTexture("Sprite Textures/Trock.fw.png"));
            newEntity.AddComponent(new cShader());
            newEntity.AddComponent(new cAI(new Vector2(startingX, startingZ), new Vector3(startingX, 0, startingZ), new Vector2(targetX, targetZ), new Vector3(targetX, 0, targetZ)));
            newEntity.AddComponent(new cSpeed(2.0f));
            newEntity.AddComponent(new cStock(stockType, stockCount));
            MainEntry.entityManager.AddEntity(newEntity);
            MainEntry.systemManager.AssignNewEntity(newEntity);
            trucks.Add(newEntity);
        }

        /// <summary>
        /// Spawns a randomly coloured car at a random passable node on the map
        /// </summary>
        public void SpawnRandomCar()
        {
            oEntity newEntity;
            bool carSpawned = false;

            while(carSpawned != true)
            {
                //Random number generators for x and y co-ordinates
                Random rndX = new Random();
                Random rndY = new Random();
                Random rndColour = new Random();

                //Generates random x and y co-ordinates
                int x = rndX.Next(0, MapLoader.mapX);
                int y = rndY.Next(0, MapLoader.mapY);
                int colour = rndColour.Next(0, 4);
                string texture = "";

                switch (colour)
                {
                    case 0:
                        {
                            texture = "Sprite Textures/RedCar.fw.png";
                            break;
                        }
                    case 1:
                        {
                            texture = "Sprite Textures/GreenCar.fw.png";
                            break;
                        }
                    case 2:
                        {
                            texture = "Sprite Textures/BlueCar.fw.png";
                            break;
                        }
                    case 3:
                        {
                            texture = "Sprite Textures/YellowCar.fw.png";
                            break;
                        }
                }

                //Checks if node location is passable
                if (MapLoader.map[x, y].Passable == true)
                {
                    //Randomly pathing cars
                    newEntity = new oEntity("Car");
                    newEntity.AddComponent(new cTransform(new Vector3(x, 0, y), new Vector3(1.5708f, 0, 0), new Vector3(0.3f, 0.2f, 0.25f)));
                    newEntity.AddComponent(new cGeometry("Shape Geometries/SquareGeometry.txt"));
                    newEntity.AddComponent(new cTexture(texture));
                    newEntity.AddComponent(new cShader());
                    newEntity.AddComponent(new cAI(new Vector2(x, y), new Vector3(x, 0, y)));
                    newEntity.AddComponent(new cSpeed(3.0f));
                    MainEntry.entityManager.AddEntity(newEntity);
                    MainEntry.systemManager.AssignNewEntity(newEntity);

                    carSpawned = true;
                }
            }
        }

        public void Update(FrameEventArgs e)
        {
            pauseToggleTimer += sceneManager.dt;
            upgradeToggleTimer += sceneManager.dt;
            tickRateTimer += sceneManager.dt;

            //Pauses game when escape is pressed and unpauses when pressed again
            if (keyboardInput.Contains("Escape") && pauseToggleTimer > 0.25f)
            {
                if (paused != true)
                {
                    paused = true;
                    pauseToggleTimer = 0;
                }
                else
                {
                    paused = false;
                    pauseToggleTimer = 0;
                }
            }

            //Only executes if game isn't paused
            if (paused != true)
            {
                #region Camera movement
                //Moves camera left
                if (keyboardInput.Contains("A"))
                {
                    camera.MoveCamera(new Vector3(-0.1f, 0, 0));
                }

                //Moves camera right
                if (keyboardInput.Contains("D"))
                {
                    camera.MoveCamera(new Vector3(0.1f, 0, 0));
                }

                //Moves camera up
                if (keyboardInput.Contains("W"))
                {
                    camera.MoveCamera(new Vector3(0, 0, 0.1f));
                }

                //Moves camera down
                if (keyboardInput.Contains("S"))
                {
                    camera.MoveCamera(new Vector3(0, 0, -0.1f));
                }

                //Zooms camera in
                if (keyboardInput.Contains("Q"))
                {
                    camera.MoveCamera(new Vector3(0, 0.1f, 0));
                }

                //Zooms camera out
                if (keyboardInput.Contains("E"))
                {
                    camera.MoveCamera(new Vector3(0, -0.1f, 0));
                }
                #endregion

                if (tickRateTimer > 1)
                {
                    #region Delivery Factories
                    //Retrieves the delivery factories from the factories list
                    foreach (oEntity entity in factories)
                    {
                        if (entity.GetStock().StockClassValue == cStock.StockClass.DELIVERY)
                        {
                            //Retrieves stock type and amount from factory, then iterates on it by its production efficiency
                            Dictionary<cStock.StockType, float> stockCounts = entity.GetStock().StockCounts;
                            foreach (var key in stockCounts.Keys.ToList())
                            {
                                stockCounts[key] = stockCounts[key] + (entity.GetStock().ProductionEfficiency * 200);

                                //If stock count is above the delivery threshold the factory spawns a truck
                                if (stockCounts[key] > 5000)
                                {
                                    //If the stock is of type RAW_FOOD sets the trucks destination to the food factory
                                    if (key == cStock.StockType.RAW_FOOD)
                                    {
                                        //Retrieves the food factory from the factories list
                                        foreach (oEntity factory in factories)
                                        {
                                            if (factory.Name.Contains("Food"))
                                            {
                                                //Removes the stock to be delivered from the current delivery factory
                                                stockCounts[key] = stockCounts[key] - 5000;

                                                //Spawns truck at current delivery factories location with the food factory as the target location as well as adding stock of type RAW_FOOD to the truck entity
                                                SpawnTruck(entity.GetTransform().Translation.X, entity.GetTransform().Translation.Z,
                                                    factory.GetTransform().Translation.X, factory.GetTransform().Translation.Z,
                                                    cStock.StockType.RAW_FOOD, 5000);
                                            }
                                        }
                                    }

                                    //If the stock is of type RAW_MEDICAL sets the trucks destination to the medical factory
                                    if (key == cStock.StockType.RAW_MEDICAL)
                                    {
                                        //Retrieves the medical factory from the factories list
                                        foreach (oEntity factory in factories)
                                        {
                                            if (factory.Name.Contains("Medical"))
                                            {
                                                //Removes the stock to be delivered from the current delivery factory
                                                stockCounts[key] = stockCounts[key] - 5000;

                                                //Spawns truck at current delivery factories location with the medical factory as the target location as well as adding stock of type RAW_MEDICAL to the truck entity
                                                SpawnTruck(entity.GetTransform().Translation.X, entity.GetTransform().Translation.Z,
                                                    factory.GetTransform().Translation.X, factory.GetTransform().Translation.Z,
                                                    cStock.StockType.RAW_MEDICAL, 5000);
                                            }
                                        }
                                    }

                                    //If the stock is of type RAW_OIL sets the trucks destination to the electronics factory
                                    if (key == cStock.StockType.RAW_OIL)
                                    {
                                        //Retrieves the electronics factory from the factories list
                                        foreach (oEntity factory in factories)
                                        {
                                            if (factory.Name.Contains("Electronics"))
                                            {
                                                //Removes the stock to be delivered from the current delivery factory
                                                stockCounts[key] = stockCounts[key] - 5000;

                                                //Spawns truck at current delivery factories location with the electronics factory as the target location as well as adding stock of type RAW_OIL to the truck entity
                                                SpawnTruck(entity.GetTransform().Translation.X, entity.GetTransform().Translation.Z,
                                                    factory.GetTransform().Translation.X, factory.GetTransform().Translation.Z,
                                                    cStock.StockType.RAW_OIL, 5000);
                                            }
                                        }
                                    }

                                    //If the stock is of type RAW_WOOD sets the trucks destination to the housing factory
                                    if (key == cStock.StockType.RAW_WOOD)
                                    {
                                        //Retrieves the housing factory from the factories list
                                        foreach (oEntity factory in factories)
                                        {
                                            if (factory.Name.Contains("Housing"))
                                            {
                                                //Removes the stock to be delivered from the current delivery factory
                                                stockCounts[key] = stockCounts[key] - 5000;

                                                //Spawns truck at current delivery factories location with the housing factory as the target location as well as adding stock of type RAW_WOOD to the truck entity
                                                SpawnTruck(entity.GetTransform().Translation.X, entity.GetTransform().Translation.Z,
                                                    factory.GetTransform().Translation.X, factory.GetTransform().Translation.Z,
                                                    cStock.StockType.RAW_WOOD, 5000);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Production Factories
                    //Retrieves the production factories from the factories list
                    foreach (oEntity entity in factories)
                    {
                        if (entity.GetStock().StockClassValue == cStock.StockClass.PRODUCTION)
                        {
                            //Retrieves stock type and amount from factory, then iterates on it by its production efficiency
                            Dictionary<cStock.StockType, float> stockCounts = entity.GetStock().StockCounts;
                            foreach (var key in stockCounts.Keys.ToList())
                            {
                                if (stockCounts[key] > 0)
                                {
                                    stockCounts[key] = stockCounts[key] - (entity.GetStock().ProductionEfficiency * 200);
                                }

                                //Adds food if factory is producing food
                                if (key == cStock.StockType.RAW_FOOD && stockCounts[key] > 0)
                                {
                                    food += entity.GetStock().ProductionEfficiency * 100;
                                }

                                //Adds medical if factory is producing medical
                                if (key == cStock.StockType.RAW_MEDICAL && stockCounts[key] > 0)
                                {
                                    medical += entity.GetStock().ProductionEfficiency * 100;
                                }

                                //Adds electronics if factory is producing electronics
                                if (key == cStock.StockType.RAW_OIL && stockCounts[key] > 0)
                                {
                                    electronics += entity.GetStock().ProductionEfficiency * 100;
                                }

                                //Adds housing if factory is producing housing
                                if (key == cStock.StockType.RAW_WOOD && stockCounts[key] > 0)
                                {
                                    housing += entity.GetStock().ProductionEfficiency * 100;
                                }
                            }
                        }
                    }
                    #endregion

                    #region Population
                    //Removes resources for each member of the population
                    food = food - population;
                    medical = medical - population;
                    housing = housing - population;
                    electronics = electronics - population;

                    //Prevents resources from going negative
                    if (food < 0)
                    {
                        food = 0;
                    }
                    if (medical < 0)
                    {
                        medical = 0;
                    }
                    if (housing < 0)
                    {
                        housing = 0;
                    }
                    if (electronics < 0)
                    {
                        electronics = 0;
                    }

                    //Increases population if excess resources
                    if (food > 1 && medical > 1 && housing > 1 && electronics > 1)
                    {
                        population += 1;
                    }

                    //Increases money by population amount
                    money = money + population;
                    #endregion

                    tickRateTimer = 0;
                }

                #region Trucks
                List<oEntity> trucksToRemove = new List<oEntity>();

                foreach (oEntity truck in trucks)
                {
                    //Executes when truck reaches destination factory
                    if (truck.GetAI().DestinationReached == true)
                    {
                        //Retrieves the stock type from the truck entity
                        Dictionary<cStock.StockType, float> stockCounts = truck.GetStock().StockCounts;
                        foreach (var key in stockCounts.Keys.ToList())
                        {
                            //If truck is carrying RAW_FOOD then adds the RAW_FOOD to the stock of the food factory on arrival
                            if (key == cStock.StockType.RAW_FOOD)
                            {
                                //Searches the factory list for the food factory
                                foreach (oEntity factory in factories)
                                {
                                    if (factory.Name.Contains("Food"))
                                    {
                                        //Adds RAW_FOOD delivered by truck to the food factories stock
                                        Dictionary<cStock.StockType, float> factoryStockCounts = factory.GetStock().StockCounts;
                                        foreach (var key2 in factoryStockCounts.Keys.ToList())
                                        {
                                            if (key2 == cStock.StockType.RAW_FOOD)
                                            {
                                                factoryStockCounts[key2] = factoryStockCounts[key2] + stockCounts[key];
                                            }
                                        }
                                    }
                                }
                            }

                            //If truck is carrying RAW_MEDICAL then adds the RAW_MEDICAL to the stock of the medical factory on arrival
                            if (key == cStock.StockType.RAW_MEDICAL)
                            {
                                //Searches the factory list for the medical factory
                                foreach (oEntity factory in factories)
                                {
                                    if (factory.Name.Contains("Medical"))
                                    {
                                        //Adds RAW_MEDICAL delivered by truck to the medical factories stock
                                        Dictionary<cStock.StockType, float> factoryStockCounts = factory.GetStock().StockCounts;
                                        foreach (var key2 in factoryStockCounts.Keys.ToList())
                                        {
                                            if (key2 == cStock.StockType.RAW_MEDICAL)
                                            {
                                                factoryStockCounts[key2] = factoryStockCounts[key2] + stockCounts[key];
                                            }
                                        }
                                    }
                                }
                            }

                            //If truck is carrying RAW_OIL then adds the RAW_OIL to the stock of the electronics factory on arrival
                            if (key == cStock.StockType.RAW_OIL)
                            {
                                //Searches the factory list for the electronics factory
                                foreach (oEntity factory in factories)
                                {
                                    if (factory.Name.Contains("Electronics"))
                                    {
                                        //Adds RAW_OIL delivered by truck to the electronics factories stock
                                        Dictionary<cStock.StockType, float> factoryStockCounts = factory.GetStock().StockCounts;
                                        foreach (var key2 in factoryStockCounts.Keys.ToList())
                                        {
                                            if (key2 == cStock.StockType.RAW_OIL)
                                            {
                                                factoryStockCounts[key2] = factoryStockCounts[key2] + stockCounts[key];
                                            }
                                        }
                                    }
                                }
                            }

                            //If truck is carrying RAW_WOOD then adds the RAW_WOOD to the stock of the housing factory on arrival
                            if (key == cStock.StockType.RAW_WOOD)
                            {
                                //Searches the factory list for the housing factory
                                foreach (oEntity factory in factories)
                                {
                                    if (factory.Name.Contains("Housing"))
                                    {
                                        //Adds RAW_WOOD delivered by truck to the housing factories stock
                                        Dictionary<cStock.StockType, float> factoryStockCounts = factory.GetStock().StockCounts;
                                        foreach (var key2 in factoryStockCounts.Keys.ToList())
                                        {
                                            if (key2 == cStock.StockType.RAW_WOOD)
                                            {
                                                factoryStockCounts[key2] = factoryStockCounts[key2] + stockCounts[key];
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Destroys all references to truck entity in system and entity manager
                        trucksToRemove.Add(truck);
                        MainEntry.entityManager.RemoveEntity(truck);
                        MainEntry.systemManager.DestroyEntity(truck);
                    }
                }

                //Removes truck entity from trucks list after iteration is complete and clears trucksToRemove list
                foreach (oEntity truck in trucksToRemove)
                {
                    if (trucks.Contains(truck))
                    {
                        trucks.Remove(truck);
                    }
                }
                trucksToRemove.Clear();
                #endregion

                #region Factory Selection
                //Selects appropriate factory based on button press
                if (keyboardInput.Contains("Number1"))
                {
                    //Retrieves the farm factory from the factories list and assigns it as selected factory
                    foreach (oEntity factory in factories)
                    {
                        if (factory.Name.Contains("Farm"))
                        {
                            selectedFactory = factory;
                        }
                    }
                }
                if (keyboardInput.Contains("Number2"))
                {
                    //Retrieves the wood factory from the factories list and assigns it as selected factory
                    foreach (oEntity factory in factories)
                    {
                        if (factory.Name.Contains("Wood"))
                        {
                            selectedFactory = factory;
                        }
                    }
                }
                if (keyboardInput.Contains("Number3"))
                {
                    //Retrieves the oil factory from the factories list and assigns it as selected factory
                    foreach (oEntity factory in factories)
                    {
                        if (factory.Name.Contains("Oil"))
                        {
                            selectedFactory = factory;
                        }
                    }
                }
                if (keyboardInput.Contains("Number4"))
                {
                    //Retrieves the food medical from the factories list and assigns it as selected factory
                    foreach (oEntity factory in factories)
                    {
                        if (factory.Name.Contains("Medical"))
                        {
                            selectedFactory = factory;
                        }
                    }
                }
                if (keyboardInput.Contains("Number5"))
                {
                    //Retrieves the electronics factory from the factories list and assigns it as selected factory
                    foreach (oEntity factory in factories)
                    {
                        if (factory.Name.Contains("Electronics"))
                        {
                            selectedFactory = factory;
                        }
                    }
                }
                if (keyboardInput.Contains("Number6"))
                {
                    //Retrieves the food factory from the factories list and assigns it as selected factory
                    foreach (oEntity factory in factories)
                    {
                        if (factory.Name.Contains("Food"))
                        {
                            selectedFactory = factory;
                        }
                    }
                }
                if (keyboardInput.Contains("Number7"))
                {
                    //Retrieves the housing factory from the factories list and assigns it as selected factory
                    foreach (oEntity factory in factories)
                    {
                        if (factory.Name.Contains("Housing"))
                        {
                            selectedFactory = factory;
                        }
                    }
                }
                #endregion

                #region Factory Upgrading
                //Checks if user has pressed U and that a factory is selected
                if (keyboardInput.Contains("U") && selectedFactory != null && upgradeToggleTimer > 0.25f)
                {
                    //If factory upgrade modifier is still at default, subtracts base upgrade cost of 5000 from money and upgrades factory if money is greater than or equal to base upgrade cost of 5000
                    if (selectedFactory.GetStock().UpgradeCost == 0.0f)
                    {
                        if (money >= 5000)
                        {
                            selectedFactory.GetStock().ProductionEfficiency += 0.1f;
                            selectedFactory.GetStock().UpgradeCost += 0.1f;
                            money -= 5000;
                            upgradeToggleTimer = 0;
                        }
                    }
                    else
                    {
                        //Checks if money is greater than or equal to base upgrade cost multiplied by the upgrade cost modifier then subtracts total cost from money and upgrades factory
                        if (money >= (selectedFactory.GetStock().UpgradeCost * 5000))
                        {
                            selectedFactory.GetStock().ProductionEfficiency += 0.1f;
                            selectedFactory.GetStock().UpgradeCost += 0.1f;
                            money -= (selectedFactory.GetStock().UpgradeCost * 5000);
                            upgradeToggleTimer = 0;
                        }
                    }
                }
                #endregion

                #region Car Spawning
                //If population has grown by 10, spawns a new car entity of a random colour
                if((population - populationTemp) >= 5)
                {
                    SpawnRandomCar();
                    populationTemp = population;
                }
                #endregion

                MainEntry.systemManager.UpdateSystems();
            }

        }

        public void Render(FrameEventArgs e)
        {
            //Sets background color to sky blue
            GL.ClearColor(0.196078f, 0.6f, 0.8f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Calls Render Systems every frame
            MainEntry.systemManager.RenderSystems();

            #region Resource Bar
            //Sets size of resource bar
            GL.Viewport(0, sceneManager.Height - (sceneManager.Height / 40), sceneManager.Width, sceneManager.Height / 40);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height / 40, -1, 1);

            //Sets font size
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;

            //Draws text
            GUI.DrawText(new Rectangle(0, 960, (int)width / 6, (int)(fontSize * 2.0f)), "Excess Food: " + (int)food, (int)(fontSize * 0.2f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 960, (int)width / 6 * 3, (int)(fontSize * 2.0f)), "Excess Medical: " + (int)medical, (int)(fontSize * 0.2f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 960, (int)width / 6 * 5, (int)(fontSize * 2.0f)), "Excess Electronics: " + (int)electronics, (int)(fontSize * 0.2f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 960, (int)width / 6 * 7, (int)(fontSize * 2.0f)), "Excess Housing: " + (int)housing, (int)(fontSize * 0.2f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 960, (int)width / 6 * 9, (int)(fontSize * 2.0f)), "Total Population: " + (int)population, (int)(fontSize * 0.2f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 960, (int)width / 6 * 11, (int)(fontSize * 2.0f)), "Total Money: " + (int)money, (int)(fontSize * 0.2f), StringAlignment.Center, Color.White);

            //Sets background color of resource bar to black
            GUI.Render(Color.Black);
            #endregion

            #region Action Bar
            //Sets size of action bar
            GL.Viewport(0, sceneManager.Height - (sceneManager.Height), sceneManager.Width, sceneManager.Height / 5);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height / 5, -1, 1);

            //Sets font size
            width = sceneManager.Width; height = sceneManager.Height; fontSize = Math.Min(width, height) / 10f;

            //Draws factory list text
            GUI.DrawText(new Rectangle(0, 800, (int)width / 2, (int)(fontSize * 2.0f)), "Factories:", (int)(fontSize * 0.5f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 850, (int)width / 4, (int)(fontSize * 2.0f)), "1. Farm", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 900, (int)width / 4, (int)(fontSize * 2.0f)), "2. Wood", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 950, (int)width / 4, (int)(fontSize * 2.0f)), "3. Oil", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 850, (int)width / 4 * 2, (int)(fontSize * 2.0f)), "4. Medical", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 900, (int)width / 4 * 2, (int)(fontSize * 2.0f)), "5. Electronics", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 950, (int)width / 4 * 2, (int)(fontSize * 2.0f)), "6. Food", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            GUI.DrawText(new Rectangle(0, 850, (int)width / 4 * 3, (int)(fontSize * 2.0f)), "7. Housing", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);

            //Draws selected factory text
            if (selectedFactory != null)
            {
                string[] splitName = selectedFactory.Name.Split();
                Dictionary<cStock.StockType, float> stockCounts = selectedFactory.GetStock().StockCounts;
                float costModifier;

                if (selectedFactory.GetStock().UpgradeCost == 0.0f)
                {
                    costModifier = 1;
                }
                else
                {
                    costModifier = selectedFactory.GetStock().UpgradeCost;
                }

                GUI.DrawText(new Rectangle(0, 800, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Selected Factory: " + splitName[0], (int)(fontSize * 0.5f), StringAlignment.Center, Color.White);
                GUI.DrawText(new Rectangle(0, 850, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Production Speed: " + selectedFactory.GetStock().ProductionEfficiency, (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
                GUI.DrawText(new Rectangle(0, 900, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Upgrade Cost: " + (int)(5000 * costModifier), (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
                GUI.DrawText(new Rectangle(0, 950, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Stock: " + stockCounts.Keys.First() + " - " + (int)stockCounts.Values.First(), (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            }
            else
            {
                GUI.DrawText(new Rectangle(0, 800, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Selected Factory: ", (int)(fontSize * 0.5f), StringAlignment.Center, Color.White);
                GUI.DrawText(new Rectangle(0, 850, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Production Speed: ", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
                GUI.DrawText(new Rectangle(0, 900, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Upgrade Cost: ", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
                GUI.DrawText(new Rectangle(0, 950, (int)width / 2 * 3, (int)(fontSize * 2.0f)), "Stock: ", (int)(fontSize * 0.3f), StringAlignment.Center, Color.White);
            }

            //Sets background color of action bar to black
            GUI.Render(Color.Black);
            #endregion
        }
    }
}
