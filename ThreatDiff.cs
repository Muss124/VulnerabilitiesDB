namespace WpfApp1
{
    public class ThreatDiff
    {
        public string ID { get; set; }
        public string Field { get; set; }
        public string Before { get; set; }
        public string After { get; set; }

        public ThreatDiff(string iD, string field, string before, string after)
        {
            ID = iD;
            Field = field;
            Before = before;
            After = after;
        }
    }
}
