using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeSmall : MonoBehaviour
{
    [SerializeField]
    private int _type = 0;

    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
            if (_type==1)
            {
                col.transform.localScale=new Vector3(col.transform.localScale.x * 1.3f,1f,1f);
            }else{
                col.transform.localScale=new Vector3(col.transform.localScale.x / 1.3f,1f,1f);
            }
            Destroy(this.gameObject);
		}
            Destroy(this.gameObject , 6f);

	}
}
