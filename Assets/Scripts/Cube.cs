using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private float _minTimeLife = 2.0f;
    private float _maxTimeLife = 5.0f;
    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _disappearCoroutine;
    private bool _hasTouchedPlatform = false;

    public event Action<Cube> OnReleased;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ResetCube();
    }

    public void SetInitialVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    private void ResetCube()
    {
        _hasTouchedPlatform = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _renderer.material.color = Color.red;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasTouchedPlatform == false && collision.gameObject.TryGetComponent(out Platform platform) == true)
        {
            _hasTouchedPlatform = true;
            _renderer.material.color = Random.ColorHSV();
            StartDisappearing();
        }
    }    

    private void StartDisappearing()
    {
        if (_disappearCoroutine != null)
        {
            StopCoroutine(_disappearCoroutine);
        }

        _disappearCoroutine = StartCoroutine(DisappearAfterDelay());
    }

    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(Random.Range(_minTimeLife, _maxTimeLife));
        OnReleased?.Invoke(this);
    }
}