using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Component_Based_Game_Engine.Managers;
using Component_Based_Game_Engine.Systems;
using Component_Based_Game_Engine.Objects;

namespace Transport_Simulation_Game
{
    public static class MainEntry
    {
        public static mEntity entityManager;
        public static mSystem systemManager;
        public static mInput inputManager;
        public static mScene sceneManager;
        public static oCamera camera;

        static void Main()
        {
            sceneManager = new mScene();
            entityManager = new mEntity();
            systemManager = new mSystem();
            inputManager = new mInput(sceneManager);
            camera = new oCamera(new Vector3(12, -35, 12), new Vector3(12, 1, 12), new Vector3(0, 0, 1), 45, 1920f / 1080f, 0.1f, 100f);

            MapLoader.loadMap("Maps/Map.txt");
            CreateSystems();

            using (sceneManager)
            {
                sceneManager.LoadScene(new Game(sceneManager));
                sceneManager.Run(60.0, 0.0);
            }
        }
        private static void CreateSystems()
        {
            ISystem newSystem;

            //Update Systems
            newSystem = new sAI(MapLoader.map, MapLoader.mapX, MapLoader.mapY);
            systemManager.AddUpdateSystem(newSystem);
            newSystem = new sPhysics(sceneManager);
            systemManager.AddUpdateSystem(newSystem);

            //Render Systems
            newSystem = new sRender(camera, sceneManager.ClientRectangle);
            systemManager.AddRenderSystem(newSystem);

            //Assigns entities to appropriate systems
            systemManager.AssignEntities(entityManager);
        }
    }
}
