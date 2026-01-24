using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graple : MonoBehaviour
{
    public GameObject hook;
    public GameObject pl;
    public Rigidbody rb;
    public SphereCollider coll;
    public AudioSource connect;
    public AudioSource throw_;
    public GameObject rope;
    private GameObject coll1;
    byte force = 17;
    public bool Hooked = false;
    bool en = false;
    public bool canThrow = true;
    public bool selected = true;
    float speed = 2;

    void Update()
    {
        if (selected == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (canThrow == true)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        hook.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.2f;
                        rb.isKinematic = false;
                        rb.velocity = Camera.main.transform.forward * force;
                        coll.enabled = true;
                        Hooked = false;
                        pl.GetComponent<Rigidbody>().useGravity = true;
                        pl.GetComponent<CharacterController>().canMove = true;
                        speed = 0.9f;
                        rope.transform.position = new Vector3(0, 100, 0);
                        rope.transform.localScale = new Vector3(0, 0, 0);
                        throw_.Play();
                        en = true;
                    }
                }
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                rb.isKinematic = true;
                hook.transform.position = new Vector3(0, 100, 0);
                pl.GetComponent<Rigidbody>().useGravity = true;
                pl.GetComponent<CharacterController>().canMove = true;
                Hooked = false;
                rope.transform.position = new Vector3(0, 100, 0);
                rope.transform.localScale = new Vector3(0, 0, 0);
                en = false;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (speed < 2.7f)
                {
                    speed += 0.3f;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (speed > 0.9f)
                {
                    speed -= 0.3f;
                }
            }
            if (Hooked == true)
            {
                hook.transform.forward = coll1.gameObject.transform.position;
                pl.GetComponent<Rigidbody>().AddForce((hook.transform.position - pl.transform.position).normalized * speed * Time.deltaTime * 4500f + Vector3.down * 60);
                pl.GetComponent<Rigidbody>().AddForce(pl.gameObject.transform.up * (-2));
            }
            if (en == true)
            {
                Vector3 startPos = hook.transform.position;
                Vector3 endPos = pl.transform.position;
                rope.transform.position = new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, startPos.z + endPos.z) / 2f;
                rope.transform.up = startPos - endPos;
                rope.transform.localScale = new Vector3(0.01f, (hook.transform.position - pl.transform.position).magnitude / 2, 0.01f);
                rope.GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(0.05f, startPos.y - endPos.y) * 4;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        connect.Play();
        if (collision.gameObject.tag == "Hookable")
        {
            rb.isKinematic = true;
            coll.enabled = false;
            coll1 = collision.collider.gameObject;
            Hooked = true;
            pl.GetComponent<Rigidbody>().useGravity = false;
            pl.GetComponent<CharacterController>().canMove = false;
        }
    }
}
