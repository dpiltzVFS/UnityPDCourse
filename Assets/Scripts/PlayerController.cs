using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.TextCore.Text;
using Unity.PlasticSCM.Editor.WebApi;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float movementX;
    public float movementY;
    private int count;
    private Vector3 oldPosition;
    public float speed = 0.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public AudioClip loseSound;
    public AudioClip pickupSound;
    public GameObject explosionVFX;
    public GameObject pickupVFX;
    public GameObject bubbleBoy;
    private AudioSource source;
    Vector3 bubbleBoyOffset;
    public GameObject mainCamera;
    private Vector3 ballDirection;
    public float rotationSpeed = 0.5f;
    float yRotation = 0f;
    float xRotation = 0f;


    void Awake()
    {
        float sphereScale = transform.localScale.x;
        //
        bubbleBoyOffset = new Vector3(0, -sphereScale/2, 0);
    }


    void Start()
    {
        rb= GetComponent<Rigidbody>();
        count = 0;
        winTextObject.SetActive(false);
        source = GetComponent<AudioSource>();
        oldPosition = transform.position;

       
    }
    private void Update()
    {
        ballDirection = oldPosition - transform.position;
        //Debug.Log("ballDirection = " + ballDirection);
        oldPosition = transform.position;
        MoveBubbleBoy();
    }
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX,0.0f,movementY);
        rb.AddForce(movement * speed);
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
            var curentPickupVFX = Instantiate(pickupVFX, transform.position, Quaternion.identity);
            Destroy(curentPickupVFX, 3);

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
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

        }
    }

    private void MoveBubbleBoy()
    {
        

        if (movementX == 0f && movementY > 0) yRotation = 0f;
        else if (movementX > 0f && movementY > 0f) yRotation = 45f;
        else if (movementX > 0f && movementY == 0f) yRotation = 90f;
        else if (movementX > 0f && movementY < 0f) yRotation = 135f;
        else if (movementX == 0f && movementY < 0f) yRotation = 180f;
        else if (movementX < 0f && movementY < 0f) yRotation = 225f;
        else if (movementX < 0f && movementY == 0f) yRotation = 270f;
        else if (movementX < 0f && movementY > 0f) yRotation = 315f;

        Debug.Log("xRotation = " + xRotation + "yRotation = " + yRotation);

        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0f, yRotation, xRotation);

        //bubbleBoy.transform.SetPositionAndRotation(transform.position + bubbleBoyOffset, rotation);
        bubbleBoy.transform.position = transform.position + bubbleBoyOffset;
        bubbleBoy.transform.rotation = Quaternion.Lerp(bubbleBoy.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

}
