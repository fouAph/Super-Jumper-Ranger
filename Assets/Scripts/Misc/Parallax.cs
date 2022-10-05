using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float speed;
    [SerializeField] Transform[] cloudGFXParent;
    float startPos;

    private void Start()
    {
        startPos = transform.position.x;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < cloudGFXParent.Length; i++)
        {
            cloudGFXParent[i].Translate(new Vector3(-Time.deltaTime * speed, 0, 0));
            if (cloudGFXParent[i].position.x <= startPos * -1)
            {
                cloudGFXParent[i].position = new Vector3(startPos * 3, cloudGFXParent[i].position.y, cloudGFXParent[i].position.z);
            }
        }
    }
}
