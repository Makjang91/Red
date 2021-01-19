using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TimeManager : MonoBehaviour
{
    List<GameObject> tabGo;

    List<List<TimeInfo>> timeInfos;

    List<string> animOrigin;
    List<string> animReversed;

    bool rewinding;

    mouseCursor mc;
    float rewindTime = 5f;

    // Start is called before the first frame update
    void Start() 
    {
        mc = GameObject.Find("Red").GetComponentInChildren<mouseCursor>();

        GameObject[] tab = FindObjectsOfType<GameObject>();
        tabGo = new List<GameObject>();
        foreach (GameObject go in tab)
        {
            if (go.tag == "MainCamera")
                continue;
            if(go.GetComponent<Animator>())
                tabGo.Add(go);
        }

        tabGo.Add(mc.gameObject);


        animOrigin = new List<string>();
        animReversed = new List<string>();
        string name = null;
        foreach (GameObject go in tabGo)
        {
            if (go.GetComponent<Animator>() && go.GetComponent<Rigidbody2D>()) 
            {
                name = go.GetComponent<Animator>().runtimeAnimatorController.name;
                animOrigin.Add("animation/Animator/" + name);
                animReversed.Add("animation/Animator/rewind/" + name + "(rewind)");
            }

        }

        StartRecording();
    }

    

    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (!rewinding)
            {
                StartRewind();
                
                mc.changeTrackingMouss();
            }
            else
            {
                StopRewind();
            }            
        }

        
    }

    void FixedUpdate()
    {
        if (rewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }

    }

    void StartRecording()
    {
        InitializeRecording();
    }

    void InitializeRecording()
    {
        timeInfos = new List<List<TimeInfo>>();
        foreach (GameObject go in tabGo)
        {
            timeInfos.Add(new List<TimeInfo>());
        }

    }

    void SaveTimeInfo(int indice, GameObject go)
    {
        Transform t = go.GetComponent<Transform>();
        Vector2 vel;


        if (go.GetComponent<Animator>())
        { 
            if (go.GetComponent<Rigidbody2D>())
            {
                vel = go.GetComponent<Rigidbody2D>().velocity;
                timeInfos[indice].Insert(0, new TimeInfo(t.position, t.localScale, vel, go.GetComponent<Animator>()));
                if (go.GetComponent<Enemy>())
                    timeInfos[indice][0].setAlive(go.GetComponent<Enemy>().getDead());
            }
            else
            {
                timeInfos[indice].Insert(0, new TimeInfo(t.position, t.localScale, go.GetComponent<Animator>()));
            }
        }
        else if (go.GetComponent<Rigidbody2D>())
        {
            vel = go.GetComponent<Rigidbody2D>().velocity;
            timeInfos[indice].Insert(0, new TimeInfo(t.position, t.localScale, vel));
        }
        else
        {
             timeInfos[indice].Insert(0, new TimeInfo(t.position, t.localScale));
        }

        
    }


    void Record()
    {
        int i = 0;
        // limiter que recording dure 5sec
        if (timeInfos[0].Count > Mathf.Round(rewindTime / Time.fixedDeltaTime))
        {
            i = 0;
            foreach (GameObject go in tabGo)
            {
                timeInfos[i].RemoveAt(timeInfos[i].Count - 1);
                i++;
            }
        }

        i = 0;
        foreach (GameObject go in tabGo)
        {
            SaveTimeInfo(i, go);
            i++;
        }
    }


    
    void StartRewind()
    {
        Debug.Log("Starting Replay");
        rewinding = true;
        SwapAnimator();

        foreach(GameObject go in tabGo)
        {
            switch (go.tag)
            {
                case "Player":
                    go.GetComponent<Player>().setRewind(true);
                    break;
                case "Enemies":
                    go.GetComponent<Enemy>().setRewind(true);
                    break;
            }
        }

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().StartRewind();
    }

    void StopRewind()
    {
        rewinding = false;
        mc.changeTrackingMouss();
        SwapAnimator();

        foreach (GameObject go in tabGo)
        {
            switch (go.tag)
            {
                case "Player":
                    go.GetComponent<Player>().setRewind(false);
                    break;
                case "Enemies":
                    go.GetComponent<Enemy>().setRewind(false);
                    break;
            }
        }

        for (int i = 0; i<timeInfos.Count; i++)
        {
            timeInfos[i].Clear();
        }

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().StopRewind();
    }

    void Rewind()
    {
        int i = 0;
        foreach (GameObject go in tabGo)
        {
            if(timeInfos[i].Count > 0)
            {
                LoadTimeInfo(i, go);
            }
            else
            {
                if(go.tag == "Bullet")
                    go.GetComponent<SpriteRenderer>().enabled = false;
            }
            i++;
        }
        if (timeInfos[0].Count <= 0)
            StopRewind();

        

    }

    void LoadTimeInfo(int indice, GameObject go)
    {
        go.GetComponent<Transform>().position = timeInfos[indice][0].position;
        go.GetComponent<Transform>().localScale = timeInfos[indice][0].scale;
        if (go.GetComponent<Rigidbody2D>())
        {
            if(go.GetComponent<Enemy>() && go.GetComponent<Enemy>().getDead())
            {

            }
            else
            {
                if (go.tag == "Bullet" && go.GetComponent<Rigidbody2D>().velocity != new Vector2( 0, 0))
                {
                    Debug.Log("rewind bullet");
                    go.GetComponent<Bullet>().rewind();
                }
                go.GetComponent<Rigidbody2D>().velocity = timeInfos[indice][0].velocity;
                
            }
        }
        if (go.GetComponent<Animator>())
        {
            Animator animator = go.GetComponent<Animator>();
            //go.GetComponent<Animator>().Play(timeInfos[indice][0].animInfo) ;

            if (go.GetComponent<Rigidbody2D>()) // pour filter tout ce qui peut bouger
            {
                if (go.GetComponent<Enemy>() && timeInfos[indice][0].alive && go.GetComponent<Enemy>().getDead())
                {
                    go.GetComponent<Enemy>().rewindDie();
                }
                
                for (int i = 0; i < animator.parameters.Length; i++)
                {
                    switch (animator.parameters[i].type.ToString())
                    {
                        case "Int":
                            animator.SetInteger(animator.parameters[i].name, timeInfos[indice][0].animParam[i].intParam);
                            break;
                        case "Float":
                            animator.SetFloat(animator.parameters[i].name, timeInfos[indice][0].animParam[i].floatParam);
                            break;
                        case "Bool":
                            animator.SetBool(animator.parameters[i].name, timeInfos[indice][0].animParam[i].boolParam);
                            break;
                    }


                }
            }
            else // des objects avec animator
            {
                for (int i = 0; i < animator.parameters.Length; i++)
                {
                    switch (animator.parameters[i].type.ToString())
                    {
                        case "Int":
                            animator.SetInteger(animator.parameters[i].name, timeInfos[indice][0].animParam[i].intParam);
                            break;
                        case "Float":
                            animator.SetFloat(animator.parameters[i].name, timeInfos[indice][0].animParam[i].floatParam);
                            break;
                        case "Bool":
                            if(animator.GetComponent<DoorScript>())
                            {
                                if (timeInfos[indice][0].animParam[i].boolParam)
                                    animator.GetComponent<DoorScript>().Open();
                                else
                                    animator.GetComponent<DoorScript>().Close();
                            }
                            animator.SetBool(animator.parameters[i].name, timeInfos[indice][0].animParam[i].boolParam);
                            break;
                    }
                }
            }
        }

        // animator place
        timeInfos[indice].RemoveAt(0);
    }

    void SwapAnimator()
    {
        if (rewinding)
        {
            
            int i = 0;
            foreach (GameObject go in tabGo)
            {
                if (go.GetComponent<Animator>() && go.GetComponent<Rigidbody2D>())
                {
                    go.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animReversed[i]);
                    i++;
                }

            }
        }
        else
        {
            int i = 0;
            foreach (GameObject go in tabGo)
            {
                if (go.GetComponent<Animator>() && go.GetComponent<Rigidbody2D>())
                {
                    go.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animOrigin[i]);
                    i++;
                }

            }
        }
    }

    public void addBullet(GameObject b)
    {
        tabGo.Add(b);
        timeInfos.Add(new List<TimeInfo>());
    }

    public void destroyBullet(GameObject b)
    {
        int ind = tabGo.FindIndex(x => x.GetInstanceID() == b.GetInstanceID());
        tabGo.RemoveAt(ind);
        timeInfos.RemoveAt(ind);
    }

}
