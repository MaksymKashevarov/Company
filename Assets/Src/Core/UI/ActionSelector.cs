namespace Src.Core.UI
{
    using Src.Core.Objects;
    using System.Collections.Generic;
    using UnityEngine;

    public class ActionSelector : MonoBehaviour
    {
        [SerializeField] private ActionComponent _actionComponent;
        private List<ActionComponent> _actions = new List<ActionComponent>();

        public void RequestActionBuild(ITObject tObject)
        {
            ObjectData data = tObject.GetObjectData();
            if (data == null)
            {
                return;
            }
            if (!data.IsPersistent())
            {
                ActionComponent actionComponent = _buildAction("Delete");
            }

        }


        private ActionComponent _buildAction(string buttonText)
        {
            ActionComponent component = Instantiate( _actionComponent, this.transform);
            ButtonData data = component.GetButtonData();
            data.SetText(buttonText);
            _actions.Add(component);
            return component;
        }

        private void _resetActions()
        {
            foreach (ActionComponent action in _actions)
            {
                Destroy(action.gameObject);

            }
            _actions.Clear();
        }

    }

}

