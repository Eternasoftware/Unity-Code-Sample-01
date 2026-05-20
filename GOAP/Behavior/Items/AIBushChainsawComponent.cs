using System;
using System.Collections.Generic;
using _GameData_.AI.GOAP;
using _GameData_.Scripts.Entities.GrassCutting;
using _GameData_.Scripts.Entities.Items.Interfaces;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.Entities.Player.Interfaces;
using _GameData_.Scripts.ScriptableObjects.Items;
using RootMotion.FinalIK;
using UnityEngine;
using Zenject;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AIBushChainsawComponent : MonoBehaviour, IInstrumentComponent, ISpeedSource, IInteractableInstrument
    {
        [Inject] private GrassPackSpawner GrassPackSpawner;
        [Inject] private AIInstrumentsSwitcher aiInstrumentsSwitcher;

        [Header("General")]
        public InstrumentItem InstrumentItem;
        
        [Header("Render Settings")]
        [Range(1,10)][SerializeField] private int amountChainPerLevel = 2;
        [SerializeField] private int defaultAmountsChainElement = 7;
        [SerializeField] private GameObject prefabChain;
        [SerializeField] private GameObject prefabCap;
        [SerializeField] private float stepZ = -0.084f;
        [SerializeField] private float stepZCap = 0.042f;
        [SerializeField] private Transform pivotToStartSpawnsChain;
        private List<GameObject> chains;
        private const int lvl = 3;

        [System.Serializable]
        public class RotateSettings
        {
            public float speed = 1;
            public float angle = 45;
        }
        [SerializeField] private RotateSettings rotateSettings;
        
        // animation
        private BipedIK bipedIK;
        private Transform targetTransform;
        
        public Transform Transform => aiInstrumentsSwitcher.transform; 
        public bool Interactable { get; private set; }
        
        public event Action<float> UpdateSpeed;

        private void Awake()
        {
            chains = new List<GameObject>();
            bipedIK = aiInstrumentsSwitcher.GetComponent<UnitAnimator>().BipedIKBushAnimation;
            Setup();
            transform.parent = aiInstrumentsSwitcher.GetItemData(InstrumentItem).parentForIdlePosition.transform;
            PutOnZeroLocalCoordinates();
        }

        private void OnEnable()
        {
            GrassPackSpawner.AddInteractableInstrument(this);
        }

        private void OnDisable()
        {
            GrassPackSpawner.DeleteInteractableInstrument(this);
        }

        public void Setup()
        {
            int amount = (lvl * amountChainPerLevel) + defaultAmountsChainElement;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < amount - 1; i++)
            {
                var obj = Instantiate(prefabChain, pivotToStartSpawnsChain);
                obj.transform.localPosition = pos;
                pos = new Vector3(pos.x, pos.y, pos.z + stepZ);
                chains.Add(obj);
            }
            pos = new Vector3(pos.x, pos.y, pos.z + stepZCap);
            var cap = Instantiate(prefabCap, pivotToStartSpawnsChain);
            cap.transform.localPosition = pos;
            chains.Add(cap);
        }

        public void StartInteract(Transform target)
        {
            if (Interactable)
            {
                return;
            }

            targetTransform = target;
            aiInstrumentsSwitcher.transform.LookAt(target);

            bipedIK.enabled = true;
            transform.parent = aiInstrumentsSwitcher.GetItemData(InstrumentItem).parentForInstrument.transform;
            PutOnZeroLocalCoordinates();

            Interactable = true;
            
            UpdateSpeed?.Invoke(1);
        }

        public void LateUpdate()
        {
            if (Interactable && targetTransform != null)
            {
                float angleOffset = Mathf.Sin(Time.time * rotateSettings.speed) * rotateSettings.angle;
                Vector3 direction = targetTransform.position - aiInstrumentsSwitcher.transform.position;
                direction = Quaternion.Euler(0, angleOffset, 0) * direction;
                aiInstrumentsSwitcher.transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        public void EndInteract()
        {
            if (!Interactable)
            {
                return;
            }
            Interactable = false;
            bipedIK.enabled = false;
            transform.parent = aiInstrumentsSwitcher.GetItemData(InstrumentItem).parentForIdlePosition.transform;

            PutOnZeroLocalCoordinates();
            targetTransform = null;
            Interactable = false;
            
            UpdateSpeed?.Invoke(0);
        }
        
        private void PutOnZeroLocalCoordinates()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}