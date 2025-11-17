using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Hero Prefabs")]
    [SerializeField] private CharacterBase warriorPrefab;
    [SerializeField] private CharacterBase magePrefab;
    [SerializeField] private CharacterBase archerPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;

    [Header("Names")]
    [SerializeField] private string[] greekNames = new[]
    {
        "Арес", "Зевс", "Гермес", "Аполлон", "Персей", "Ахилл", "Леонид", "Гектор"
    };

    [Header("UI")]
    [SerializeField] private VictoryUI victoryUI;

    private CharacterBase _leftHero;
    private CharacterBase _rightHero;

    private void Start()
    {
        StartNewBattle();
    }

    public void StartNewBattle()
    {
        // скрываем экран победы
        if (victoryUI != null)
            victoryUI.Hide();

        // удаляем старых
        if (_leftHero != null) Destroy(_leftHero.gameObject);
        if (_rightHero != null) Destroy(_rightHero.gameObject);

        // создаём новых
        _leftHero  = SpawnRandomHero(leftSpawnPoint.position);
        _rightHero = SpawnRandomHero(rightSpawnPoint.position);

        // случайные разные имена
        AssignRandomNames(_leftHero, _rightHero);

        // Обновления имен
        RefreshNameUI(_leftHero);
        RefreshNameUI(_rightHero);
        
        // подписка на смерть
        _leftHero.OnDeath += OnHeroDeath;
        _rightHero.OnDeath += OnHeroDeath;

        // настраиваем бой
        var leftCombat  = _leftHero.GetComponent<CharacterCombat>();
        var rightCombat = _rightHero.GetComponent<CharacterCombat>();

        leftCombat.SetTarget(_rightHero);
        rightCombat.SetTarget(_leftHero);

        leftCombat.StartCombat();
        rightCombat.StartCombat();
    }

    
    
    private void RefreshNameUI(CharacterBase hero)
    {
        // ищем компонент CharacterNameUI на детях
        var nameUI = hero.GetComponentInChildren<CharacterNameUI>();
        if (nameUI != null)
        {
            nameUI.Refresh();
        }
    }
    
    private CharacterBase SpawnRandomHero(Vector3 position)
    {
        CharacterBase[] pool = { warriorPrefab, magePrefab, archerPrefab };
        int index = Random.Range(0, pool.Length);
        return Instantiate(pool[index], position, Quaternion.identity);
    }

    private void AssignRandomNames(CharacterBase hero1, CharacterBase hero2)
    {
        if (greekNames.Length < 2)
        {
            hero1.SetName("Герой 1");
            hero2.SetName("Герой 2");
            return;
        }

        int index1 = Random.Range(0, greekNames.Length);
        int index2;
        do
        {
            index2 = Random.Range(0, greekNames.Length);
        } while (index2 == index1);

        hero1.SetName(greekNames[index1]);
        hero2.SetName(greekNames[index2]);
    }


    private void OnHeroDeath(CharacterBase deadHero)
    {
        // отключаем бой
        var leftCombat  = _leftHero.GetComponent<CharacterCombat>();
        var rightCombat = _rightHero.GetComponent<CharacterCombat>();

        leftCombat.StopCombat();
        rightCombat.StopCombat();

        CharacterBase winner = deadHero == _leftHero ? _rightHero : _leftHero;

        if (victoryUI != null)
        {
            victoryUI.Show(winner.CharacterName);
        }
    }
}
