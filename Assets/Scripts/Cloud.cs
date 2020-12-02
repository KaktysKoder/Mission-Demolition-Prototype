using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private GameObject cloudSphere;

    public int numSpheresMin = 6;
    public int numSpheresMax = 10;

    public float scaleYMin = 2.0f;

    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    
    public Vector3 sphereScaleRangeX = new Vector3(4, 8);
    public Vector3 sphereScaleRangeY = new Vector3(3, 4);
    public Vector3 sphereScaleRangeZ = new Vector3(2, 4);

    private List<GameObject> spheres;

    private void Start()
    {
        #region bone::unnecessary code
        // Если вдруг произойдёт ситуация при которой объект cloudSphere потеряет
        // в инспекторе ссылку на префаб. Создаём кастыль, который найдёт невидимый объект cloudSphere
        // который всё это время находился на сцене как CloudSphere enabled = false, и вставим его в инспектор.
        // После чего спавним облока, и enabled = true Для всех дочерних объектов.

        //if (cloudSphere == null)
        //    cloudSphere = GameObject.FindWithTag("CloudSphere");
        #endregion

        Respawn();
    }

    private void Update()
    {
        #region Test::code
        if (Input.GetKeyDown(KeyCode.Space))
            Restart();

        if (Input.GetKeyDown(KeyCode.B))
            Respawn();
        #endregion
    }

    private void Restart()
    {
        foreach (var sphere in spheres)
        {
            Destroy(sphere);    // Удалить старые сферы, состовляющие облоко
        }
    }

    private void Respawn()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(numSpheresMin, numSpheresMax);

        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate(cloudSphere);
            spheres.Add(sp);

            Transform spTrans = sp.transform;
            spTrans.SetParent(transform);

            // Выбор случайного местоположения
            Vector3 offset = Random.insideUnitSphere;

            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;

            spTrans.localPosition = offset;

            // Выбор случайного масштаба
            Vector3 scale = Vector3.zero;

            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            // скорректировать масштаб у по расстоянию х от центра
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spTrans.localScale = scale;

            #region bone::unnecessary code
            // Кастыль
            // Активируем заспавненные объекты, у которых  по умолчанию было enabled = false/ 
            //GameObject refToTheCloud = GameObject.Find("Cloud");

            //foreach (Transform child in refToTheCloud.GetComponentInChildren<Transform>())
            //{
            //    child.GetComponentInChildren<MeshRenderer>().enabled = true;
            //}
            #endregion
        }
    }
}
