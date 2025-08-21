using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator _instance;

    public static ServiceLocator Instance { get { return _instance; } }

    private readonly Dictionary<System.Type, BaseService> _registeredServices = new();

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterService(System.Type serviceType, BaseService service)
    {
        if (_registeredServices.ContainsKey(serviceType))
            return;

        _registeredServices.Add(serviceType, service);
    }

    public void UnregisterService(System.Type serviceType)
    {
        if (!_registeredServices.ContainsKey(serviceType))
            return;

        _registeredServices.Remove(serviceType);
    }

    public T AccessService<T>() where T : class
    {
        if (!_registeredServices.ContainsKey(typeof(T)))
            return default;

        return _registeredServices[typeof(T)] as T;
    }
}
