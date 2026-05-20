using Somnambulo.Scripts.Runtime.Infrastructure.Installers.Binders;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Somnambulo.Scripts.Runtime.Infrastructure.Installers
{
    public class SceneMasterBinder : IStartable
    {
        private readonly SceneWeaponsBinder weaponsBinder;
        private readonly SceneDoorsBinder doorsBinder;
        private readonly SceneItemsBinder itemsBinder;
        private readonly SceneTriggersBinder triggersBinder;
        private readonly SceneFabricCutBinder fabricCutBinder;
        private readonly ScenePuzzlesBinder puzzlesBinder;
        private readonly SceneSocketsBinder socketsBinder;
        private readonly SceneInventoryBinder inventoryBinder;

        [Inject]
        public SceneMasterBinder(
            SceneWeaponsBinder weaponsBinder,
            SceneDoorsBinder doorsBinder,
            SceneItemsBinder itemsBinder,
            SceneTriggersBinder triggersBinder,
            SceneFabricCutBinder fabricCutBinder,
            ScenePuzzlesBinder puzzlesBinder,
            SceneSocketsBinder socketsBinder,
            SceneInventoryBinder inventoryBinder
        )
        {
            this.weaponsBinder = weaponsBinder;
            this.doorsBinder = doorsBinder;
            this.itemsBinder = itemsBinder;
            this.triggersBinder = triggersBinder;
            this.fabricCutBinder = fabricCutBinder;
            this.puzzlesBinder = puzzlesBinder;
            this.socketsBinder = socketsBinder;
            this.inventoryBinder = inventoryBinder;
        }
        
        public void Start()
        {
            Debug.Log("--- [BOOTSTRAP] Scene Initialization Started ---");
            
            doorsBinder.Bind();
            weaponsBinder.Bind();
            itemsBinder.Bind();
            triggersBinder.Bind();
            fabricCutBinder.Bind();
            puzzlesBinder.Bind();
            socketsBinder.Bind();
            inventoryBinder.Bind();

            Debug.Log("--- [BOOTSTRAP] Scene Initialization Completed ---");
        }
    }
    
}