using UnityEngine;
using System.Collections;

public class DelegateTest : MonoBehaviour {

    //델리게이트에 연결할 함수의 원형정의
    delegate void CalNumDelegate (int num);
    
    //델리게이트 변수 선언    
    CalNumDelegate calNum;

	void Start () {
        //calNum 델리게이트 변수에 OnePlusNum 함수 연결
	    calNum = OnePlusNum;
        //함수 호출
        calNum(4);
        
        //callNum 델리게이트 변수에 PowerNum 함수 연결        
        calNum = PowerNum;
        calNum(5);
	}
	
    void OnePlusNum (int num)
    {
        int result = num + 1;
        Debug.Log ("One Plus = " + result);
    }
    
    void PowerNum (int num)
    {
        int result = num * num;
        Debug.Log ("Power = " + result);
    }
}
