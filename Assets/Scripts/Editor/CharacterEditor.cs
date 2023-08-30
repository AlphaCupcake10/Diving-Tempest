using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterConfig))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(20);

        CharacterConfig config = (CharacterConfig)target;

        GUILayout.BeginVertical();
            float AirTime = Mathf.Sqrt(Mathf.Abs(8*config.JumpHeight/Physics2D.gravity.y));
            float MaxRange = config.MaxVelocity * AirTime;

            GUILayout.Label("Normal");
            PrintProp("Jump Height :",config.JumpHeight);
            PrintProp("Max Velocity :",config.MaxVelocity);
            PrintProp("Max Jump Range :",MaxRange);
            PrintSingle("Air Time :",AirTime);
            GUILayout.Space(25);
            
            AirTime = Mathf.Sqrt(Mathf.Abs(8*config.SlideJumpHeight/Physics2D.gravity.y));
            MaxRange = config.MaxVelocity * AirTime * config.SlideJumpSpeedBoost;

            GUILayout.Label("Sliding");
            PrintProp("Jump Height :",config.SlideJumpHeight);
            PrintProp("Max Velocity :",config.MaxVelocity*config.SlideJumpSpeedBoost);
            PrintProp("Max Jump Range :",MaxRange);
            PrintSingle("Air Time :",AirTime);

        GUILayout.EndVertical();
    }
    void PrintProp(string Label,float Value)
    {
        GUILayout.BeginHorizontal();
            GUILayout.Label(Label,GUILayout.Width(150));
            GUILayout.Label(Value + "units",GUILayout.Width(150));
            GUILayout.Label(Value*2 + "blocks",GUILayout.Width(150));
        GUILayout.EndHorizontal();
    }
    void PrintSingle(string Label,float Value)
    {
        GUILayout.BeginHorizontal();
            GUILayout.Label(Label,GUILayout.Width(150));
            GUILayout.Label(Value + "ms",GUILayout.Width(150));
        GUILayout.EndHorizontal();
    }
}
