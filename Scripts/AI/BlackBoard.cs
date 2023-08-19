using System.Collections.Generic;

public class BlackBoard
{
    private Dictionary<string, object> blackboardData = new Dictionary<string, object>();

    public delegate void OnBlackboardValueChanged(string key, object value);
    public event OnBlackboardValueChanged onBlackboardValueChanged;

    public void SetOrAddData(string key, object value)
    {
        if (blackboardData.ContainsKey(key))
            blackboardData[key] = value;
        else
            blackboardData.Add(key, value);

        onBlackboardValueChanged?.Invoke(key, value);
    }

    // If there is a problem here ----- Return back.

    public bool GetBlackboardData<T>(string key, out T value)
    {
        value = default(T);

        if (blackboardData.TryGetValue(key, out var storedValue) && storedValue is T)
        {
            value = (T)storedValue;
            return true;
        }

        return false;
    }

    public void RemoveBlackboardData(string key)
    {
        if (blackboardData.ContainsKey(key))
        {
            blackboardData.Remove(key);
            onBlackboardValueChanged?.Invoke(key, null);
        }
    }

    public bool HasKey(string key)
    {
        return blackboardData.ContainsKey(key);
    }
}
