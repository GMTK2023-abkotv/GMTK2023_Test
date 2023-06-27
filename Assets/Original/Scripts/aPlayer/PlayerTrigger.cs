using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField]
    Collider _trigger;

    void Awake()
    { 
        PlayerDelegatesContainer.EventPlayerDead  += OnDead;
        PlayerDelegatesContainer.EventPlayerAlive += OnAlive;
    }
     
    void OnDestroy()
    { 
        PlayerDelegatesContainer.EventPlayerDead  -= OnDead;
        PlayerDelegatesContainer.EventPlayerAlive -= OnAlive;
    }

    void OnDead()
    {
        _trigger.enabled = false;
    }

    void OnAlive()
    {
        _trigger.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Holes
            || other.gameObject.layer == Layers.Enemies)
        {
            PlayerDelegatesContainer.EventPlayerDead();
        }
    }
}