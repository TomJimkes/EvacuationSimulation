using UnityEngine;

public class Fire : MonoBehaviour
{
    private float time;
    public int state = 1;
    public Sprite[] fireSprites;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 10)
        {
            state = 2;
            GetComponent<SpriteRenderer>().sprite = fireSprites[1];
        }
        if (time > 25 && state == 2)
        {
            state = 3;
            GetComponent<SpriteRenderer>().sprite = fireSprites[2];
        }
    }
}
