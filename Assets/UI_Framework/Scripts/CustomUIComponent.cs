using UnityEngine;

namespace UI_Framework.Scripts
{
    public abstract class CustomUIComponent : MonoBehaviour
    {
        public abstract void Setup();
        public abstract void Configure();

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            Setup();
            Configure();
        }

        private void OnValidate()
        {
            Init();
        }
    }
}