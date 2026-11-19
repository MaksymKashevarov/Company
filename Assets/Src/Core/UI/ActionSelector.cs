namespace Src.Core.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ActionSelector : MonoBehaviour
    {
        [SerializeField] private ActionComponent _actionComponent;
        private List<ActionComponent> _actions = new List<ActionComponent>();

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

