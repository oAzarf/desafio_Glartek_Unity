

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

public enum CubeColor {Red,Green,Blue }
public class CubeScript : MonoBehaviour
{
    public bool onTheCorrecttColum = false;
    public CubeColor myColor;
    Vector3 angles = Vector3.zero;
    public MeshRenderer meshRenderer;
    public Material myMat;
    public List<Color> cubesColors=new List<Color>();
    public float speed=.1f;
    // Start is called before the first frame update
    public void CreatingCubes()
    {
        myMat = meshRenderer.material;
        var colorSelect = Random.Range(0, cubesColors.Count);
        myMat.color = cubesColors[colorSelect];
        myColor = (CubeColor)colorSelect;
    }

    // Update is called once per frame
    void Update()
    {
        angles += new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f))*Time.deltaTime*100f;
        transform.eulerAngles = angles;
    }

    public IEnumerator MoveToNewPosCoroutine(Vector3 newPosition)
    {

        while (transform.position != newPosition)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
        }
    }
    public async Task MoveToNewPos(Vector3 newPosition)
    {

        while (transform.position != newPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
            await Task.Yield();
        }
        
    }
}
