namespace Src.Core.Player
{
    using Src.Core.Objects;
    using Src.Core.UI;
    using UnityEngine;

    public class PlayerInterfaceController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ActionSelector _selector;
        public void BindComponents(Canvas canvas, ActionSelector selector)
        {
            _canvas = canvas;
            _selector = selector;
        }
        public void ProcessClickOnObject(ITObject tObject)
        {
            

        }
    }

}