using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed,gravityModifier,jumpPower,runSpeed  = 12f;
    public CharacterController charCon;
    public Transform camTrans;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump,canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;
    //public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public int currentGun;
    public Transform adsPoint, gunHolder;
    public Vector3 gunStartPos;
    public float adsSpeend = 2f;
    public ParticleSystem ps;
    private Vector3 moveInput;

    public GameObject muzzleFlash;


    public AudioSource footstepFast, footstepSlow;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGun--;
        SwitchGun();

        gunStartPos = gunHolder.localPosition;
        //activeGun = allGuns[currentGun];
        //activeGun.gameObject.SetActive(true);
        //UIController.instance.ammoText.text = "Ammo: " + activeGun.currentAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        ps.Stop();
        if (!UIController.instance.pauseScreen.activeInHierarchy)
        {
            //moveInput.x = Input.GetAxis("Horizontal")*moveSpeed*Time.deltaTime;
            //moveInput.z = Input.GetAxis("Vertical")*moveSpeed*Time.deltaTime;
            // store y velocity
            float yStore = moveInput.y;
            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");
            //moveInput = vertMove * moveSpeed * Time.deltaTime;
            moveInput = horiMove + vertMove;
            moveInput.Normalize();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveInput = moveInput * runSpeed;
            }
            else
            {
                moveInput = moveInput * moveSpeed;
            }

            moveInput = moveInput * moveSpeed;
            moveInput.y = yStore;
            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
            if (charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            }


            canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;
            if (canJump)
            {
                canDoubleJump = false;
            }
            //Handle Jumping
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                moveInput.y = jumpPower;
                canDoubleJump = true;
            }
            else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                moveInput.y = jumpPower;
                canDoubleJump = false;
            }
            charCon.Move(moveInput * Time.deltaTime);







            //charCon.Move(moveInput);

            //control camera rotation
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
            if (invertX)
            {
                mouseInput.x = -mouseInput.x;
            }
            if (invertY)
            {
                mouseInput.y = -mouseInput.y;
            }
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(mouseInput.y, 0f, 0f));

            //spray
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    ps.Play();
            //}
            //else
            //{
            //    ps.Stop();
            //}
            muzzleFlash.SetActive(false);

            // Handle shooting
            // single shots
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                    firePoint.LookAt(hit.point);
                }
                else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
                }
                //  Instantiate(bullet, firePoint.position, firePoint.rotation);
                FireShot();
                //ps.Play();


            }

            //repeating shots
            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                    //ps.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchGun();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraController.instance.ZoomIn(activeGun.zoomAmount);
            }


            if (Input.GetMouseButton(1))
            {
                gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeend * Time.deltaTime);
            }
            else
            {
                gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPos, adsSpeend * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(1))
            {
                CameraController.instance.ZoomOut();
            }
            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);
        }
    }

    internal void DamagePlayer(int damage)
    {
        throw new NotImplementedException();
    }

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;

            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;

            UIController.instance.ammoText.text = "Ammo: " + activeGun.currentAmmo;
            muzzleFlash.SetActive(true);
            if (currentGun == 2)
            {
                ps.Play();
            }
        }
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);
        currentGun++;
        if(currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }
        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);
        UIController.instance.ammoText.text = "Ammo: " + activeGun.currentAmmo;
        firePoint.position = activeGun.firepoint.position;
    }
}
