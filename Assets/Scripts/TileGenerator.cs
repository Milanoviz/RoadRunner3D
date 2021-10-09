using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _tilesTemplate;
    [SerializeField] private Transform _player;
    private float _spawnPosition = 0;
    private float _tileLenght = 100;
    private int _startTileCount = 6;

    private List<GameObject> _activeTiles = new List<GameObject>();

    public event UnityAction<float> SpawnNewPositionObstacle;
    


    private void Start()
    {

        for (int i = 0; i < _startTileCount; i++)
        {
            if (i == 0) 
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, _tilesTemplate.Length));
        }

        MapGenerator.instance.ResetMaps();
    }

    private void Update()
    {
        if (_player.position.z > _spawnPosition - (_startTileCount * _tileLenght))
        {
            SpawnTile(Random.Range(0, _tilesTemplate.Length));
            DeleteTile();
        }
    }

    private void SpawnTile(int tileIndex)
    {
        GameObject nextTile =  Instantiate(_tilesTemplate[tileIndex], transform.forward * _spawnPosition, Quaternion.identity, transform);
        _activeTiles.Add(nextTile);
        _spawnPosition += _tileLenght;
        SpawnNewPositionObstacle?.Invoke(_spawnPosition);
    }

    private void DeleteTile()
    {
        Destroy(_activeTiles[0], 5);
        _activeTiles.RemoveAt(0);
    }
}
