using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject cloudPrefabs = null;                                       // Шаблон для облаков.

    public int numClouds = 40;                                                   // Число облаков.

    public float cloudScaleMin = 1.0f;                                           // Мин. масштаб каждого облака.
    public float cloudScaleMax = 3.0f;                                           // Макс. масштаб каждого облака.
    public float cloudSpeedMult = 0.5f;                                          // Коэффицент скорости облаков.

    public Vector3 cloudPosMin = new Vector3(-50, 5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);

    private GameObject[] cloudInstances = null;

    private void Awake()
    {
        cloudInstances = new GameObject[numClouds];                              // Создать массив для хранения всех экземпляров облаков.

        GameObject anchor = GameObject.Find("CloudAnchor");                      // Найти родительский игровой объект CloudAnchor.

        GameObject cloud;                                                        // Создать в цикле заданное кол-во облаков.

        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate(cloudPrefabs);                                   // Создать экземпляр cloudPrefabs.

            Vector3 cloudPosition = Vector3.zero;                                // Выбрать местоположение для облока.

            cloudPosition.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cloudPosition.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
                                                                                 // Масштаб облака
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            cloudPosition.z = 100 - 90 * scaleU;                                 // Меньшие облока должны быть дальше.
                                                                                 // Применить полученные знания координат и масштаб к облаку.
            cloud.transform.position = cloudPosition;
            cloud.transform.localScale = Vector3.one * scaleVal;
            cloud.transform.SetParent(anchor.transform);                         // Сделать облако дочерним по отношению к ancher.

            cloudInstances[i] = cloud;                                           // Добавить облако в массив cloudInstances.
        }
    }

    private void Update()
    {
        foreach (GameObject cloud in cloudInstances)                             // Обойти в цикле все созданные облака.
        {                                                                       
            float scaleVal = cloud.transform.localScale.x;                       // Получить масштаб и координаты облака.
                                                                                
            Vector3 cloudPosition = cloud.transform.position;                   
                                                                                
            cloudPosition.x -= scaleVal * Time.deltaTime * cloudSpeedMult;       // Увеличить скорость для ближайших облаков.
                                                                                
            if (cloudPosition.x <= cloudPosMin.x)                                // Если облако сместилось слишком далеко в лево ...
            {                                                                   
                cloudPosition.x = cloudPosMax.x;                                 // Переместить его далеко в право.
            }                                                                   
                                                                                
            cloud.transform.position = cloudPosition;                            // Применить новые координаты к облаку.
        }
    }
}