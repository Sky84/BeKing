using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DragonBones;
using UnityEngine;
using UnityEngine.UI;
using static SpamGameController;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D hook;
    public float timeBeforeRelease;
    public float maxDistance;
    public GameObject nextProjectile;
    public Collider2D targetToWin;
    public ThrowGameController throwGameController;
    [HideInInspector] public SpamButtonType selectedBar = SpamButtonType.Food;
    [HideInInspector] public bool isReleased;
    [HideInInspector] public List<GameObject> throwBarButtons = new List<GameObject>();
    private bool isPressed;

    private void Start()
    {
        throwBarButtons = GameObject.FindGameObjectsWithTag("throwBarButtons").ToList();
    }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("win"))
        {
            GameManager.Instance.slowBonuses.Add(selectedBar);
            Destroy(this.gameObject);
            WinIfPossible();
        }
        else if (other.gameObject.CompareTag("river"))
        {
            Destroy(this.gameObject);
            WinIfPossible();
        }
        else if (other.gameObject.CompareTag("bird"))
        {
            var animationBird = other.gameObject.GetComponent<UnityArmatureComponent>().animation;
            animationBird.Play("Hurt", 1);
            animationBird.FadeIn("Flying", 0.2f);
            Destroy(this.gameObject);
            WinIfPossible();
        }
    }

    void WinIfPossible()
    {
        if (!nextProjectile)
        {
            GameManager.Instance.Win();
        }
    }

    IEnumerator ReleaseProjectile()
    {
        yield return new WaitForSeconds(timeBeforeRelease);
        GetComponent<SpringJoint2D>().enabled = false;
        isReleased = true;
        throwGameController.SendMessage("RespawnIfPossible", nextProjectile);
    }
}
