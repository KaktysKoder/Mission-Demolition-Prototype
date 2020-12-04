using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MissionDemolition будет играть роль диспетчера состояний игры.
/// </summary>
public class MissionDemolition : MonoBehaviour
{
    private static MissionDemolition S;

    [Header("Set in Inspector")]
    public Text uitLevel;                       // Ссылка на объект UIText_Level
    public Text uitShots;                       // Ссылка на объект UIText_Shots
    public Text uitButton;                      // Ссылка на дочерний объект Text в UIButton_View

    public Vector3 castlePos;                   // Местоположение замка
    public GameObject[] castles;                // Массив замков

    [Header("Set Dynamically")]
    public int level;                           // Текущий уровень
    public int levelMax;                        // Количество уровней
    public int shotsTaken;

    // TODO: rename in currentCastle
    public GameObject castle;                   // Текущий замок 
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";   // Режим FollowCam

    private void Start()
    {
        S = this;

        level = 0;
        level = castles.Length;

        StartLevel();
    }

    private void Update()
    {
        UpdateGUI();

        if ((mode == GameMode.playing) && Goal.IsGoalMet)   // Проверить завершение уровня
        {
            mode = GameMode.levelEnd;                       // Изменить режим; чтобы прекратить проверку завершения уровня.

            SwitchView("Show Both");                        // Умениьшить масштаб.

            Invoke("NextLevel", 2.0f);                         // Начать новый уровень через 2 секнды.
        }
    }

    /// <summary>
    /// Переключить вид.
    /// </summary>
    /// <param name="eView"></param>
    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }

        showing = eView;

        switch (showing)
        {
            case "Show Slingshot":
                {
                    FollowCam.POI = null;
                    uitButton.text = "Show Castle";
                }
                break;

            case "Castle":
                {
                    FollowCam.POI = S.castle;
                    uitButton.text = "Show Both";
                }
                break;

            case "Show Both":
                {
                    FollowCam.POI = GameObject.Find("ViewBoth");
                    uitButton.text = "Show Castle";
                }
                break;

            default:
                {
                    Debug.LogError($@"Error in swith (Code line ~91)." +
                                   $" Script type: {typeof(MissionDemolition)}," +
                                   $" Script name: {nameof(MissionDemolition)}");
                }
                break;
        }
    }

    /// <summary>
    /// Произведены выстрелы.
    /// Вызывается сценарием Slingshot, чтобы уведомить MissionDemolition о произведенном выстреле.
    /// </summary>
    public static void ShotsFired() => S.shotsTaken++;

    /// <summary>
    /// Запуск уровня.
    /// </summary>
    private void StartLevel()
    {
        if (castle != null)                                                 // Уничтожить прежний замок, если он существует
        {
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile"); // Уничтожить прежние снаряды, если они существуют

        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        castle = Instantiate(castles[level]);                               // Создать новый замок
        castle.transform.position = castlePos;

        shotsTaken = 0;

        SwitchView("Show Both");                                            // Переустановить камеру в начальную позицию

        ProjectileLine.S.Clear();

        Goal.IsGoalMet = false;                                             // Сбросить цель

        UpdateGUI();

        mode = GameMode.playing;
    }

    /// <summary>
    /// Обновление интерфейса.
    /// </summary>
    private void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;        // Показать данные в элементах UI
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    /// <summary>
    /// Переход на следующий уровень.
    /// </summary>
    private void NextLevel()
    {
        level++;

        if (level == levelMax)
        {
            level = 0;
        }

        StartLevel();
    }
}