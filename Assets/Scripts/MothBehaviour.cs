using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MothBehaviour : MonoBehaviour
{
    public GameObject moth;
    public Sprite deadMoth;
    public float cooldown = 1.0f;
    float spawn = 0.0f;
    public List<Moth> mothList = new List<Moth>();
    int[] randomXPos = new int[2] { -10, 10 };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawn)
        {
            cooldown = Random.Range(60, 240);
            spawn = Time.time + cooldown;
            Vector3 position = new Vector3(randomXPos[Random.Range(0, 2)], Random.Range(-4, 4));
            Vector3 destination = new Vector3(Random.Range(-8, 8), position.y);
            GameObject mothInstance = Instantiate(moth, position, moth.transform.rotation);
            if (destination.x > position.x)
                mothInstance.GetComponent<SpriteRenderer>().flipX = true;
            else
                mothInstance.GetComponent<SpriteRenderer>().flipX = false;
            mothList.Add(new Moth(mothInstance, destination));
        }
        foreach (Moth moth in mothList.ToList())
        {
            if ((moth.destination - moth.position).magnitude < 0.5f && !moth.setPosition)
            {
                moth.mothGameObject.GetComponent<SpriteRenderer>().sprite = deadMoth;
                moth.isDead = true;
                moth.destination = new Vector3(moth.destination.x, moth.destination.y - 1);
                moth.setPosition = true;
            }
            moth.mothGameObject.transform.position += (moth.destination - moth.position).normalized * 1f * Time.deltaTime;
            if (moth.health == 0)
            {
                Destroy(moth.mothGameObject);
                mothList.Remove(moth);
            }    
        }
    }
}
public class Moth
{
    public float health = 100f;
    public GameObject mothGameObject;
    public Vector3 destination;
    public Vector3 deadDestination;
    public bool isDead = false;
    public bool setPosition = false;
    public Moth(GameObject _mothGameObject, Vector3 _destination)
    {
        mothGameObject = _mothGameObject;
        destination = _destination;
    }
    public Vector3 position => mothGameObject.transform.position;
}
