using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public bool inHand;
    public bool inInv;
    public GameObject ItemPos;
    public GameObject cam;
    public AudioSource snd;
    public Rigidbody rb;
    public int addXRotation = 0, addYRotation = 0, addZRotation = 0;
    private void Start()
    {
        cam = Camera.main.gameObject;
    }
    public void InHand()
    {
        inHand = true;
        inInv = false;
        gameObject.SetActive(true);
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    public void InInventory()
    {
        inHand = false;
        inInv = true;
        gameObject.SetActive(false);
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    public void Drop()
    {
        inHand = false;
        inInv = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
    private void LateUpdate()
    {
        if (inHand == true)
        {
            transform.position = ItemPos.transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, cam.transform.rotation * Quaternion.Euler(addZRotation,addYRotation,addXRotation), Time.deltaTime * 15);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(snd.clip, transform.position);
    }
}
