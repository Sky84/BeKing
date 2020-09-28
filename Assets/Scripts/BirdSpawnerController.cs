using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawnerController : MonoBehaviour
{
    public GameObject birdPrefab;

    public int speedBird;
    public float secondsBeforeSpawnABird;

    public int AmountBirdsLimit = 3;
    private Sprite sprite;
    private List<GameObject> birds = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(SpawnBirdAfter(secondsBeforeSpawnABird));
    }

    private void Update()
    {
        for (int i = 0; i < birds.Count; i++)
        {
            GameObject bird = birds[i];
            if (bird.transform.position.x < Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)).x)
            {
                bird.transform.position += new Vector3(speedBird, 0) * Time.deltaTime;
            }
            else
            {
                var randomY = Random.Range(sprite.bounds.min.y, sprite.bounds.max.y);
                bird.transform.position = new Vector3(transform.position.x, transform.position.y - randomY);
            }
        }
    }

    IEnumerator SpawnBirdAfter(float seconds)
    {
        var randomY = Random.Range(sprite.bounds.min.y, sprite.bounds.max.y);
        var position = new Vector3(transform.position.x, transform.position.y - randomY);


        yield return new WaitForSeconds(seconds);
        var bird = GameObject.Instantiate(birdPrefab, position, Quaternion.identity);
        bird.transform.SetParent(transform);
        birds.Add(bird);
        if (birds.Count < AmountBirdsLimit)
        {
            StartCoroutine(SpawnBirdAfter(seconds));
        }
    }
}
