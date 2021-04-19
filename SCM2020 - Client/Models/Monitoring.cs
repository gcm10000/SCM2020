using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client.Models
{
    public class Monitoring : ModelsLibraryCore.Monitoring
    {
        public Monitoring() { }
        /// <summary>
        /// Verifica se o monitoramento é existente.
        /// </summary>
        /// <param name="workOrder">Ordem de serviço consultada.</param>
        /// <returns></returns>
        public static bool ExistsMonitoring(string workOrder)
        {
            if (workOrder == string.Empty)
            {
                throw new Exception("Ordem de serviço em branco.");
            }
            if (workOrder == null)
            {
                throw new ArgumentNullException("Ordem de serviço nula.");
            }
            workOrder = System.Uri.EscapeDataString(workOrder);
            var result = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"Monitoring/ExistsWorkOrder/{workOrder}").ToString(), Helper.Authentication);
            return result;
        }

        /// <summary>
        /// Verifica se a ordem de serviço encontra-se aberta ou fechada. Valores: aberta é *false*, fechada é *true*.
        /// </summary>
        /// <param name="workOrder">Ordem de serviço consultada.</param>
        /// <returns></returns>
        public static bool CheckWorkOrder(string workOrder)
        {
            if (workOrder == string.Empty)
            {
                throw new Exception("Ordem de serviço em branco.");
            }
            if (workOrder == null)
            {
                throw new ArgumentNullException("Ordem de serviço nula.");
            }
            workOrder = System.Uri.EscapeDataString(workOrder);
            var result = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"Monitoring/CheckWorkOrder/{workOrder}").ToString(), Helper.Authentication);
            return result;
        }
    }
}
