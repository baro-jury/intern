using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private float angleChange, addedForce, maxSpeed;

    private float movementX, movementY;

    private Rigidbody2D myBody;
    private SpriteRenderer sr;

    [SerializeField]
    private Sprite movingSprite, idleSprite;

    private Vector3 positionOnScreen;

    [SerializeField]
    private float warpMarginX, warpMarginY;

    [SerializeField]
    private GameObject bulletPrefab;

    private bool shootFlag = true;
    [SerializeField]
    private float shootCooldownTime;

    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        ChangeMovingSprite();
        ShootBullet();
        Debug.Log(-Mathf.Atan2(myBody.velocity.x, myBody.velocity.y) * Mathf.Rad2Deg);
    }

    void FixedUpdate()
    {
        Move();
        ScreenWarp();
    }

    void Rotate()
    {
        movementX = -Input.GetAxisRaw("Horizontal");

        //Vector3 tempRotation = transform.rotation.eulerAngles;
        //tempRotation.z += angleChange * movementX * Time.deltaTime;
        //transform.rotation = Quaternion.Euler(tempRotation);

        transform.Rotate(new Vector3(0, 0, angleChange * movementX * Time.deltaTime));
    }

    void Move()
    {
        movementY = Input.GetAxisRaw("Vertical");

        //transform.up tra ve vector can di chuyen dua theo goc
        Vector2 moveVector = transform.up * movementY * addedForce;

        if (myBody.velocity.magnitude <= maxSpeed)
            myBody.AddForce(moveVector, ForceMode2D.Force);
        positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
    }

    //Da OK (?)
    //Nhung co ve chi can teleport sang ben kia man hinh 1 cach don gian nhat co the, khong can chinh x, y theo tan hay gi :v
    //Van co the lam phuc tap hon: Ve 1 duong thang cat voi 1 canh khac cua camera (khong nhat thiet la canh doi dien) va teleport ra cho do
    void ScreenWarp()
    {
        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;
        float movingAngle = -Mathf.Atan2(myBody.velocity.x, myBody.velocity.y); //in radian

        if (positionOnScreen.x < -warpMarginX)
        {
            positionOnScreen.x = width + warpMarginX;

            positionOnScreen.y += width / Mathf.Tan(movingAngle);

            if (positionOnScreen.y < 0) positionOnScreen.y += height;
            if (positionOnScreen.y > height) positionOnScreen.y -= height;
        }
        if (positionOnScreen.x > width + warpMarginX)
        {
            positionOnScreen.x = -warpMarginX;

            positionOnScreen.y += width / Mathf.Tan(movingAngle);

            if (positionOnScreen.y < 0) positionOnScreen.y += height;
            if (positionOnScreen.y > height) positionOnScreen.y -= height;
        }
        if (positionOnScreen.y < -warpMarginY)
        {
            positionOnScreen.y = height + warpMarginY;

            positionOnScreen.x += height * Mathf.Tan(movingAngle);

            if (positionOnScreen.x < 0) positionOnScreen.x += width;
            if (positionOnScreen.x > width) positionOnScreen.x -= width;

        }
        if (positionOnScreen.y > height + warpMarginY)
        {
            positionOnScreen.y = -warpMarginY;

            positionOnScreen.x += height * Mathf.Tan(movingAngle);

            if (positionOnScreen.x < 0) positionOnScreen.x += width;
            if (positionOnScreen.x > width) positionOnScreen.x -= width;
        }

        transform.position = Camera.main.ScreenToWorldPoint(positionOnScreen);
    }

    void ChangeMovingSprite()
    {
        if (Input.GetButton("Vertical"))
        {
            sr.sprite = movingSprite;
        }
        else sr.sprite = idleSprite;
    }

    void ShootBullet()
    {
        if (Input.GetButton("Fire1"))
        {
            if (shootFlag)
            {
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                shootFlag = false;
                StartCoroutine("ShootCooldown");
            }
        }
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldownTime);

        shootFlag = true;
    }
}
