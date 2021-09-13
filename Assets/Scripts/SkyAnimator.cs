using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyAnimator : MonoBehaviour
{
    public List<Image> images;

    public float cycleDuration;
    private float timer;
    public Gradient range;
    public Color target;
    private bool ping;
    // Start is called before the first frame update
    void Start()
    {
        NewTarget();
    }



    void NewTarget()
    {
        timer = 0;
        target = range.Evaluate(Random.Range(0.01f, 0.49f) + (ping ? 0.5f : 0));
        ping = !ping;
    }
    // Update is called once per frame
    void Update()
    {
        if (timer > cycleDuration)
        {
            NewTarget();
        }
        else
        {
            foreach (Image i in images)
            {
                i.color = Color.Lerp(i.color, target, (timer / cycleDuration) * Time.deltaTime);
            }
        }
        timer += Time.deltaTime;
    }
}
