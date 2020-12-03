using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    public static ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this;                                         // Установить ссылку на объект-одиночку

        line = GetComponent<LineRenderer>();              // Получить ссылку на LineRenderer
        line.enabled = false;                             // Выключить LineRenderer, пока он не понадобится

        points = new List<Vector3>();                     // Инициализировать список точек
    }

    private void FixedUpdate()
    {
        if (Poi == null)
        {
            if (FollowCam.POI != null)                   // Если свойство Poi содержит пустое значение, найти интересующий объект.
            {
                if (FollowCam.POI.CompareTag("Projectile"))
                {
                    Poi = FollowCam.POI;
                }
                else return;                             // Выйти, если интересующий объект не найден
            }
            else return;                                 // Выйти, если интересующий объект не найден
        }

        AddPoint();                                     // Если интересующий объект найден, попытаться добавить точку с его координатами в каждом FixedUpdate

        if (FollowCam.POI == null)
        {
            Poi = null;                                 // Если FollowCam.POI содержит null, записать nulll в poi
        }
    }

    public GameObject Poi
    {
        get => _poi;

        set
        {
            _poi = value;

            if (_poi != null)
            {
                line.enabled = false;                    // Если поле _poi содержит действительную ссылку, сбросить все остальные параметры в исходное состояние
                points = new List<Vector3>();

                AddPoint();
            }
        }
    }

    /// <summary>
    /// Возвращает местоположение последней добавленной точки.
    /// </summary>
    public Vector3 LastPoint
    {
        // Если точек нет, вернуть Vector3.zero
        get
        {
            if (points == null)
            {
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }

    /// <summary>
    /// Этот метод можно вызвать непосредственно, чтобы стереть линию
    /// </summary>
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    private void AddPoint()
    {
        Vector3 pt = _poi.transform.position;

        if (points.Count > 0 && (pt - LastPoint).magnitude < minDist) return; // Если точка недостаточно далека от предыдущей, просто выйти

        if (points.Count == 0)  // Если это точка запуска...
        {
            // Для определения
            // ...добавить дополнительный фрагмент линии,
            // чтобы помочь лучше прицелиться в будущем
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;

            // Установить первые 2 точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            // Включить LineRenderer
            line.enabled = true;
        }
        else
        {
            // Обычная последовательность добавления точки
            points.Add(pt);

            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, LastPoint);
            line.enabled = true;
        }
    }
}