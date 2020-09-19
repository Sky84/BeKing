using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ProjectileController projectile;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!projectile.isReleased)
        {
            lineRenderer.SetPosition(1, projectile.rb.position);
        }
        else if (projectile.nextProjectile)
        {
            projectile = projectile.nextProjectile.GetComponent<ProjectileController>();
        }
    }
}
