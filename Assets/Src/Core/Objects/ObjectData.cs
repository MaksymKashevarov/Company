namespace Src.Core.Objects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewObjectData", menuName = "Company/Objects/Object Data")]
    public class ObjectData : ScriptableObject
    {
        [SerializeField] private string _objectName;
        [SerializeField] private bool _isConsumable;
        [SerializeField] private bool _isPersistent;

    }

}

