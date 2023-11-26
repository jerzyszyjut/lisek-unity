using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneratedPlatforms : MonoBehaviour
{
    public int PLATFORMS_NUM = 3;
    public GameObject platformPrefab;
    public GameObject[] platforms;
    public Vector3[] positions;
    //zmienne do schodów xd
    public float spacing = 2.0f; // Spacing between platforms
    public float slope = -10.0f; // Slope of the linear function
    public float yOffset = -10.0f; // Y offset for adjusting the height
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            //skomplikowana formu³a matematyczna obliczona by Anna Berkowska and Mateusz Fydrych
            float x = i * spacing;
            float y = slope * x + yOffset;
            positions[i] = new Vector3(x, y, 0);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity); //tworzenie platformy
            MovingPlatformController movingPlatformController = platforms[i].GetComponent<MovingPlatformController>();
            if (movingPlatformController != null)
            {
                movingPlatformController.speed = Random.Range(1.5f, 3.0f);
                movingPlatformController.minOffset = Random.Range(2.0f, 4.0f);
                movingPlatformController.maxOffset = Random.Range(0.0f, 2.0f);
                movingPlatformController.movementAxis = MovementAxis.Y;
            }
        }
    }
}
