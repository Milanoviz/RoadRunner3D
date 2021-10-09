using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    [Header("Obstacle")]
    private int _obstaleDistance = 20;
    [SerializeField] private int _obstacleCountInMap = 5;
    [SerializeField] private List<Obstacle> _obstacles = new List<Obstacle>();
    private float _lineoffset = 6;
    [Header("Coin")]
    [SerializeField] private Coin _coinTemplate;
    [SerializeField] private BonusShield _sieldTempate;
    [SerializeField] private int _coinsCountInItem;
    [SerializeField] private float _coinsHeight;
    [SerializeField] private float _sieldHeight;
    [SerializeField] private TileGenerator _tileGenerator;
    
    private float _mapSize;

    [SerializeField] private Transform _player;
    

    private List<GameObject> maps = new List<GameObject>();
    private List<GameObject> activeMaps = new List<GameObject>();

    static public MapGenerator instance;


    private void Awake()
    {
        instance = this;

        _mapSize = _obstaleDistance * _obstacleCountInMap;
        maps.Add(MakeMap1());
        maps.Add(MakeMap2());
        maps.Add(MakeMap3());
        foreach (var map in maps)
        {
            map.SetActive(false);
        }
    }

    private void Update()
    {
        if (activeMaps[0].transform.position.z < _player.position.z - 100)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }

        Debug.Log(activeMaps.Count);

    }

    public void ResetMaps()
    {
        while (activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }

        AddActiveMap();
        AddActiveMap();
        AddActiveMap();

    }

    private void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    public void AddActiveMap()
    {
        int randomIndex = 0;
        GameObject nextMap = maps[randomIndex];
        nextMap.SetActive(true);
        foreach (Transform child in nextMap.transform)
        {
            child.gameObject.SetActive(true);
        }
        nextMap.transform.position = activeMaps.Count > 0 ? activeMaps[activeMaps.Count -1].transform.position + Vector3.forward * _mapSize :
                                                            new Vector3(0, 0, 20);

        maps.RemoveAt(randomIndex);
        activeMaps.Add(nextMap);
    }

    private GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        bool createSield = false;
        MapItem item = new MapItem();
        for (int i = 0; i < _obstacleCountInMap; i++)
        {
            int randoIndexObstale = Random.Range(0, _obstacles.Count);
            int randomIndexPos = Random.Range(-1, 1);
            item.SetValue(null, TrackPos.Centre, CoinsStyle.Line);

            if (i == 1) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Left, CoinsStyle.Jump); }
            else if (i == 2) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Right, CoinsStyle.Jump); }
            else if (i == 3) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Centre, CoinsStyle.Jump); }
            else if (i == 4) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Right, CoinsStyle.Jump);createSield = true; }

            Vector3 obstaclePos = new Vector3((int)item.trackPos * _lineoffset, 2.6f, i * _obstaleDistance);
            CreateCoins(item.coinsStyle, obstaclePos, result);

            if (createSield)
            {
                CreateShields(item.coinsStyle, obstaclePos, result);
                createSield = false;
            }

            if (item.obstacle != null)
            {
                Obstacle newObstacle = Instantiate(item.obstacle, obstaclePos, item.obstacle.transform.rotation, result.transform);
            }
        }
        return result;
    }

    private GameObject MakeMap2()
    {
        GameObject result = new GameObject("Map2");
        result.transform.SetParent(transform);
        MapItem item = new MapItem();
        for (int i = 0; i < _obstacleCountInMap; i++)
        {
            int randoIndexObstale = Random.Range(0, _obstacles.Count);
            item.SetValue(null, TrackPos.Right, CoinsStyle.Line);

            if (i == 2) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Right, CoinsStyle.Jump); }
            else if (i == 3) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Left, CoinsStyle.Jump); }
            else if (i == 4) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Centre, CoinsStyle.Jump); }
            
            Vector3 obstaclePos = new Vector3((int)item.trackPos * _lineoffset, 2.6f, i * _obstaleDistance);
            CreateCoins(item.coinsStyle, obstaclePos, result);

            if (item.obstacle != null)
            {
                Obstacle newObstacle = Instantiate(item.obstacle, obstaclePos, item.obstacle.transform.rotation, result.transform);
            }
        }
        return result;
    }

    private GameObject MakeMap3()
    {
        GameObject result = new GameObject("Map3");
        result.transform.SetParent(transform);
        bool createSield = false;
        MapItem item = new MapItem();
        for (int i = 0; i < _obstacleCountInMap; i++)
        {
            int randoIndexObstale = Random.Range(0, _obstacles.Count);
            item.SetValue(null, TrackPos.Right, CoinsStyle.Line);

            if (i == 1) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Left, CoinsStyle.Jump); }
            else if (i == 2) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Centre, CoinsStyle.Jump); createSield = true; }
            else if (i == 3) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Right, CoinsStyle.Jump);  }
            else if (i == 4) { item.SetValue(_obstacles[randoIndexObstale], TrackPos.Centre, CoinsStyle.Jump); }

            Vector3 obstaclePos = new Vector3((int)item.trackPos * _lineoffset, 2.6f, i * _obstaleDistance);
            CreateCoins(item.coinsStyle, obstaclePos, result);

            if (createSield)
            {
                CreateShields(item.coinsStyle, obstaclePos, result);
                createSield = false;
            }


            if (item.obstacle != null)
            {
                Obstacle newObstacle = Instantiate(item.obstacle, obstaclePos, item.obstacle.transform.rotation, result.transform);
            }
        }
        return result;
    }


    private void CreateCoins(CoinsStyle style, Vector3 position, GameObject parentObject)
    {
        Vector3 coinPos = Vector3.zero;
        if (style == CoinsStyle.Line)
        {
            for (int i = -_coinsCountInItem / 2; i < _coinsCountInItem / 2; i++)
            {
                coinPos.y = _coinsHeight;
                coinPos.z = i * ((float)_obstaleDistance / _coinsCountInItem);
                Instantiate(_coinTemplate, coinPos + position, Quaternion.identity,parentObject.transform);
            }
        }
        else if (style == CoinsStyle.Jump)
        {
            for (int i = -_coinsCountInItem / 2; i < _coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 3, _coinsHeight);
                coinPos.z = i * ((float)_obstaleDistance / _coinsCountInItem);
                Instantiate(_coinTemplate, coinPos + position, Quaternion.identity, parentObject.transform);
                
            }
        }
    }

    private void CreateShields(CoinsStyle style, Vector3 position, GameObject parentObject)
    {
        Vector3 shieldPosition = Vector3.zero;
        if (style == CoinsStyle.Line)
        {
                shieldPosition.y = _sieldHeight;
                shieldPosition.z = ((float)_obstaleDistance / _coinsCountInItem);
                Instantiate(_sieldTempate, shieldPosition + position, Quaternion.identity, parentObject.transform);
            
        }
        else if (style == CoinsStyle.Jump)
        {
                shieldPosition.y = Mathf.Max(-1 / 2f * Mathf.Pow(1, 2) + 3, _sieldHeight);
                shieldPosition.z = ((float)_obstaleDistance / _coinsCountInItem);
                Instantiate(_sieldTempate, shieldPosition + position, Quaternion.identity, parentObject.transform);
        }
    }

}

struct MapItem
{
    public void SetValue(Obstacle obstacle, TrackPos trackPos, CoinsStyle coinsStyle)
    {
        this.obstacle = obstacle; this.trackPos = trackPos; this.coinsStyle = coinsStyle;
    }

    public Obstacle obstacle;
    public TrackPos trackPos;
    public CoinsStyle coinsStyle;
}

enum TrackPos
{
    Left = -1,
    Centre = 0,
    Right = 1
};
enum CoinsStyle
{
    Line,
    Jump
};
