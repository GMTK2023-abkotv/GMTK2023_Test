using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerTriggerEventsBroadcaster : MonoBehaviour
{
    Collider2D _collider;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerDelegatesContainer.EventPlayerOnTriggerEnter2D?.Invoke(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerDelegatesContainer.EventPlayerOnTriggerStay2D?.Invoke(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerDelegatesContainer.EventPlayerOnTriggerExit2D?.Invoke(other);
    }

    void Awake()
    { 
        PlayerDelegatesContainer.EventDeath      += OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnEnd   += OnPlayerSpawnEnd;
    }
     
    void OnDestroy()
    { 
        PlayerDelegatesContainer.EventDeath      -= OnPlayerDeath;
        PlayerDelegatesContainer.EventSpawnEnd   -= OnPlayerSpawnEnd;
    }

    void OnPlayerDeath()
    {
        _collider.enabled = false;
    }

    void OnPlayerSpawnEnd()
    {
        _collider.enabled = true;
    }
}