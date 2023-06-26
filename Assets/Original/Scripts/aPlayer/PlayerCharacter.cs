using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    protected void Awake()
    {
        PlayerDelegatesContainer.GetTransform += GetTransform;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.GetTransform -= GetTransform;
    }

    Transform GetTransform()
    {
        return transform;
    }
}
