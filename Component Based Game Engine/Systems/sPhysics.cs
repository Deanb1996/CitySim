using Component_Based_Game_Engine.Components;
using Component_Based_Game_Engine.Managers;
using Component_Based_Game_Engine.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component_Based_Game_Engine.Systems
{
    public class sPhysics : ISystem
    {
        const ComponentMasks MASK = (ComponentMasks.COMPONENT_TRANSFORM | ComponentMasks.COMPONENT_SPEED);

        List<oEntity> entityList;
        mScene sceneManager;

        public sPhysics(mScene sceneManagerIn)
        {
            entityList = new List<oEntity>();
            sceneManager = sceneManagerIn;
        }

        public string Name
        {
            get { return "SystemPhysics"; }
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
            foreach (oEntity entity in entityList)
            {
                //Retrieves list of components from current entity
                List<IComponent> components = entity.Components;

                //Retrieves transform component from current entity
                IComponent transformComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_TRANSFORM;
                });

                //Sets transform if not already set
                if (((cTransform)transformComponent).SetTransform == false)
                {
                    UpdateTransform((cTransform)transformComponent);
                    ((cTransform)transformComponent).SetTransform = true;
                }

                //Retrieves speed component from current entity
                IComponent speedComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_SPEED;
                });

                //Retrieves AI component from current entity
                IComponent aiComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_AI;
                });


                #region AI movement
                //Checks if the entity has an AI component, then applies movement based on AI path
                if (aiComponent != null && ((cAI)aiComponent).DestinationReached != true)
                {
                    cAI ai = ((cAI)aiComponent);
                    float velocity = ((cSpeed)speedComponent).Speed;
                    cTransform transform = ((cTransform)transformComponent);
                    Vector3 direction;
                    float angle;

                    //Finds the direction towards the target node
                    direction = new Vector3(ai.NextNodeLocation - transform.Translation).Normalized();

                    //Finds the angle between the target direction and the agent forward vector
                    angle = (float)(Math.Atan2(direction.X, direction.Z) - Math.Atan2(transform.Right.X, transform.Right.Z));

                    //if the angle is over 180 in either direction then invert it to be a smaller angle in the opposite direction
                    if (angle > Math.PI)
                    {
                        angle -= (float)(2 * Math.PI);
                    }
                    if (angle < -Math.PI)
                    {
                        angle += (float)(2 * Math.PI);
                    }

                    //Rotates the agent towards the target location if not facing it
                    transform.Rotation += new Vector3(0, angle, 0);

                    //Moves agent towards target location
                    transform.Translation += transform.Right * velocity * sceneManager.dt;

                    //Checks if entity is moving left
                    if (direction.X < -0.3f)
                    {
                        //Checks if entity has reached the next node then sets current node to the next node causing the AI system to update to a new next node
                        if ((transform.Translation.X) < (ai.NextNodeLocation.X + 0.1f))
                        {
                            ai.CurrentNode = ai.NextNode;
                        }
                    }
                    //Checks if entity is moving down
                    else if(direction.Z < -0.3f)
                    {
                        //Checks if entity has reached the next node then sets current node to the next node causing the AI system to update to a new next node
                        if ((transform.Translation.Z) < (ai.NextNodeLocation.Z + 0.1f))
                        {
                            ai.CurrentNode = ai.NextNode;
                        }
                    }
                    else
                    {
                        //Checks if entity has reached the next node then sets current node to the next node causing the AI system to update to a new next node
                        if ((int)(transform.Translation.X + 0.1f) == ai.NextNodeLocation.X && (int)(transform.Translation.Z + 0.1f) == ai.NextNodeLocation.Z)
                        {
                            ai.CurrentNode = ai.NextNode;
                        }
                    }
                }
                #endregion

                //Updates transform of entity every frame
                UpdateTransform((cTransform)transformComponent);
            }
        }

        public void UpdateTransform(cTransform transform)
        {
            Matrix4 scaleMat = Matrix4.CreateScale(transform.Scale);
            Matrix4 rotateMat = Matrix4.CreateRotationX(transform.Rotation.X) * Matrix4.CreateRotationY(transform.Rotation.Y) * Matrix4.CreateRotationZ(transform.Rotation.Z);
            Matrix4 translateMat = Matrix4.CreateTranslation(transform.Translation);

            transform.Transform = scaleMat * rotateMat * translateMat;

            transform.Up = new Vector3(transform.Transform[1, 0], transform.Transform[1, 1], transform.Transform[1, 2]).Normalized();
            transform.Forward = -new Vector3(transform.Transform[2, 0], transform.Transform[2, 1], transform.Transform[2, 2]).Normalized();
            transform.Right = new Vector3(transform.Transform[0, 0], transform.Transform[0, 1], transform.Transform[0, 2]).Normalized();
        }
    }
}
