using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float _distanceBetweenLine;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private Text _coinsText;
    [SerializeField] private Score _scoreResult, _score;

    private Animator _animator;
    private CapsuleCollider _collider;
    private CharacterController _controller;
    public int _coins;
    private Vector3 _direction;
    private LineToMove _lineToMove;
    private ParticleSystem _particleSystem;

    private bool _isSliding;
    private bool _isImmortal;

    private const float _maxSpeed = 110;

    private void Start()
    {
        Time.timeScale = 1;
        _losePanel.SetActive(false);
        _controller = GetComponent<CharacterController>();
        _collider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _coins = PlayerPrefs.GetInt("coins");
        _coinsText.text = _coins.ToString();
        _lineToMove = LineToMove.Center;
        StartCoroutine(Acceleration());
        _isImmortal = false;
        _particleSystem.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (_lineToMove != LineToMove.Right)
                _lineToMove++;
        }

        if (SwipeController.swipeLeft)
        {
            if (_lineToMove != LineToMove.Left)
                _lineToMove--;
        }

        if (SwipeController.swipeUp)
        {
            if (_controller.isGrounded)
                Jump();
        }

        if (SwipeController.swipeDown)
        {
            StartCoroutine(Slide());
        }

        if(_controller.isGrounded && !_isSliding)
        {
            _animator.SetBool("Running", true);
        }
        else
        {
            _animator.SetBool("Running", false);
        }

        Vector3 targetPosition = _player.position.z * _player.forward + _player.position.y * _player.up;
        if (_lineToMove == LineToMove.Left)
            targetPosition += Vector3.left * _distanceBetweenLine;
        else if (_lineToMove == LineToMove.Right)
            targetPosition += Vector3.right * _distanceBetweenLine;

        if (transform.position == targetPosition) return;

        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            _controller.Move(moveDir);
        else
            _controller.Move(diff);
    }

    private void FixedUpdate()
    {
        _direction.z = _speed;
        _direction.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _direction.y = _jumpForce;
        _animator.SetTrigger("Jump");
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Obstacle")
        {
            if (_isImmortal)
            {
                Destroy(hit.gameObject);
            } else
            {
                _losePanel.SetActive(true);
                int lastRunScore = int.Parse(_scoreResult._scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                Time.timeScale = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Coin coin))
        {
            _coins++;
            PlayerPrefs.SetInt("coins", _coins);
            _coinsText.text = _coins.ToString();
            coin.gameObject.SetActive(false);

        }
        if(other.TryGetComponent(out BonusStar bonusStar))
        {
            StartCoroutine(StarBonus());
            bonusStar.gameObject.SetActive(false);
        }
        if(other.TryGetComponent(out BonusShield bonusShield))
        {
            bonusShield.gameObject.SetActive(false);
            StartCoroutine(ShieldBonus());
            
        }
    }

    private IEnumerator Acceleration()
    {
        yield return new WaitForSeconds(4);
        if (_speed < _maxSpeed)
        {
            _speed += 3;
            StartCoroutine(Acceleration());
        }
    }

    private IEnumerator Slide()
    {
        Vector3 startPositionCollider = _collider.center;
        float _startHeighCollider = _collider.height;

        _collider.center = new Vector3(0, 0.3f, 0);
        _collider.height = 0.6f;
        _isSliding = true;
        _animator.SetTrigger("Slide");

        yield return new WaitForSeconds(1);

        _collider.center = startPositionCollider;
        _collider.height = _startHeighCollider;
        _isSliding = false;
    }

    private IEnumerator StarBonus()
    {
        _score.scoreMultipier = 2;

        yield return new WaitForSeconds(5);

        _score.scoreMultipier = 1;
    }

    private IEnumerator ShieldBonus()
    {
        _isImmortal = true;
        _particleSystem.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        _isImmortal = false;
        _particleSystem.gameObject.SetActive(false);
    }

}

enum LineToMove
{
    Left = -1,
    Center = 0,
    Right = 1
}
