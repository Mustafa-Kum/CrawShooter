using UnityEngine;

public class Perception : MonoBehaviour
{

    void Start()
    {
        SenseComponent.RegisterPerception(this);
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        SenseComponent.UnRegisterPerception(this);
    }
}
