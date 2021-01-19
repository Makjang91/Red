using UnityEngine.EventSystems;
using UnityEngine;

public class AnimatorParameter 
{
    public int intParam;
    public float floatParam;
    public bool boolParam;
    public bool trig;

    public AnimatorParameter(int _int) { intParam = _int; }
    public AnimatorParameter(float _float) { floatParam = _float; }
    public AnimatorParameter(bool _bool) { boolParam = _bool; }
    public AnimatorParameter(EventTrigger _et) { trig = _et; }

}
