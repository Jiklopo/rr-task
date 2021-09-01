using UnityEngine;

public abstract class GenericMonoBehaviourFactory<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] T prefab;

    public virtual T GetInstance()
    {
        T instance = Instantiate(prefab);
        return instance;
    }

    public virtual T[] GetInstances(int amount)
    {
        T[] instances = new T[amount];
        for (int i = 0; i < amount; i++)
        {
            instances[i] = GetInstance();
        }
        return instances;
    }
}
