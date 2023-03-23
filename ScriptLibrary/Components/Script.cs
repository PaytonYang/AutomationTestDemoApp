using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ScriptLibrary
{
    [Serializable]
    public class Script : ScriptBase
    {
        public List<Step> Content = new List<Step>();

        public Script()
        {
            ScriptType = ScriptType.NormalScript;
        }
    }

    [Serializable]
    public class ScriptBase
    {
        public ScriptType ScriptType { get; set; }
        public int LoopTimes { get; set; } = 1;
        public List<string> AllClassNameInScript { get; set; } = new List<string>();
    }

    public enum ScriptType
    {
        NormalScript = 0,
        SemiAutoScript,
        CompoundScript
    }
}