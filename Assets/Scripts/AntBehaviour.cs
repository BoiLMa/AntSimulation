using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class AntBehaviour : MonoBehaviour
{
    public int totalFood = 0;
    public GameObject ant;
    public Sprite antDefaultSprite;
    public Sprite antCarryFood;
    public List<Ant> antList = new List<Ant>();
    List<Moth> mothList = new List<Moth>();
    Vector3 borderMax;
    Vector3 borderMin;
    // Start is called before the first frame update
    void Start()
    {
        borderMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        borderMin = Camera.main.ScreenToWorldPoint(Vector2.zero);
        for (int i = 0; i < 500; i++)
        {
            antList.Add(new Ant(Instantiate(ant))); 
        }
        mothList = GetComponent<MothBehaviour>().mothList;
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach(Ant ant in antList)
        {
            //rotate towards (very good)
            Vector2 direction = ant.destination - ant.position;
            ant.antGameObject.transform.up = -direction;
            //move
            ant.antGameObject.transform.position += (ant.destination - ant.position).normalized * ant.speed * Time.deltaTime;
            switch (ant.state)
            {
                case States.Wandering:
                    ant.antGameObject.GetComponent<SpriteRenderer>().sprite = antDefaultSprite;
                    if ((ant.position - ant.destination).magnitude < 0.05f && !ant.hasFood)
                    {
                        ant.destination = new Vector3(Random.Range(ant.position.x - ant.radius, ant.position.x + ant.radius), Random.Range(ant.position.y - ant.radius, ant.position.y + ant.radius));
                        ant.pathFromHome.Add(ant.position);
                    }
                    while (ant.destination.x < borderMin.x || ant.destination.x > borderMax.x || ant.destination.y < borderMin.y || ant.destination.y > borderMax.y)
                        ant.destination = new Vector3(Random.Range(ant.position.x - ant.radius, ant.position.x + ant.radius), Random.Range(ant.position.y - ant.radius, ant.position.y + ant.radius));
                    //searching for food
                    float closestFood = 99;
                    Moth closestFoodGameObject = null;
                    foreach (Moth moth in mothList)
                    {
                        if ((moth.position - ant.position).magnitude < closestFood && moth.isDead)
                        {
                            closestFood = (moth.position - ant.position).magnitude;
                            closestFoodGameObject = moth;
                        }
                        if (closestFood < 0.5f)
                            ant.destination = closestFoodGameObject.position;
                        if (closestFood < 0.2f)
                        {
                            ant.hasFood = true;
                            ant.pathToFood.Add(ant.position);
                            ant.length = ant.pathFromHome.Count;//the path he walked so far
                            if (!ant.getFood)
                            {
                                closestFoodGameObject.health--;
                                ant.getFood = true;
                            }
                            ant.state = States.Collecting;
                        }
                    }
                    break;
                case States.Collecting:
                    ant.atDestination = true;
                    ant.antGameObject.GetComponent<SpriteRenderer>().sprite = antCarryFood;
                    //GetToCloserPath(antList);
                    if ((ant.destination - ant.position).magnitude < 0.05f && ant.atDestination) //als de mier een pad tegen komt van een andere mier die minder punten heeft waar hij langs moet
                    {                                                                            //volgt hij die punten, uitzoeken hoe verschillende mieren elkaars variables kunnen lezen
                        ant.pathToFood.Add(ant.position);
                        if (ant.length > 0)
                        {
                            ant.length--;
                            ant.destination = ant.pathFromHome[ant.length];
                        }
                        ant.atDestination = false;
                    }
                    if ((transform.position - ant.position).magnitude < 0.05f)
                    {
                        ant.getFood = false;
                        ant.hasFood = false;
                        totalFood++;
                        ant.destination = transform.position;
                        ant.pathFromHome.Clear();
                        ant.state = States.Wandering;
                    }
                    break;
            }
        }                                                                                               
    }                                       //WERKT NIET/SLECHT    
    void GetToCloserPath(List<Ant> antList) //gaat door iedere list van iedere ant. En de ant als de transform.position van deze ant in de buurt is van de een van de posities in de pathFromHome lijst word er gekeken
    {                                       //vanaf waar in de lijst de positie is en vanaf daar afgeteld totdat de mier bij het kortste pad is aangekomen
        int positionIndex = 0;
        foreach(Ant ant in antList)
        {
            Ant thisAnt = ant;
            foreach (Vector3 position in ant.pathFromHome)
            {
                if ((thisAnt.position - ant.position).magnitude < 0.5f && thisAnt.hasFood && ant.pathFromHome.Contains(position))
                {
                    thisAnt.pathFromHome = ant.pathFromHome;
                }
                for (int i = 0; i < ant.pathFromHome.Count; i++)
                {
                    if ((position - ant.pathFromHome[i]).magnitude < 0.5f)
                        positionIndex = i;
                }
                //vanaf de i positie verder gaan naar de 0de positie in de lijst
                //als de mier een kortere path vind doe dit opnieuw

            }
            thisAnt.destination = thisAnt.pathFromHome[positionIndex];
        }
    }
}
public class Ant
{
    public List<Vector3> pathToFood = new List<Vector3>();
    public List<Vector3> pathFromHome = new List<Vector3>();
    public GameObject antGameObject;
    public bool getFood;
    public bool hasFood;
    public bool atDestination = true;
    public States state;
    public Vector3 destination = new Vector3(0,0,0);
    public float speed = 0.5f;
    public float radius = 1.5f;
    public int length;

    public Ant(GameObject _antGameObject)
    {
        antGameObject = _antGameObject;
    }
    public Vector3 position => antGameObject.transform.position;
}
public enum States
{
    Wandering,
    Collecting
}
public struct Positions
{
    Vector3 position;
    List<Vector3> parentList;

}
