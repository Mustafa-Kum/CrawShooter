using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Transform scannerPoint;

    [Header("Scan Info")]
    [SerializeField] private float scanRange;
    [SerializeField] private float scanDuration;

    public delegate void OnScanDetectionUpdated(GameObject newDetection);
    public event OnScanDetectionUpdated onScanDetectionUpdated;

    internal void SetScanRange(float scanRange)
    {
        this.scanRange = scanRange;
    }

    internal void SetScanDuration(float duration)
    {
        this.scanDuration = duration;
    }

    internal void AddChildAttached(Transform newChild)
    {
        newChild.parent = scannerPoint;
        newChild.localPosition = Vector3.zero;
    }

    internal void StartScan()
    {
        scannerPoint.localScale = Vector3.zero;

        StartCoroutine(StartScanCoroutine());
    }

    IEnumerator StartScanCoroutine()
    {
        float scanGrowthRate = scanRange / scanDuration;
        float startTime = 0;

        while (startTime < scanDuration)
        {
            startTime += Time.deltaTime;
            scannerPoint.localScale += Vector3.one * scanGrowthRate * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        onScanDetectionUpdated?.Invoke(other.gameObject);
    }
}
