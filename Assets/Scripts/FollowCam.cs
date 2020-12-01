using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static GameObject POI;   // Ссылка на интересующий объект. // point of interest

    [Header("Set Dynamically")]
    public float cameraZ;           // Желаемая координата Z камеры.

    private float easing = 0.05f;   // ослабление
    public Vector2 minXY = Vector2.zero;

    private void Awake()
    {
        cameraZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null) return; // Выйти, если нет интересующего объекта.

        // Получить позицию интересующего объекта.
        Vector3 destination = POI.transform.position;

        // ограничить X и Y минимальными знаниями
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        // Определить точку между текущим местоположением камеры и destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        // Принудительно установить значение destination.z равным camZ, чтобы
        // отодвинуть камеру подальше
        destination.z = cameraZ;

        // Поместить камеру в позицию destination
        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }
}