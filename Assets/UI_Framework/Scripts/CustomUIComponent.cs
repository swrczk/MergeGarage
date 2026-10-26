using UnityEngine;

namespace UI_Framework.Scripts
{
    public abstract class CustomUIComponent : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        private void OnValidate()
        {
            Init();
        }

        public abstract void Setup();
        public abstract void Configure();

        public void Init()
        {
            Setup();
            Configure();
        }
    }
}