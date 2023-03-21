using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimationManager : MonoBehaviour
{
    public static KeyAnimationManager Instance { get; private set; }

    [SerializeField]
    float _goalZ = 0f;

    [SerializeField]
    float _speed = 5;

    bool _isActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception();
        }
        else
        {
            Instance = this;
        }
    }

    Rigidbody _key;

    public void SetKey(bool active)
    {
        _key = LockManager.Lock.GetKey();
        _key.gameObject.SetActive(active);
        _isActive = active;

        if (active)
        {
            StartCoroutine(KeyAnimation());
        }
    }

    IEnumerator KeyAnimation()
    {
        while(true)
        {
            yield return null;

            if (_key == null)
            {
                break;
            }

            Vector3 movePosition = _key.transform.position;

            movePosition.z = Mathf.MoveTowards(_key.transform.position.z, _goalZ, _speed * Time.deltaTime);

            _key.MovePosition(movePosition);

            if (_key.transform.position.z == _goalZ)
            {
                break;
            }

        }
        
    }

    public bool GetIsActive()
    {
        return _isActive;
    }

}
