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
        
        GUILayout.Space(20);
        GUILayout.Label("Normal",EditorStyles.boldLabel);
        PrintProp("Max Velocity",config.MaxVelocity);
        PrintProp("Max Jump Height",config.JumpHeight);
        float airTime = config.GetAirTime(config.JumpHeight);
        PrintProp("Air Time",airTime);
        PrintProp("Range",config.MaxVelocity*airTime);
        
        GUILayout.Space(20);
        GUILayout.Label("Slide Jump",EditorStyles.boldLabel);
        PrintProp("Max Velocity",config.MaxVelocity * config.SlideJumpSpeedRatio);
        PrintProp("Max Jump Height",config.JumpHeight * config.SlideJumpHeightRatio);
        airTime = config.GetAirTime(config.JumpHeight * config.SlideJumpHeightRatio);
        PrintProp("Air Time",airTime);
        PrintProp("Range",config.MaxVelocity*airTime*config.SlideJumpSpeedRatio);
        
        GUILayout.Space(20);
        GUILayout.Label("Charged Jump",EditorStyles.boldLabel);
        PrintProp("Max Jump Height",config.JumpHeight * config.ChargedJumpHeightRatio);
        airTime = config.GetAirTime(config.JumpHeight * config.ChargedJumpHeightRatio);
        PrintProp("Air Time",airTime);

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
            GUILayout.Label(" : ",GUILayout.Width(10));
            GUILayout.Label(Value + "ms",GUILayout.Width(150));
        GUILayout.EndHorizontal();
    }
}
