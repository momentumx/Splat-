using UnityEngine;


public class ItemScript : MonoBehaviour {

    public float speed, magnitude;
    public AudioClip soundFX;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity += new Vector2(0, speed);
    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        GetComponent<Animator>().SetTrigger("Crash");
        GameObject balloon = GameObject.Find("Balloon");
        balloon.GetComponent<AudioSource>().PlayOneShot(soundFX);
        balloon.GetComponent<GameController>().ShakeCall(magnitude);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D> ().gravityScale = 0;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
