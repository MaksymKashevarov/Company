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
            _selector.gameObject.SetActive(false);
        }

        public void ProcessClickOnObject(ITObject tObject, Vector2 screenPosition)
        {
            GameObject selectedObject = tObject.GetGameObject();

            RectTransform canvasRect = (RectTransform)_canvas.transform;
            RectTransform selectorRect = (RectTransform)_selector.transform;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, screenPosition, _canvas.worldCamera, out Vector2 localPosition))
                return;

            selectorRect.anchoredPosition = localPosition;
            _selector.RequestActionBuild(tObject);
            _selector.gameObject.SetActive(true);
        }
    }

}