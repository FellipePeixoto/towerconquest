﻿using UnityEngine;
using System.Collections;

using NLua;

public class ExampleBehaviour : MonoBehaviour {

	string source = @"
import 'System'
import 'UnityEngine'
import 'Assembly-CSharp'	-- The user-code assembly generated by Unity.

local Angle = Vector3.up;
local Speed = 5;

local showEnv = false
local envScroll = Vector2.zero

function Update()
	transform:RotateAround(transform.position,Angle,Speed * Time.deltaTime)

	if Input.GetKey(KeyCode.W) then
		Speed = Speed + 1
	end

	if Input.GetKey(KeyCode.S) then
		Speed = Speed - 1
	end

	if Input.GetKey(KeyCode.A) then
		Angle.z = Angle.z + 1
	end

	if Input.GetKey(KeyCode.D) then
		Angle.z = Angle.z - 1
	end

end

function OnGUI()
	GUILayout.BeginArea(Rect(10,10,(Screen.width / 2) - 20,Screen.height - 20))

	-- Adding '{ }' to the end of GUI functions satisfies their 'params' argument.
	GUILayout.Label('[W/S] Speed: ' .. Speed) 
	GUILayout.Label('[A/D] Rot Angle: ' .. Angle:ToString())

	GUILayout.EndArea()

	GUILayout.BeginArea(Rect((Screen.width / 2) + 10,10,(Screen.width / 2) - 20,Screen.height - 20))

	if GUILayout.Button('Show Enviroment',{ }) then
		showEnv = not showEnv
	end

	if showEnv then
		envScroll = GUILayout.BeginScrollView(envScroll,{ })
		for k, v in pairs(_G) do
			GUILayout.BeginHorizontal({ })

			GUILayout.Label(k,{ })
			GUILayout.FlexibleSpace()
			GUILayout.Label(tostring(v),{ })

			GUILayout.EndHorizontal()
		end
		GUILayout.EndScrollView()
	end	

	GUILayout.EndArea()
end

";

	Lua env;

	void Awake() {
		env = new Lua();
		env.LoadCLRPackage();
		
		env["this"] = this; // Give the script access to the gameobject.
		env["transform"] = transform;
		
		//System.Object[] result = new System.Object[0];
		try {
			//result = env.DoString(source);
			env.DoString(source);
		} catch(NLua.Exceptions.LuaException e) {
			Debug.LogError(FormatException(e), gameObject);
		}
	}

	void Start () {
		Call("Start");
	}
	
	void Update () {
		Call("Update");
	}

	void OnGUI() {
		Call("OnGUI");
	}

	public System.Object[] Call(string function, params System.Object[] args) {
		System.Object[] result = new System.Object[0];
		if(env == null) return result;
		LuaFunction lf = env.GetFunction(function);
		if(lf == null) return result;
		try {
			// Note: calling a function that does not 
			// exist does not throw an exception.
			if(args != null) {
				result = lf.Call(args);
			} else {
				result = lf.Call();
			}
		} catch(NLua.Exceptions.LuaException e) {
			Debug.LogError(FormatException(e), gameObject);
			throw e;
		}
		return result;
	}

	public System.Object[] Call(string function) {
		return Call(function, null);
	}

	public static string FormatException(NLua.Exceptions.LuaException e) {
		string source = (string.IsNullOrEmpty(e.Source)) ? "<no source>" : e.Source.Substring(0, e.Source.Length - 2);
		return string.Format("{0}\nLua (at {2})", e.Message, string.Empty, source);
	}
}
