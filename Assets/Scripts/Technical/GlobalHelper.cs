using UnityEngine;
using System.Collections;


public class GlobalHelper : MonoBehaviour {


}

namespace RedHelp
{
	[System.Serializable]
	public class LevelTime
	{
		public int DTime;
		public int DPTime;
		public int CTime;
		public int CPTime;
		public int BTime;
		public int BPTime;
		public int AMTime;
		public int ATime;
		public int APTime;
		public int STime;
		public int SPTime;
		public int SSTime;
	}

	public enum SubTurretType {Gun, Laser};
	public enum TrackingType {Turn, Aim, AimIgnore, Fire, FireIgnore};
	
	[System.Serializable]
	public class SubTurret
	{
		public SubTurretType SubType;
		public GameObject Projectile;
		public float AngularOffset;
		public int ProjectileVelocity;
		public int InitDelay;
		public int ContDelay;
		public TrackingType Tracking;
		public float TurnSpeed;
		
		
		public GameObject ChildTurret;
		public float CurAngle;
		public int TimeSeen = 0;
		public Transform FirePosition;
		public Transform Target;
		public bool Visible;
	}



	class Helper
	{
		const string RECORD_TIME_HANDLE = "LevelTime";
		private const string KEYBLOCK_BREAK0 = "";
		public const string LineBreak = "\r\n"; 

		public static string RenderTime(int time)
		{
			if (time == -1)
			{
				return "99:59.999";
			}
			int m = Mathf.FloorToInt (time / 3600);
			int s = Mathf.FloorToInt (time / 60) % 60;
			float ms = Mathf.FloorToInt((time % 60) * 1000/60); 
			string y = "";
			if (m < 10)
			{
				y += "0";
			}
			y += m;
			y += ":";
			if (s < 10) {y += "0";}
			y += s + ".";
			if (ms < 100) {y += "0";}
			if (ms < 10) {y += "0";}
			y += ms;
			return y;
		}    

		public static string RenderRank (int r)
		{
			switch (r)
			{
			case 0:
				return "<color=#555555ff>NONE</color>";
			case 1:
				return "<color=#777777ff>D   </color>";
			case 2:
				return "<color=#999999ff>D+  </color>";
			case 3:
				return "<color=#bbbb00ff>C   </color>";
			case 4:
				return "<color=#eeee00ff>C+  </color>";			
			case 5:
				return "<color=#0088ddff>B   </color>";
			case 6:
				return "<color=#33bbffff>B+  </color>";
			case 7:
				return "<color=#ff5500ff>A-  </color>";
			case 8:
				return "<color=#ff8800ff>A   </color>";
			case 9:
				return "<color=#ffbb00ff>A+  </color>";
			case 10:
				return "<color=#55ff11ff>S   </color>";
			case 11:
				return "<color=#aaff66ff>S+  </color>";
			case 12:
				return "<color=#ddffaaff>SS  </color>";
			default:
				return "redspah was drunk while coding again";
			}
		}

		public static void SetRecordTime(int level, int time)
		{
			PlayerPrefs.SetInt(RECORD_TIME_HANDLE + level.ToString(), time);
		}

		public static int GetRecordTime(int level)
		{
			if (PlayerPrefs.HasKey(RECORD_TIME_HANDLE + level.ToString()))
			{
				return PlayerPrefs.GetInt(RECORD_TIME_HANDLE + level);
			}
			else
			{
				return -1;
			}

		}

		public static string ColorString(string text, Color color)
		{
			string end = "";
			end += "<color=#";
			end += Mathf.RoundToInt(color.r*255).ToString ("X2");
			end += Mathf.RoundToInt(color.g*255).ToString ("X2");
			end += Mathf.RoundToInt(color.b*255).ToString ("X2");
			end += Mathf.RoundToInt(color.a*255).ToString ("X2");
			end += ">";
			end += text;
			end += "</color>";
			return end;
		}

		public static string ColorString(string text, int r, int g, int b)
		{
			string end = "";
			end += "<color=#";
			end += r.ToString ("X2");
			end += g.ToString ("X2");
			end += b.ToString ("X2");
			end += "ff>";
			end += text;
			end += "</color>";
			return end;
		}

		public static string CustomString(string text, Color color, bool bold, bool cursive, bool resize, float size)
		{
			string end = "";
			if (bold)
			{
				end += "<b>";
			}
			
			if (cursive)
			{
				end += "<i>";
			}
			if (resize)
			{
				end += "<size="; 
				end += size.ToString();
				end += ">";
			}
			
			end += "<color=#";
			end += Mathf.RoundToInt(color.r*255).ToString ("X2");
			end += Mathf.RoundToInt(color.g*255).ToString ("X2");
			end += Mathf.RoundToInt(color.b*255).ToString ("X2");
			end += Mathf.RoundToInt(color.a*255).ToString ("X2");
			end += ">";
			
			
			end += text;
			
			if (resize){
				end += "</size>";
			}

			end += "</color>";
			
			if (bold)
			{
				end += "</b>";
			}
			if (cursive)
			{
				end += "</i>";
			}
			return end;
		}

		public static float HalfRound(float f)
		{
			return (Mathf.Round(f*2)/2);
		}


	}
}

