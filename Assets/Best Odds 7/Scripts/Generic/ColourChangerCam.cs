using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourChangerCam : MonoBehaviour {

    public Color[] m_Colors;
    Color currentColor;
    int index = 0;
    int nextIndex;
    public float speed = 1f;
    float startTime = 0f;
    float progress = 0f;

    public Renderer rend;

    //Colour switch flags
    public bool inTransition;

    void Start()
    {
        inTransition = false;

        if (m_Colors.Length > 0)
        {
            currentColor = m_Colors[index];
            nextIndex = (index + 1) % m_Colors.Length;
            rend = this.GetComponent<Renderer>();
        }


    }

    void Update()
    {
        if(!inTransition)
        {
            progress = (Time.time - startTime) / speed;
            if (progress >= 1)
            {
                nextIndex = (index + 2) % m_Colors.Length;
                index = (index + 1) % m_Colors.Length;
                startTime = Time.time;
            }
            else
            {
                currentColor = Color.Lerp(m_Colors[index], m_Colors[nextIndex], progress);
            }
            rend.material.color = currentColor;
            rend.material.SetColor("_EmissionColor", currentColor);
        }
    }

    public IEnumerator RedFlash() 
    {
        if(!inTransition)
            inTransition=true;

        Debug.Log("Starting Red Flash!");
        float ElapsedTime = 0.0f;
        float TotalTime = .25f;
        while (ElapsedTime < TotalTime) {
            ElapsedTime += Time.deltaTime;
            rend.material.SetColor("_EmissionColor",  Color.Lerp(currentColor, Color.red, (ElapsedTime / TotalTime)));
            rend.material.color = Color.Lerp(currentColor, Color.red, (ElapsedTime / TotalTime));
            yield return null;
        }
        //end of lerp
        yield return new WaitForSeconds(1f);
        inTransition=false;
    }

    public IEnumerator GreenFlash() 
    {
        if(!inTransition)
            inTransition=true;

        Debug.Log("Starting Green Flash!");
        float ElapsedTime = 0.0f;
        float TotalTime = .25f;
        while (ElapsedTime < TotalTime) {
            ElapsedTime += Time.deltaTime;
            rend.material.SetColor("_EmissionColor",  Color.Lerp(currentColor, Color.green, (ElapsedTime / TotalTime)));
            rend.material.color = Color.Lerp(currentColor, Color.green, (ElapsedTime / TotalTime));
            yield return null;
        }
        //end of lerp
        yield return new WaitForSeconds(1f);
        //StartCoroutine(ResetGridColour());
        inTransition=false;
    }

    public IEnumerator ResetGridColour() 
    {
        Debug.Log("Resetting Grid!");
        float ElapsedTime = 0.0f;
        float TotalTime = 2.0f;
        while (ElapsedTime < TotalTime) {
            ElapsedTime += Time.deltaTime;
            rend.material.SetColor("_EmissionColor",  Color.Lerp(currentColor, m_Colors[0], (ElapsedTime / TotalTime)));
            rend.material.color = Color.Lerp(currentColor, m_Colors[0], (ElapsedTime / TotalTime));
            yield return null;
        }
        inTransition=false;
    }

}
