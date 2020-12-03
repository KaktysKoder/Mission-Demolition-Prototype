using UnityEngine;

public class Goal : MonoBehaviour
{
    public static bool IsGoalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Projectile"))               // Когда в область действия триггера попадает что-то, проверить, является ли это "что-то” снарядом
        {
            IsGoalMet = true;                                       // Если это снаряд, присвоить полю goalMet значение true
            
            Material material = GetComponent<Renderer>().material;  // Также изменить альфа-канал цвета, чтобы увеличить непрозрачность

            Color swichColor = material.color;
            swichColor.a = 1;

            material.color = swichColor;
        }
    }
}