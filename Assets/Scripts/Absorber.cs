using UnityEngine;
using System.Collections;

public class Absorber : MonoBehaviour
{
    public float ProjectileSpeed;
	
    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Amount to Move
        float amtToMove = ProjectileSpeed * Time.deltaTime;

        // Move Down
        transform.Translate(Vector3.left * amtToMove);
	}

    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.tag == "Aperture" || otherObject.tag == "Absorber")
        {
            Debug.Log("Collision!");
            transform.position = new Vector3(0, 1, 0);
        }
    }
}
