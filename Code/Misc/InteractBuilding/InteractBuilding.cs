using Players;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Works.Factory.Code.Misc.InteractBuilding
{
    public class InteractBuilding : InteractObject
    {
        [SerializeField] private string sceneName;

        protected override void HandleInteract()
        {
            base.HandleInteract();
            SceneManager.LoadScene(sceneName);
        }
    }
}