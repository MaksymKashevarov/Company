namespace Src.Core.UI
{
    using UnityEngine;

    public class ActionComponent : MonoBehaviour
    {
        [SerializeField] private ButtonData _button;

        public GameObject GetGameObject()
        {
            return _button.gameObject;
        }

        public ButtonData GetButtonData()
        {
            return _button;
        }
    }

}