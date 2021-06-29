using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed =8f;
    public bool _isEnemyLaser = false;
    public bool _isBackwards = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false || _isBackwards ==true)
        {
            MoveUp();
        }
        
        else MoveDown();

        if(transform.position.x>15 || transform.position.x < -15 || transform.position.y>20 || transform.position.y<-20)
        {
            Destroy(gameObject);
        }

     

    }
    void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        // if object has parent, destroy parent
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // if object has parent, destroy parent
        if (transform.position.y < -8f)
        {
            if (transform.parent !=null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }


   public void EnemyLaserBackwards()
    {
        _isBackwards = true;
    }

    public void TurnOffEnemyLaserBackwards()
    {
        _isBackwards = false;
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {    
                player.Damage();
                this.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(gameObject);
            }
        }
        if (other.tag == "Shield" && _isEnemyLaser == true)
        {
            Player player = other.GetComponentInParent<Player>();
            player.Damage();
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
