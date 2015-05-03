using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AIDatatypes
{
	[System.Serializable]
	public class InputVar 
	{
		public string VarName;
		public Type VarType; 
		
		public InputVar()
		{
			VarName = "NULL_INPUT";
			VarType = typeof(void);
		}
		
		public InputVar(string vnam, Type vtyp)
		{
			VarName = vnam;
			VarType = vtyp; 
		}
		
		public bool IsNull()
		{
			return (VarType == typeof(void));
		}
	}

	[System.Serializable]
	public class AIFunction
	{
		//public AIFunctionType Type;
		public string Code;
		public string Discription;
		public string Name;
		public List<InputVar> Inputs;
		
		public AIFunction(string nam, string disc, InputVar i, string cd) : this(nam, disc, i, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, cd) {}
		public AIFunction(string nam, string disc, InputVar i, InputVar i2, string cd) : this(nam, disc, i, i2, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, cd) {}
		public AIFunction(string nam, string disc, InputVar i, InputVar i2, InputVar i3, string cd) : this(nam, disc, i, i2, i3, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, cd) {}
		public AIFunction(string nam, string disc, InputVar i, InputVar i2, InputVar i3, InputVar i4, string cd) : this(nam, disc, i, i2, i3, i4, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, cd) {}
		public AIFunction(string nam, string disc, InputVar i, InputVar i2, InputVar i3, InputVar i4, InputVar i5, string cd) : this(nam, disc, i, i2, i3, i4, i5, AIFunctions.NULL_INPUT, AIFunctions.NULL_INPUT, cd) {}
		public AIFunction(string nam, string disc, InputVar i, InputVar i2, InputVar i3, InputVar i4, InputVar i5, InputVar i6, string cd) : this(nam, disc, i, i2, i3, i4, i5, i6, AIFunctions.NULL_INPUT, cd) {}
		/// <summary>
		/// Constructor for an AIFunction class.
		/// </summary>
		public AIFunction(string nam, string disc, InputVar i1, InputVar i2, InputVar i3, InputVar i4, InputVar i5, InputVar i6, InputVar i7, string cd)
		{
			Name = nam;
			Discription = disc;
			Code = cd;
			Inputs = new List<InputVar>();
			if (!i1.IsNull())
			{
				Inputs.Add(i1);
			}
			if (!i2.IsNull())
			{
				Inputs.Add(i2);
			}
			if (!i3.IsNull())
			{
				Inputs.Add(i3);
			}
			if (!i4.IsNull())
			{
				Inputs.Add(i4);
			}
			if (!i5.IsNull())
			{
				Inputs.Add(i5);
			}
			if (!i6.IsNull())
			{
				Inputs.Add(i6);
			}
			if (!i7.IsNull())
			{
				Inputs.Add(i7);
			}
		}
		
	}

public class AIFunctions : MonoBehaviour {

	public static readonly InputVar NULL_INPUT = new InputVar("NULL_INPUT", typeof(void));
	
	public List<AIFunction> Functions = new List<AIFunction> ();

	public void FuncListInit()
		{
			Functions.Add (new AIFunction("FixedUpdate Wait", "Waits given number of FixedUpdates.", new InputVar("No. of FixedUpdates",typeof(int)), "for(int i = 0; i < {0}; i++) { \r\n yield new WaitForFixedUpdate(); \r\n }; \r\n"));
			Functions.Add (new AIFunction("Time Wait", "Waits given number of seconds.", new InputVar("Time in seconds", typeof(float)), "yield WaitForSeconds({0}) \r\n"));

































		}


























}}