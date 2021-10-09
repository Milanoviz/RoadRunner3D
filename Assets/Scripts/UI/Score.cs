using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] public Text _scoreText;
    private int _totalScore;

    public int scoreMultipier = 1;

    private void Update()
    {
        _totalScore = (int)(_player.transform.position.z / 4);
        _scoreText.text = _totalScore.ToString();
    }

}
