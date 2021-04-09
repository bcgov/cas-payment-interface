using System;

namespace Gov.Cscp.VictimServices.Public.JsonObjects
{
    public class CornetDynamicsModel
    {
        public Int64 EventId { get; set; }
        public string EventType { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public string DataElement1 { get; set; }
        public string DataValue1 { get; set; }
        public string DataElement2 { get; set; }
        public string DataValue2 { get; set; }
        public string DataElement3 { get; set; }
        public string DataValue3 { get; set; }
        public string DataElement4 { get; set; }
        public string DataValue4 { get; set; }
    }

}


