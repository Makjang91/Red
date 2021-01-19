using UnityEngine;

public class TimeInfo
{
    public Vector3 position;
    public Vector3 scale;
    public Vector2 velocity;
    public AnimatorParameter[] animParam;

    public bool alive;


    public TimeInfo(Vector3 _position, Vector3 _Scale)
    {
        position = _position;
        scale = _Scale;
    }
    public TimeInfo(Vector3 _position, Vector3 _Scale, Vector2 _velocity)
    {
        position = _position;
        scale = _Scale;
        velocity = _velocity;
    }

    public TimeInfo(Vector3 _position, Vector3 _Scale, Animator _anim)
    {
        position = _position;
        scale = _Scale;

        int longeur = _anim.parameters.Length;
        animParam = new AnimatorParameter[longeur];

        for (int i = 0; i < longeur; i++)
        {
            switch (_anim.parameters[i].type.ToString())
            {
                case "Int":
                    animParam[i] = new AnimatorParameter(_anim.GetInteger(_anim.parameters[i].name));
                    break;
                case "Float":
                    animParam[i] = new AnimatorParameter(_anim.GetFloat(_anim.parameters[i].name));
                    break;
                case "Bool":
                    animParam[i] = new AnimatorParameter(_anim.GetBool(_anim.parameters[i].name));
                    break;
            }
        }
    }

    public TimeInfo(Vector3 _position, Vector3 _Scale, Vector2 _velocity, Animator _anim)
    {
        position = _position;
        scale = _Scale;
        velocity = _velocity;

        int longeur = _anim.parameters.Length;
        animParam = new AnimatorParameter[longeur];

        for(int i=0; i< longeur; i++ )
        {
            switch(_anim.parameters[i].type.ToString())
            {
                case "Int":
                    animParam[i] = new AnimatorParameter(_anim.GetInteger(_anim.parameters[i].name));
                    break;
                case "Float":
                    animParam[i] = new AnimatorParameter(_anim.GetFloat(_anim.parameters[i].name));
                    break;
                case "Bool":
                    animParam[i] = new AnimatorParameter(_anim.GetBool(_anim.parameters[i].name));
                    break;
                /*case "Trigger":
                    Debug.Log(_anim.GetBool(_anim.parameters[i].name) + "   " + _anim.parameters[i].name);
                    animParam[i] = new AnimatorParameter(_anim.GetBool(_anim.parameters[i].name));
                    break;*/
            }
        }

        
    }
    public void setAlive(bool _alive)
    {
        alive = _alive;
    }
}
