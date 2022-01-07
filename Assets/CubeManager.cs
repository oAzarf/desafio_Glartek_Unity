using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Cube;
    [SerializeField]
    private Transform unsortedPosition;
    [SerializeField]
    private Transform sortedPosition;
    [SerializeField]
    private Transform redPosition;
    [SerializeField]
    private Transform greenPosition;
    [SerializeField]
    private Transform bluePosition;
    static int cubesToCreate=64;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject unsortedHolder;
    [SerializeField]
    GameObject button;
    private float oldSpeed;
    List<CubeScript> unsortedCubes=new List<CubeScript>();
    private float scale;


    List<CubeScript> sortedCubes =  new List<CubeScript>();


    private void Start()
    {
        scale = Cube.transform.localScale.x;
        for (int i = 0; i < cubesToCreate; i++)
        {
            unsortedCubes.Add(CreateCubes(i, unsortedPosition));
            sortedCubes.Add(CreateCubes(i, sortedPosition, unsortedCubes[i].gameObject));

        }
    }
    public void ButtonCallStart()
    {

        button.SetActive(false);
        
        StartCoroutine(MoveToColum());
    }

    private CubeScript CreateCubes(int i,Transform createHere, GameObject newCube)
    {  
        var cube = Instantiate(newCube, createHere.position + Vector3.right * i * (scale * 1.5f), Quaternion.identity, createHere); 
        var cubeScript = cube.GetComponent<CubeScript>();        
        cubeScript.speed = speed;
        return cubeScript;
    }
    private CubeScript CreateCubes(int i, Transform createHere)
    {
        
        var cube = Instantiate(Cube, createHere.position + Vector3.right * i * (scale * 1.5f), Quaternion.identity, createHere);
        var cubeScript = cube.GetComponent<CubeScript>();

        cubeScript.CreatingCubes();
        cubeScript.speed = speed;
        
        return cubeScript;
    }

    private void Update()
    {
        if (oldSpeed!=speed)
        {
            foreach (var item in unsortedCubes)
            {
                item.speed = speed;
            }
        } 
        oldSpeed = speed;
    }

    static int SortByColor(CubeScript p1, CubeScript p2)
    {
        return p1.myColor.CompareTo(p2.myColor);
    }



IEnumerator  SortNow()
    {
        yield return new WaitForSeconds(1f);
        sortedCubes.Sort(SortByColor);

        

        for (int i = 0; i < cubesToCreate; i++)
        {
            var go = sortedPosition.position + Vector3.right * i * (scale * 1.5f) ;
            StartCoroutine(sortedCubes[i].MoveToNewPosCoroutine(go));

        }
    }

    public IEnumerator  MoveToColum()
    {
        System.Predicate<CubeScript> isRed = s => s.myColor.Equals(CubeColor.Red);
        System.Predicate<CubeScript> isGreen = s => s.myColor.Equals(CubeColor.Green);
        System.Predicate<CubeScript> isBlue = s => s.myColor.Equals(CubeColor.Blue);
        var red = unsortedCubes.FindAll(isRed);
        var blue = unsortedCubes.FindAll(isBlue);
        var green = unsortedCubes.FindAll(isGreen);


        Vector3 goThere = Vector3.zero;
        int indexOnColorList =0;
        List<CubeScript> listToUse=new List<CubeScript>();

        while (unsortedCubes.Count!=0)
        {

            var randomCubeIndex = Random.Range(0, unsortedCubes.Count);
            var theRandomCube = unsortedCubes[randomCubeIndex];
            var theRandomColor = theRandomCube.myColor;


            switch (theRandomColor)
            {
                case CubeColor.Red:
                    indexOnColorList = red.IndexOf(theRandomCube);
                    goThere = redPosition.position + Vector3.right * indexOnColorList * (scale * 1.5f);
                    listToUse = red;
                    break;
                case CubeColor.Green:
                    indexOnColorList = green.IndexOf(theRandomCube);
                    goThere = greenPosition.position + Vector3.right * indexOnColorList * (scale * 1.5f);
                    listToUse = green;
                    break;
                case CubeColor.Blue:
                    indexOnColorList = blue.IndexOf(theRandomCube);
                    goThere = bluePosition.position + Vector3.right * indexOnColorList * (scale * 1.5f);
                    listToUse = blue;
                    break;
            }
            StartCoroutine(listToUse[indexOnColorList].MoveToNewPosCoroutine(goThere));
            unsortedCubes.Remove(theRandomCube);
            yield return new WaitForSeconds(0.1f);

        }

        unsortedHolder.SetActive(false);

        var coroutine =StartCoroutine(SortNow());
        
        
    }


}
