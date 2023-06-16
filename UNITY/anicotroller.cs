using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class anicotroller : MonoBehaviour
{
    public string portName = "COM1";  // 아두이노가 연결된 시리얼 포트 이름
    public int baudRate = 9600;      // 통신 속도 (baud rate)

    private SerialPort serialPort;
    private Animator anim;
    private AudioSource theAudio;

    [SerializeField] private AudioClip summer;  // 수정된 부분
    [SerializeField] private AudioClip winter;
    [SerializeField] private AudioClip alone;

    // Start is called before the first frame update
    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        anim = GetComponent<Animator>();
        theAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 'D' 키를 눌렀을 때 dance 동작과 노래 실행
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetFloat("dance2", 5.0f);
            PlayAudio("alone");
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetFloat("dance2", 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetFloat("hiphop2", 5.0f);
            PlayAudio("summer");
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            anim.SetFloat("hiphop2", 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetFloat("samba2", 5.0f);
            PlayAudio("winter");
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetFloat("samba2", 0.0f);
        }
    }

    void FixedUpdate()
    {
        if (serialPort.IsOpen)
        {
            string message = serialPort.ReadExisting(); // 시리얼 포트로부터 메시지 읽기
            ProcessSerialMessage(message); // 메시지 처리
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    void PlayAudio(string clipName)
    {
        switch (clipName)
        {
            case "summer":
                theAudio.clip = summer;
                break;
            case "winter":
                theAudio.clip = winter;
                break;
            case "alone":
                theAudio.clip = alone;
                break;
            default:
                return;
        }

        theAudio.Play();
    }

    void ProcessSerialMessage(string message)
    {         
        if (message.Contains("warning"))
        {
            anim.SetFloat("dance2", 5.0f);
            PlayAudio("alone");
            Debug.Log("Received warning message: " + message);
            StartCoroutine(ResetDance2State(1.0f)); // 1초 후에 dance2 변수 초기화
        }
        else if (message.Contains("high temp"))
        {
            anim.SetFloat("hiphop2", 5.0f);
            PlayAudio("summer");
            Debug.Log("Received high temperature message: " + message);
            StartCoroutine(ResetHipHop2State(1.0f)); // 1초 후에 hiphop2 변수 초기화
        }
        else if (message.Contains("low temp"))
        {
            anim.SetFloat("samba2", 5.0f);
            PlayAudio("winter");
            Debug.Log("Received low temperature message: " + message);
            StartCoroutine(ResetSamba2State(1.0f)); // 1초 후에 samba2 변수 초기화
        }
    }

    IEnumerator ResetDance2State(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetFloat("dance2", 0.0f);
    }

    IEnumerator ResetHipHop2State(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetFloat("hiphop2", 0.0f);
    }

    IEnumerator ResetSamba2State(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetFloat("samba2", 0.0f);  
    }
}
