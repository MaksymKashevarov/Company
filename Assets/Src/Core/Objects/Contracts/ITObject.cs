using UnityEngine;

namespace Src.Core.Objects
{
    public interface ITObject
    {
        ObjectData GetObjectData();
        GameObject GetGameObject();
    }
}
