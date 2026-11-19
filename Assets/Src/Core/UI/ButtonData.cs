namespace Src.Core.UI
{
    using TMPro;
    using UnityEngine;

    public class ButtonData : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        public void SetText(string text)
        {
            _text.text = text;
        }
    }

}

