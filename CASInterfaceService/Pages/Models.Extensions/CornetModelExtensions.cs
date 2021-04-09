using Gov.Cscp.VictimServices.Public.JsonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASInterfaceService.Pages.Models.Extensions
{
    public static class CornetModelExtensions
    {
        public static CornetDynamicsModel ToCornetDynamicsModel(this CornetTransaction model)
        {
            CornetDynamicsModel cornetTransaction = new CornetDynamicsModel();

            cornetTransaction.EventId = model.event_message_id;
            cornetTransaction.EventType = model.message_event_type_cd;
            cornetTransaction.EventDate = model.event_dtm;
            cornetTransaction.DataElement1 = model.event_data[0].data_element_nm;
            cornetTransaction.DataValue1 = model.event_data[0].data_value_txt;
            cornetTransaction.DataElement2 = model.event_data[1].data_element_nm;
            cornetTransaction.DataValue2 = model.event_data[1].data_value_txt;
            cornetTransaction.DataElement3 = model.event_data[2].data_element_nm;
            cornetTransaction.DataValue3 = model.event_data[2].data_value_txt;
            cornetTransaction.DataElement4 = model.event_data[3].data_element_nm;
            cornetTransaction.DataValue4 = model.event_data[3].data_value_txt;
            return cornetTransaction;
        }
    }
}

