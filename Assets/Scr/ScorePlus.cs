using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlus : MonoBehaviour
{

    [SerializeField]
    private int _score = 0;
    private ArkanoidController _arkController;
    void Start()
    {
        _arkController = FindObjectOfType<ArkanoidController>();
    }

    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
            _arkController.plusScore(_score);
            int totalScore = _arkController.getScore();
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(_score, totalScore);
            Destroy(this.gameObject);
		}
            Destroy(this.gameObject , 6f);

	}
}
