using System.Collections;
using UnityEngine;

public class BaseService : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitForServiceLocator());
    }

    private void OnDisable()
    {
        Unregister();
    }

    private IEnumerator WaitForServiceLocator()
    {
        while (ServiceLocator.Instance == null)
        {
            yield return null;
        }

        Register();
    }

    protected virtual void Register()
    {
        ServiceLocator.Instance.RegisterService(this.GetType(), this);
    }

    protected virtual void Unregister()
    {
        if (ServiceLocator.Instance != null)
            ServiceLocator.Instance.UnregisterService(this.GetType());
    }
}
