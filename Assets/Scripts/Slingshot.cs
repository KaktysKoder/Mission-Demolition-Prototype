using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private GameObject prefabProjectile;
    [SerializeField] private float velocityMult = 8.0f;

    [Header("Set Dynamically")]
    [SerializeField] private GameObject launchPoint;
    [SerializeField] private GameObject projectile;         // это ссылка на вновь созданный экземпляр Projectile.

    public Vector3 launchPosition;                          // хранит трехмерные мировые координаты launchPoint.
    public bool aimingMode;

    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        Transform launchPointTransform = transform.Find("LaunchPoint"); // Находим игравой обхект LaunchPoint, сохраняем его в launchPoint, деактивируем.

        launchPoint = launchPointTransform.gameObject;
        launchPoint.SetActive(false);

        launchPosition = launchPointTransform.position;
    }

    private void Update()
    {
        if (!aimingMode) return;                                            // Если рогатка не в режиме прицеливания, не выполнять этот код

        Vector3 mousePosition2D = Input.mousePosition;                      // Получить текущие координаты указателя мыши

        mousePosition2D.z = -Camera.main.transform.position.z;

        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(mousePosition2D);

        Vector3 mouseDelta = mousePosition3D - launchPosition;              // Найти разность координат между launchPosition и mousePosition3D

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;    // Ограничить mouseDelta радиусом коллайдера объекта Slingshot

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projectilePosition = launchPosition + mouseDelta;            // Передвинуть снаряд в правую позицию

        projectile.transform.position = projectilePosition;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;                                               // Кнопка мыши отпущена

            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;

            FollowCam.POI = projectile;                                        // Слежка за шаром.

            projectile = null;
        }
    }

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        aimingMode = true;                                          // Игрок нажал кнопку мыши, когда указатель находился над рогаткой

        projectile = Instantiate(prefabProjectile);                 // Создать снаряд
        projectile.transform.position = launchPosition;             // Поместить в точку launchPoint
        projectile.GetComponent<Rigidbody>().isKinematic = true;    // Сделать его кинематическим

        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
}