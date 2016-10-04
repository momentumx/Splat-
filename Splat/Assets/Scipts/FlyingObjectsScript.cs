using UnityEngine;

public class FlyingObjectsScript : MovingObjects {

    public AudioClip soundFX, hurt;
    public enum Type
    {
        delete,
        wrapping,
        turning
    }
    public Type myType;
    float myWidth;
	// Use this for initialization
	void Start () {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x = Random.Range(-20, Screen.width + 21);
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, transform.position.y, 0);
        if (myType == Type.wrapping)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(-15, 16), 0);
            dir = Random.Range(.1f, .2f);
        }
        if (Random.Range(0, 2) == 0)
            dir *= -1;
        myWidth = GetComponent<SpriteRenderer>().bounds.extents.x * transform.lossyScale.x;
        transform.localScale = new Vector3(transform.localScale.x * Mathf.Sign(dir), transform.localScale.y, transform.localScale.z);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float width = Screen.width * .5f;
        if (Mathf.Abs(Camera.main.WorldToScreenPoint(transform.position).x - width)-width > 21)
        {
            switch (myType)
            {
                case Type.wrapping:
                    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                    pos.x = width - (width + 20) * Mathf.Sign(dir);
                    transform.position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, transform.position.y, transform.position.z);
                    break;
                case Type.turning:
                    dir *= -1;
                    transform.position = new Vector3(transform.position.x + dir, transform.position.y, transform.position.z);
                    transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
                    break;
                case Type.delete:
                    Destroy(gameObject);
                    break;
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        switch (myType)
        {
            case Type.wrapping:
                GameObject.Find("Balloon").GetComponent<AudioSource>().PlayOneShot(soundFX);
                GetComponent<Animator>().SetTrigger("Collided");
                break;
            case Type.turning:
                dir = 0;
                if (Mathf.Abs(transform.position.x - _other.transform.position.x) > myWidth + 5)
                {
                    GameObject.Find("Balloon").GetComponent<AudioSource>().PlayOneShot(hurt);
                    GetComponent<Animator>().SetTrigger("Hurt");
                }
                else {
                    
                    GameObject.Find("Balloon").GetComponent<AudioSource>().PlayOneShot(soundFX);
                    GetComponent<Animator>().SetTrigger("Collided");
                }
                break;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
