using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float maxX;
    public float minX;
    public float maxZ;
    public float minZ;
    public float maxFieldView;
    public float minFieldView;
    public float speed = 1.0f;
    public float mouseSpeed = 60.0f;
    public int ManualWidth = 1920;
    public int ManualHeight = 1080;

    public float XpositionFix;
    public float YpositionFix;
    public float ZpositionFix;

    [HideInInspector]
    public new Camera camera;
    public void scaleFieldOfView(float scale)
    {
        float x = camera.fieldOfView * scale;
        if (x > maxFieldView)
            camera.fieldOfView = maxFieldView;
        else if (x < minFieldView)
            camera.fieldOfView = minFieldView;
        else
            camera.fieldOfView = x;
    }
    public void moveCamera(float dx, float dz)
    {
        Vector3 newPosition = transform.position + new Vector3(dx, 0, dz);
        if (newPosition.x > maxX)
            newPosition.x = maxX;
        else if (newPosition.x < minX)
            newPosition.x = minX;
        if (newPosition.z > maxZ)
            newPosition.z = maxZ;
        else if (newPosition.z < minZ)
            newPosition.z = minZ;
        transform.position = newPosition;
    }
    void Start()
    {
        int manualHeight;
        if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
            manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        else
            manualHeight = ManualHeight;
        camera = GetComponent<Camera>();
        float scale = System.Convert.ToSingle(manualHeight / (float)ManualHeight);
        scaleFieldOfView(scale);
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouseSlip = Input.GetAxis("Mouse ScrollWheel");
        moveCamera(h * speed * Time.deltaTime, v * speed * Time.deltaTime);
        scaleFieldOfView(1 - mouseSlip * mouseSpeed * Time.deltaTime);
    }
    public void FixOnCharacter(Vector2 temp)
    {
        GameObject character = GameObject.FindGameObjectWithTag("Player");
        Vector3 fixPositon = new Vector3(character.transform.position.x + XpositionFix, character.transform.position.y + YpositionFix, character.transform.position.z + ZpositionFix);
        transform.position = fixPositon;
    }
}
