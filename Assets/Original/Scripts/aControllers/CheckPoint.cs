using System.Collections;

using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    Renderer _modelRenderer;

    [SerializeField]
    Material _idleMaterial;

    [SerializeField]
    Material _activeMaterial;

    [SerializeField]
    float _lerpSpeed;

    bool _isActive;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActive)
        {
            return;
        }

        if (other.TryGetComponent(out PlayerCharacter player))
        {
            ApplicationDelegatesContainer.EventCheckPointEntered?.Invoke(this);
        }
    }

    public void ShowActivation()
    {
        _isActive = true;
        StartCoroutine(ShowActivationAnimation());
    }

    IEnumerator ShowActivationAnimation()
    {
        float lerpParam = 0;
        while (lerpParam < 1)
        {
            lerpParam += _lerpSpeed * Time.deltaTime;
            _modelRenderer.material.Lerp(_idleMaterial, _activeMaterial, lerpParam);
            yield return null;
        }
    }

    public void ShowDeactivation()
    {
        _isActive = false;
        StartCoroutine(ShowDeactivationAnimation());
    }

    IEnumerator ShowDeactivationAnimation()
    { 
        float lerpParam = 0;
        while (lerpParam < 1)
        {
            lerpParam += _lerpSpeed * Time.deltaTime;
            _modelRenderer.material.Lerp(_activeMaterial, _idleMaterial, lerpParam);
            yield return null;
        }
    }
}