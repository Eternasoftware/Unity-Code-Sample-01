using System;
using System.Collections.Generic;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.Decoration;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.ScriptableObjects.Items;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using Game.Scripts.Core;
using Memento;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using ItemData = _GameData_.Scripts.Entities.Player.ItemData;

namespace _GameData_.AI.GOAP
{
    [RequireComponent(typeof(BackpackComponent), typeof(AgentBehaviour))]
    public class AIInstrumentsSwitcher : SerializedMonoBehaviour
    {
        [Inject] private GameplayComponent gameplayComponent;
        [Inject] private GameObjectFactory gameObjectFactory;
        [Inject] private DecorationBox decorationBox;
        
        [field:SerializeField] public List<ItemData> ItemData { get; private set; }

        [field: SerializeField] public List<GoalConfigScriptable> backpackGoals;

        public GameObject prevInstrumentGameObject { get; private set; }

        private AgentBehaviour agentBehaviour;

        public Dictionary<WorldKeyScriptable, InstrumentItem> settings = new Dictionary<WorldKeyScriptable, InstrumentItem>();

        private IInstrumentComponent hasCurrentInstrument;
        
        public IInstrumentComponent CurrentInstrument => hasCurrentInstrument;

        public InstrumentItem CurrentInstrumentItem { get; set; }
        
        public WorldKeyScriptable CurrentWorldKeyInstrumentItem { get; private set; }

        public BackpackComponent BackpackComponent { get; set; }

        public event Action<WorldKeyScriptable> OnTakeInstrument;
        public event Action<WorldKeyScriptable> OnDropInstrument;
        
        private void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
            BackpackComponent = GetComponent<BackpackComponent>();
        }

        private void OnEnable()
        {
            agentBehaviour.Events.OnGoalStart += OnHandleNewGoal;
            BackpackComponent.IsEmptyHands += OnHandleEmptyHands;
        }

        private void OnDisable()
        {
            agentBehaviour.Events.OnGoalStart -= OnHandleNewGoal;
            BackpackComponent.IsEmptyHands -= OnHandleEmptyHands;
        }

        private void OnHandleEmptyHands()
        {
            if (!gameplayComponent.IsProgressMax())
            {
                if (gameplayComponent.GetCurrentState() == IDStateGame.decorationFloor && !decorationBox.Visible && BackpackComponent.AllBackpackIsEmpty())
                {
                    decorationBox.ReEnableMeshRender();
                }
            }
        }

        private void OnHandleNewGoal(IGoalBase goal)
        {
            CheckBackpack(goal);
        }

        private void CheckBackpack(IGoalBase goal)
        {
            bool wasActivated = false;
            foreach (var vSetting in backpackGoals)
            {
                if (vSetting.Name == goal.Config.Name)
                {
                    wasActivated = true;
                    if (!BackpackComponent.enabled)
                    {
                        BackpackComponent.enabled = true;
                    }
                }
            }
            if (!wasActivated)
            {
                BackpackComponent.enabled = false;
            }
        }
        
        public void ActiavateInstrummentForCurrentGoal(IEffect[] effects)
        {
            foreach (var vSetting in settings)
            {
                foreach (var takeInstr in effects)
                {
                    if (vSetting.Key.Name == takeInstr.WorldKey.Name)
                    {
                        CurrentWorldKeyInstrumentItem = vSetting.Key;
                        ActivateInstrument(vSetting.Value);
                        OnTakeInstrument?.Invoke(CurrentWorldKeyInstrumentItem);
                        return;
                    }   
                }
            }
        }

        public void DropInstrument()
        {
            if (prevInstrumentGameObject != null)
            {
                Destroy(prevInstrumentGameObject);
            }
            OnDropInstrument?.Invoke(CurrentWorldKeyInstrumentItem);
            CurrentInstrumentItem = null;
            hasCurrentInstrument = null;
            CurrentWorldKeyInstrumentItem = null;
        }

        public void ActivateInstrument(InstrumentItem instrumentItem)
        {
            Vector2Int data = MementoSystem.Load<Vector2Int>(instrumentItem.id);
            if (prevInstrumentGameObject != null)
            {
                Destroy(prevInstrumentGameObject);
            }
            ItemData itemData = GetItemData(instrumentItem);
            CurrentInstrumentItem = instrumentItem;
            if (data.x > instrumentItem.data.Count)
            {
                data.x = 0;
            }
            prevInstrumentGameObject = gameObjectFactory.Create(instrumentItem.data[data.x].prefab, itemData.parentForInstrument.transform);
            prevInstrumentGameObject.transform.localPosition = Vector3.zero;
            prevInstrumentGameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            hasCurrentInstrument = prevInstrumentGameObject.GetComponent<IInstrumentComponent>();
        }

        public ItemData GetItemData(InstrumentItem instrumentItem)
        {
            for (int i = 0; i < ItemData.Count; i++)
            {
                if (ItemData[i].instrumentItem.id == instrumentItem.id)
                {
                    return ItemData[i];
                }
            }
            Debug.LogError("I Can't find Item data");
            return ItemData[0];
        }
    }
}