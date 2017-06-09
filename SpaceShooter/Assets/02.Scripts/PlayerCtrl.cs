using UnityEngine;
using UnityEngine.UI;  //UI 항목에 접근하기 위해 반드시 추가
using System.Collections;


//클래스는 System.Serializable이라는 어트리뷰트(Attribute)를 명시해야 
//Inspector 뷰에 노출됨
[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
}

public class PlayerCtrl : MonoBehaviour {
    private float h = 0.0f;
    private float v = 0.0f;
    
    //자주 사용하는 컴포넌트는 반드시 변수에 할당한 후 사용
    private Transform tr;
    //이동 속도 변수 (public으로 선언되어 Inspector에 노출됨)
    public float moveSpeed = 10.0f;
    
    //회전 속도 변수
    public float rotSpeed = 100.0f;
    
    //인스펙터뷰에 표시할 애니메이션 클래스 변수
    public Anim anim;
    
    //아래에 있는 3D 모델의 Animation 컴포넌트에 접근하기 위한 변수
    public Animation _animation;
    
    //Player의 생명 변수
    public int hp = 100;
    //Player의 생명 초깃값
    private int initHp;
    //Player의 Health bar 이미지
    public Image imgHpbar;
    
    //게임매니저에 접근하기 위한 변수
    private GameMgr gameMgr;
        
    //델리게이트 및 이벤트 선언
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    
    void Start () {
        //생명 초깃값 설정
        initHp = hp;
        
        //스크립트 처음에 Transform 컴포넌트 할당
        tr = GetComponent<Transform>();
        //GameMgr 스크립트 할당
        gameMgr = GameObject.Find("GameManager").GetComponent<GameMgr>();
        
        
        //자신의 하위에 있는 Animation 컴포넌트를 찾아와 변수에 할당
        _animation = GetComponentInChildren<Animation>();
        
        //Animation 컴포넌트의 애니메이션 클립을 지정하고 실행
        _animation.clip = anim.idle;
        _animation.Play();
    }
    
    void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        
        //Debug.Log("H = " + h.ToString());
        //Debug.Log("V = " + v.ToString());
        
        //전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        
        //Translate(이동 방향 * Time.deltaTime * 변위값 * 속도, 기준좌표)
        tr.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
        
        //Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));
        
        //키보드 입력값을 기준으로 동작할 애니메이션 수행
        if (v >= 0.1f){
            //전진 애니메이션
            _animation.CrossFade(anim.runForward.name, 0.3f);
        }
        else if (v <= -0.1f){
            //후진 애니메이션
            _animation.CrossFade(anim.runBackward.name, 0.3f);
        }
        else if (h >= 0.1f){
            //오른쪽 이동 애니메이션
            _animation.CrossFade(anim.runRight.name, 0.3f);
        }
        else if (h <= -0.1f){
            //왼쪽 이동 애니메이션
            _animation.CrossFade(anim.runLeft.name, 0.3f);
        }
        else {
            //정지시 idle애니메이션
            _animation.CrossFade(anim.idle.name, 0.3f);
        }
    }
    
    //충돌한 Collider의 IsTrigger 옵션이 체크됐을 때 발생
    void OnTriggerEnter(Collider coll)
    {
        //충돌한 Collider가 몬스터의 PUNCH이면 Player의 HP 차감
        if (coll.gameObject.tag == "PUNCH")
        {
            hp -= 10;

            //Image UI 항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
            imgHpbar.fillAmount = (float) hp / (float) initHp;
            
            Debug.Log("Player HP = " + hp.ToString());
            
            //Player의 생명이 0이하 이면 사망 처리
            if (hp <= 0)
            {
                PlayerDie();
            }
        }
    }
    
    //Player의 사망 처리 루틴
    void PlayerDie()
    {
        Debug.Log("Player Die !!");

//기존의 루틴은 주석처리 하거나 삭제
//        //MONSTER Tag를 가진 모든 게임오브젝트를 찾아옴
//        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
//        
//        //모든 몬스터의 OnPlayerDie 함수를 순차적으로 호출
//        foreach(GameObject monster in monsters)
//        {
//            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
//        }

        //이벤트 발생시킴
        OnPlayerDie();
        //게임매니저의 isGameOver 변수값을 변경해 몬스터 출현을 중지시킴
        //gameMgr.isGameOver = true;
        
        //GameMgr의 싱글톤 인스턴스를 접근해 isGameOver변수값을 변경
        GameMgr.instance.isGameOver = true;
       
    }    
}



