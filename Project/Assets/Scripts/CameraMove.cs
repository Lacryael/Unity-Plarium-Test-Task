using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float mainSpeed = 4.0f; 
    float shiftAdd = 10.0f; 
    float maxShift = 15.0f; 
    private float totalRun = 1.0f;

    void Update()
    {
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        { 
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }
            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            transform.Translate(p);
        }
    }

    private Vector3 GetBaseInput()
    { 
        Vector3 pVelocity = new Vector3();

        if (Input.GetKey(KeyCode.A) && transform.position.x > 4.5)
        {
            pVelocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < 28)
        {
            pVelocity += new Vector3(1, 0, 0);
        }
        return pVelocity;
    }
}
