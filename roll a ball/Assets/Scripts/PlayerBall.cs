using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 가져오기 위해 꼭 필요!

public class PlayerBall : MonoBehaviour
{
    public float jumpPower;
    public int itemCount;
    public GameManagerLogic manager;
    bool isJump;
    AudioSource audio;
    Rigidbody rigid;

    void Awake()
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Vertical"); //Vertical
        float v = Input.GetAxisRaw("Horizontal"); //Horizontal

        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isJump = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            itemCount++;
            audio.Play();

            //gameObject : 자기자신
            other.gameObject.SetActive(false); //SetActive(bool) : 오브젝트 활성화 함수
            manager.GetItem(itemCount);
        }
        // Finish Point
        else if (other.tag == "Point")
        {
            //Scene을 불러오려면 꼭 Build Setting에서 추가!
            if (itemCount == manager.totalItemCount)
            {
                //Game Clear! && Next Stage
                if (manager.stage == 2)
                    SceneManager.LoadScene(0);
                else
                    SceneManager.LoadScene(manager.stage + 1);
            }
            else
            {
                //ReStart..
                //SceneManager : 장면을 관리하는 기본 클래스
                SceneManager.LoadScene(manager.stage); //LoadScene() : 주어진 장면을 불러오는 함수
            }
        }
    }
}
