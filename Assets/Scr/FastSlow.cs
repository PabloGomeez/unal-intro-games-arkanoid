using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSlow : MonoBehaviour
{

    [SerializeField]
    private int _type = 0;
    private Ball _ball;
    void Start()
    {
        _ball = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
            if (_type==1)
            {
                _ball.setSpeed(1.2f);
            }else{
                _ball.setSpeed(0.7f);
            }
            Destroy(this.gameObject);
		}
            Destroy(this.gameObject , 6f);

	}
}
