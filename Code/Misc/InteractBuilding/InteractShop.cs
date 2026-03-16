using Code.Core.EventSystems;
using GondrLib.Dependencies;
using Mining.Events;
using Players;
using Unity.Cinemachine;
using UnityEngine;

namespace Works.Factory.Code.Misc.InteractBuilding
{
    public class InteractShop : InteractObject
    {
        [SerializeField] private GameObject shopUI;
        [SerializeField] private CinemachineCamera cam;
        
        override protected void HandleInteract()
        {
            shopUI.SetActive(true);
            GameEventBus.RaiseEvent(CursorEvents.OnOffCursorEvent.SetEvent(true, gameObject));

            if (cam != null)
                cam.Priority = 20;
            
            _player.OnOffPlayer(false);
        }

        public void CloseInteract()
        {
            shopUI.SetActive(false);
            GameEventBus.RaiseEvent(CursorEvents.OnOffCursorEvent.SetEvent(false, gameObject));
            if (cam != null)
                cam.Priority = 0;
            _player.OnOffPlayer(true);
        }
    }
}