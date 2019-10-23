using UnityEngine;
using System.Collections;

using NLua;
using System.IO;

public class ScriptLua : MonoBehaviour
{

    private string source;


    Lua env;

    void Awake()
    {
        source = File.ReadAllText(@".\Assets\ScriptLua\ExampleBehaviour.lua");
        env = new Lua();
        env.LoadCLRPackage();

        env["this"] = this; // Give the script access to the gameobject.
        env["transform"] = transform;

        //System.Object[] result = new System.Object[0];
        try
        {
            //result = env.DoString(source);
            env.DoString(source);
        }
        catch (NLua.Exceptions.LuaException e)
        {
            Debug.LogError(FormatException(e), gameObject);
        }

    }

    void Start()
    {
        Call("Start");
    }

    void Update()
    {
        Call("Update");
    }

    void OnGUI()
    {
        Call("OnGUI");
    }

    public System.Object[] Call(string function, params System.Object[] args)
    {
        System.Object[] result = new System.Object[0];
        if (env == null) return result;
        LuaFunction lf = env.GetFunction(function);
        if (lf == null) return result;
        try
        {
            // Note: calling a function that does not 
            // exist does not throw an exception.
            if (args != null)
            {
                result = lf.Call(args);
            }
            else
            {
                result = lf.Call();
            }
        }
        catch (NLua.Exceptions.LuaException e)
        {
            Debug.LogError(FormatException(e), gameObject);
            throw e;
        }
        return result;
    }

    public System.Object[] Call(string function)
    {
        return Call(function, null);
    }

    public static string FormatException(NLua.Exceptions.LuaException e)
    {
        string source = (string.IsNullOrEmpty(e.Source)) ? "<no source>" : e.Source.Substring(0, e.Source.Length - 2);
        return string.Format("{0}\nLua (at {2})", e.Message, string.Empty, source);
    }
}