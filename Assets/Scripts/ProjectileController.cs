using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D hook;
    public float timeBeforeRelease;
    public float timeBeforeSpawn;
    public float maxDistance;
    public GameObject nextProjectile;
    [HideInInspector] public bool isReleased;
    private bool isPressed;

    private void Update()
    {
        if (isPressed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector3.Distance(mousePos, hook.position) > maxDistance)
            {
                rb.position = hook.position + (mousePos - hook.position).normalized * maxDistance;
            }
            else
            {
                rb.position = mousePos;
            }
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
        isReleased = true;
        yield return new WaitForSeconds(timeBeforeSpawn);
        if (nextProjectile)
        {
            nextProjectile.SetActive(true);
        }
    }
}
