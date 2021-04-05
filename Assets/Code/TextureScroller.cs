using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private float offset = 0;
    private MeshRenderer render;
    private Material mat;
    private bool moving = false;

    void Start()
    {
        render = GetComponent<MeshRenderer>();
        mat = render.material;
    }

    void Update()
    {
        if (moving) {
            mat.mainTextureOffset = new Vector2(offset, 0);

            offset += speed * Time.deltaTime;
        }
    }

    public void setIsMoving(bool isMoving)
    {
        moving = isMoving;
    }
}
