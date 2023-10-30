using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int count;

    public float speed = 0.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public AudioClip loseSound;
    public AudioClip pickupSound;
    private AudioSource source;


    void Start()
    {
        rb= GetComponent<Rigidbody>();
        count = 0;
        winTextObject.SetActive(false);
        source = GetComponent<AudioSource>();

    }
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX,0.0f,movementY);
        rb.AddForce(movement * speed);
        SetCountText();
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX= movementVector.x;
        movementY= movementVector.y;
    }
    private void SetCountText()
    {
        countText.text = "Count = " + count.ToString();
        if (count > 11)
        {
            winTextObject.SetActive(true);
            if(loseSound != null)
                source.PlayOneShot(loseSound);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            if (pickupSound != null)    
                source.PlayOneShot(pickupSound);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object
            Destroy(gameObject);
            // Update the winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }

}
