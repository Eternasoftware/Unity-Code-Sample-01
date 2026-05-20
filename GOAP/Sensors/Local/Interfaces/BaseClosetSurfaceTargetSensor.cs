using System.Collections.Generic;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Cleaning;
using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using _GameData_.Scripts.ScriptableObjects.Items;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public abstract class BaseClosetSurfaceTargetSensor<T>: LocalTargetSensorBase, IInjectable where T : DirtSurfaceComponent
    {
        [Inject] private AIGeneralGameplayBlackboard aiGeneralGameplayBlackboard;
        protected DirtySurfaceBlackboard<T> SurfaceBlackboard;
        public List<IDStateGame> AvailableStates;
        private Dictionary<IMonoAgent, PositionTarget> cachePositions;

        public override void Created()
        {
            AvailableStates = new List<IDStateGame>();
            cachePositions = new Dictionary<IMonoAgent, PositionTarget>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            if (!cachePositions.ContainsKey(agent))
            {
                cachePositions.Add(agent, new PositionTarget(agent.transform.position));
            }
            
            var tmp = aiGeneralGameplayBlackboard.cacheState;
            if (AvailableStates == null || !AvailableStates.Contains(tmp))
            {
                return null;
            }
            
            if (SurfaceBlackboard.CurrentSurface == null || SurfaceBlackboard.CurrentSurface.IsComplete)
            {
                SetupNewSurface(agent.transform.position);
            }
            
            if (SurfaceBlackboard.CurrentSurface != null)
            {
                if (SurfaceBlackboard.CurrentSurface.globalCoordinates == null || SurfaceBlackboard.CurrentSurface.globalCoordinates.Count == 0)
                {
                    var bounds = SurfaceBlackboard.CurrentSurface.Collider.bounds;
                    float x = Random.Range(bounds.min.x, bounds.max.x);
                    float y = agent.transform.position.y;
                    float z = Random.Range(bounds.min.z, bounds.max.z);
                    cachePositions[agent].Position = new Vector3(x, y, z);
                    return cachePositions[agent];
                }
                int r = Random.Range(0, SurfaceBlackboard.CurrentSurface.globalCoordinates.Count);
                if (SurfaceBlackboard.CurrentSurface.globalCoordinates[r].IsClean)
                {
                    var data = GetClosetPoint(agent.transform.position, SurfaceBlackboard.CurrentSurface.globalCoordinates);
                    cachePositions[agent].Position = new Vector3(data.x, agent.transform.position.y, data.z);
                    return cachePositions[agent];
                }
                else
                {
                    var data = SurfaceBlackboard.CurrentSurface.globalCoordinates[r];
                    cachePositions[agent].Position = new Vector3(data.Position.x, agent.transform.position.y, data.Position.z);
                    return cachePositions[agent];
                }
                // var data = GetClosetPoint(agent.transform.position, SurfaceBlackboard.CurrentSurface.globalCoordinates);
                // cachePositions[agent].Position = new Vector3(data.x, agent.transform.position.y, data.z);
                // return cachePositions[agent];
            }
            return null;
        }

        protected abstract void SetupNewSurface(Vector3 position);

        protected void FindNewSurface(Vector3 position, List<T> dirtSurfaceComponents)
        {
            float minDistance = float.MaxValue;
            T near = null;
            foreach (var container in dirtSurfaceComponents)
            {
                if (!container.IsComplete)
                {
                    float distance = Vector3.Distance(container.transform.position, position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        near = container;
                    }
                }
            }
            SurfaceBlackboard.SetSurface(near);
        }
        
        protected Vector3 GetClosetPoint(Vector3 position, List<DirtSurfaceComponent.CoordinateData> data)
        {
            float maxDistance = float.MinValue;
            Vector3 near = Vector3.zero;
            for (int i = 0; i < data.Count; i++)
            {
                if (!data[i].IsClean)
                {
                    float distance = Vector3.Distance(data[i].Position, position);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        near = data[i].Position;
                    }
                }
            }
            return near;  
        }
    }
}