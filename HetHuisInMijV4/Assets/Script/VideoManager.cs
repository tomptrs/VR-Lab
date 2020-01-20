using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
//C:\Program Files\Unity\Hub\Editor\2019.2.12f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools
//adb logcat -s unity
public class VideoManager : MonoBehaviour
{

    public Transform rig;

    public Vector3 PositionVideo;
    public Vector3 PositionBlur;

    public GameObject VideoSphere;
    public GameObject BlurSphere;

    VideoPlayer Video;
    VideoPlayer Blur;

    bool IntroEnd;
    bool startEnd;
    bool end;
    bool reset;

    public GameObject videoLight;

    //voor de container
    public Animator anim;
    public GameObject container;
    public Vector3 Away;
    public Vector3 Home;
    public GameObject containerLight;

    // Start is called before the first frame update
    void Start()
    {
        Video = VideoSphere.GetComponent<VideoPlayer>();
        Blur = BlurSphere.GetComponent<VideoPlayer>();

        Video.Prepare();
        Blur.Prepare();

        Video.loopPointReached += eindeVideo;
        Blur.loopPointReached += eindeBlur;
        startEnd = false;
        IntroEnd = false;
        reset = false;
        rig.position = PositionVideo;
        Blur.Pause();
        Video.Pause();

        containerLight.SetActive(true);
        videoLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rig.position);
        if ( Video.isPrepared && Blur.isPrepared){
            if (OVRInput.GetDown(OVRInput.Button.One)){
                anim.SetBool("start", true);
                anim.SetBool("stop", false);
                IntroEnd = true;
                }
            if( anim.GetCurrentAnimatorStateInfo(0).IsName("open container state") && 
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                container.transform.position = Away;
                videoLight.SetActive(true);
                anim.SetBool("start", false);
                anim.SetBool("stop", true);
                anim.Play("Waiting State",-1,0f);
            }
            if (IntroEnd){
                if (!startEnd){
                    Blur.Pause();
                    Video.Play();
                }

                if (end || OVRInput.GetDown(OVRInput.Button.Two)){
                    startEnd = true;
                    rig.position = PositionBlur;
                    container.transform.position = Home;
                    Blur.Play();
                    Video.Pause();
                }
            }
        }
        

        if(reset){
            Video.Prepare();
            Blur.Prepare();
            rig.position = PositionVideo;
            reset = false;
            startEnd = false;
            videoLight.SetActive(false);
            containerLight.SetActive(true);
        }
    }

    void eindeVideo(UnityEngine.Video.VideoPlayer vp){
        end = true;
    }
    void eindeBlur(UnityEngine.Video.VideoPlayer vp2){
        reset = true;
        end = false;
        Video.Stop();
        Blur.Stop();
    }
}
