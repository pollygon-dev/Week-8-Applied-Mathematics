using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    public static float focalLenth = 300f;

    public static float GetPerspective(float zDistance)
    {
        var focalSum = Mathf.Max(zDistance + focalLenth, 0f);
        return focalSum > 0 ? focalLenth / focalSum : 0;
    }
}