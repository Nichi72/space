using UnityEngine;
using System.Collections;

public class BarrelCtrl : MonoBehaviour {
    //폭발효과 파티클 연결 변수
    public GameObject expEffect;
    //무작위로 선택할 텍스처 배열
    public Texture[] textures;
    
    private Transform tr;
    
    //총알 맞은 횟수를 누적시킬 변수
    private int hitCount = 0;
    
    void Start (){
        tr = GetComponent<Transform>();
        
        int idx = Random.Range(0, textures.Length);
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[idx];
    }
    
    //충돌 시 발생하는 콜백 함수(CallBack Function)
    void OnCollisionEnter(Collision coll){
        if (coll.collider.tag == "BULLET"){
            //충돌한 총알 제거
            Destroy(coll.gameObject);
            
            //총알 맞은 횟수를 증가시키고 3회 이상이면 폭발 처리
            if (++hitCount >= 3){
                ExpBarrel();
            }
        }
    }
    
    //드럼통 폭발시킬 함수
    void ExpBarrel(){
        //폭발효과 파티클 생성
        Instantiate(expEffect, tr.position, Quaternion.identity);
        
        //지정한 원점을 중심으로 10.0f 반경 내에 들어와 있는 Collider 객체 추출
        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);
        
        //추출한 Collider 객체에 폭발력 전달
        foreach (Collider coll in colls){
            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            if (rbody != null){
                rbody.mass = 1.0f;
                rbody.AddExplosionForce(1000.0f, tr.position, 10.0f, 300.0f);
            }
        }
        
        //5초 후에 드럼통 제거
        Destroy(gameObject, 5.0f);
    }
    
    //Raycast에 맞았을 때 호출할 함수
    void OnDamage(object[] _params)
    {
        // 발사 위치
        Vector3 firePos = (Vector3) _params[0];
        // 드럼통에 맞은 hit 위치
        Vector3 hitPos = (Vector3) _params[1];
        // 입사벡터(Ray의 각도)  = 맞은좌표 - 발사원점
        Vector3 incomeVector = hitPos - firePos;
        
        // 입사벡터를 정규화(Normalized) 벡터로 변경
        incomeVector = incomeVector.normalized;
        
        // Ray의 hit 좌표에 입사벡터의 각도로 힘을 생성
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1000f, hitPos);
        
        // 총알 맞은 횟수를 증가시키고 3회 이상이면 폭발 처리
        if (++hitCount >= 3){
            ExpBarrel();
        }
    }
}
