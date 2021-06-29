using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit3 : MonoBehaviour
{
    Player _player;
    private float flashTime = 0.1f;
    private MeshRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FlashRed()
    {

        _renderer.enabled = true;
        Invoke("ResetColor", flashTime);
    }
    void ResetColor()
    {
        _renderer.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        Boss _boss = gameObject.GetComponentInParent<Boss>();

        if (other.CompareTag("Laser") && _boss._isBossStage3Active == true)
        {
            _boss._hitCount++;
            FlashRed();
            Destroy(other.gameObject);
            _player.AddScore(Random.Range(10, 20));
            if (_boss._hitCount >= 10)
            {
                _boss.BossStage3Over();
            }

        }

        if (other.CompareTag("HomingMissile") && _boss._isBossStage3Active == true)
        {
            _boss._hitCount+=2;
            FlashRed();
            Destroy(other.gameObject);
            _player.AddScore(Random.Range(15, 30));
            if (_boss._hitCount >= 10)
            {
                _boss.BossStage3Over();
            }

        }

        if (other.CompareTag("Player") && _boss._isBossStage3Active == true)
        {

            _boss._hitCount++;
            FlashRed();
            _player.Damage();
            if (_boss._hitCount >= 10)
            {
                _boss.BossStage3Over();
            }

        }
        if (other.CompareTag("Shield") && _boss._isBossStage3Active == true)
        {

            _boss._hitCount++;
            FlashRed();
            _player.Damage();
            if (_boss._hitCount >= 10)
            {
                _boss.BossStage3Over();
            }

        }

    }
}
