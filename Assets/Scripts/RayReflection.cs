using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class RayReflection : MonoBehaviour
{
    private Transform incTransform;
    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 refDirection;
    private bool isHit;
    private string objectHit = "none";
    public float raySize = 7;
    private Vector3 defOrigin;
    private Vector3 defDirection;
    private Vector3 rayOrigin;
    private Vector3 rayDirection;
    public static int totRays = 0;
    public static int refRays = 0;
    public static int absRays = 0;
    private float e1;
    private float e2;
    private float theta;
    private float beta;
    private float thetaMax = Mathf.PI / 12.0f;
    private float mag;
    private Vector3 rayHitPoint;
    private Vector3 centrePoint;
    private float positionAngle;
    private Vector3 v1;
    private Vector3 v2;
    private int[] count;
    private Vector3 sourceDirection;
    private Vector3 newDir;
    private float rotAngleX;
    private float rotAngleY;
    private float rotAngleZ;
    private Vector3 normalVect;
    private Vector3 tempVectX;
    private Vector3 tempVectY;

    void Awake()
    {
        incTransform = this.GetComponent<Transform>();
        lineRenderer = this.GetComponent<LineRenderer>();
        count = new int[20];
        sourceDirection = -incTransform.up;
        normalVect = new Vector3(0, 0, 1);
    }

    void Update()
    {
        genRay();
        passRay();
    }

    string passRay()
    {
        RayReflection.totRays++;

        for (int i = 0; i <= 1; i++)
        {
            if (i == 0)
            {
                if (isHit = Physics.Raycast(ray.origin, ray.direction, out hit, raySize))
                {
                    if (hit.transform.tag == "Reflector")
                    {
                        RayReflection.refRays++;
                        refDirection = Vector3.Reflect(ray.direction, hit.normal);
                        ray = new Ray(hit.point, refDirection);

                        Debug.DrawRay(hit.point, hit.normal * 1, Color.blue);
                        Debug.DrawRay(hit.point, refDirection * raySize, Color.magenta);

                        if (isHit)
                        {
                            lineRenderer.SetVertexCount(2);           
                        }

                        lineRenderer.SetPosition(1, hit.point);
                    }
                    else if (hit.transform.tag == "Absorber")
                    {
                        RayReflection.absRays++;
                        objectHit = "dAbsorber";
                        break;
                    }
                }
            }
            else 
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 5))
                {
                    if (hit.transform.tag == "Absorber")
                    {
                        RayReflection.absRays++;
                        rayHitPoint = hit.point;
                        calcPositionAngle();
                        objectHit = "indAbsorber";
                    }
                }
            }
        }

        //setSourceDirection();
        return objectHit;
    }

    void genRay()
    {
        defOrigin = incTransform.position;
        defDirection = -incTransform.up;

        setIncRayOrigin();
        //rayOrigin = defOrigin;
        rayDirection = defDirection;

        ray = new Ray(rayOrigin, rayDirection);
        Debug.DrawRay(rayOrigin, rayDirection * raySize, Color.magenta);

        lineRenderer.SetVertexCount(1);
        lineRenderer.SetPosition(0, rayOrigin);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Total Rays: " + RayReflection.totRays.ToString());
        GUI.Label(new Rect(10, 30, 200, 20), "Reflected Rays: " + RayReflection.refRays.ToString());
        GUI.Label(new Rect(10, 50, 200, 20), "Absorbed Rays: " + RayReflection.absRays.ToString());
    }

    void setIncRayOrigin()
    {
        rayOrigin.x = Random.Range(-0.75f, 0.75f);
        rayOrigin.y = Random.Range(-0.5f, 0.5f);
        rayOrigin.z = incTransform.position.z;
    }

    void setSourceDirection()
    {
        e1 = Random.Range(0, 1.0f);
        e2 = Random.Range(0, 1.0f);

        theta = Mathf.Atan((Mathf.Sqrt(e1) * Mathf.Tan(thetaMax))*Mathf.Rad2Deg);
        beta = 2*Mathf.PI*e2;

        newDir.x = Mathf.Sin(theta) * Mathf.Cos(beta);
        newDir.y = -Mathf.Cos(theta);
        newDir.z = Mathf.Sin(theta) * Mathf.Sin(beta);

        tempVectX = new Vector3(0, newDir.y, newDir.z);
        tempVectY = new Vector3(newDir.x, 0, newDir.z);

        rotAngleX = Vector3.Angle(tempVectX, normalVect);
        rotAngleY = Vector3.Angle(tempVectY, normalVect);
        rotAngleZ = 0;

        //printCoordinates(newDir);
        //Debug.Log(rotAngleX + " " + rotAngleY + " " + rotAngleZ);
   
        incTransform.eulerAngles = new Vector3(rotAngleX+90, rotAngleY+180, rotAngleZ);
    }

    void calcPositionAngle()
    {
        centrePoint = new Vector3(rayHitPoint.x, 0, 1.222f);
        v1 = new Vector3(0, 0, 1);
        v2 = rayHitPoint - centrePoint;
        positionAngle = Vector3.Angle(v2, v1);
        countRays();
        printCounts();
    }

    void countRays()
    {
        int temp = 10; ;

        for (int i = 1; i <= 18; i++)
        {
            if (positionAngle <= temp)
            {
                count[i]++;
                break;
            }
            else
            {
                temp += 10;
            }
        }
    }

    void printCoordinates(Vector3 d)
    {
        Debug.Log("x: " + d.x + ", y: " + d.y + ", z: " + d.z);
    }

    void printCounts()
    {
        string c = " ";
        
        for (int i = 1; i <= 18; i++)
        { 
            c += count[i] + " ";
        }

        Debug.Log(c);
    }
}