using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
   
    private Player _player;
    [SerializeField]
    private float _speed = 4f;
    private Transform _target;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float _angleChangingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else return;
        
        StartCoroutine(TimeToDie());

        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        if (FindClosestEnemy() != null)
        {
            _target = FindClosestEnemy().transform;
            Vector2 dir = (Vector2)_target.position - rb.position;
            dir.Normalize();
            float rotateAmount = Vector3.Cross(dir, transform.up).z;
            rb.angularVelocity = -_angleChangingSpeed * rotateAmount;
            rb.velocity = transform.up * _speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }

    IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(this.gameObject);
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector2 position = transform.position;

        foreach (GameObject go in enemies)
        {
            Vector2 diff = (Vector2)go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)

            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

}
