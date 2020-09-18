using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float timeBeforeRelease;
    private bool isPressed;

    private void Update()
    {
        if (isPressed)
        {
            rb.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void OnMouseDown()
    {
        rb.isKinematic = true;
        isPressed = true;
    }

    void OnMouseUp()
    {
        rb.isKinematic = false;
        isPressed = false;
        StartCoroutine(ReleaseProjectile());
    }

    IEnumerator ReleaseProjectile()
    {
        yield return new WaitForSeconds(timeBeforeRelease);
        GetComponent<SpringJoint2D>().enabled = false;
    }
}
