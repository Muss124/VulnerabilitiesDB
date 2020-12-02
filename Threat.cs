using System;

namespace WpfApp1
{
    [Serializable]
    public class Threat
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Obj { get; set; }
        public string Confidentiality { get; set; }
        public string Integrity { get; set; }
        public string Availability { get; set; }

        public Threat(string id, string name, string description, string source, string obj, string confidentiality, string integrity, string availability)
        {
            ID = id;
            Name = name;
            Description = description;
            Source = source;
            Obj = obj;
            Confidentiality = confidentiality;
            Integrity = integrity;
            Availability = availability;
        }
        public Threat() { }

        public static bool Equals(Threat first, Threat second)
        {
            if (first.Name == second.Name &&
                first.Description == second.Description &&
                first.Source == second.Source &&
                first.Obj == second.Obj &&
                first.Confidentiality == second.Confidentiality &&
                first.Integrity == second.Integrity &&
                first.Availability == second.Availability)
            {
                return true;
            }
            return false;
        }
    }

}
