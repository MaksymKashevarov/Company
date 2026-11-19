namespace Src.Core.Objects
{
    using Core.Objects;
    using UnityEngine;

    public class ObjectInfo : MonoBehaviour, ITObject
    {
        [SerializeField] private ObjectData _objectType;
        
        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public ObjectData GetObjectData()
        {
            return _objectType;
        }
    }

}
