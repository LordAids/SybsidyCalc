using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SybsidiiCalc
{
    public class SubsidiiCalc : ISubsidyCalculation
    {
        public delegate void EventHandler(string message);

        public event EventHandler<string> OnNotify;
        public event EventHandler<Tuple<string, Exception>> OnException;
        
        /// <summary>
        /// Метод для вызова ошибки
        /// </summary>
        /// <param name="message">Сообщение ошибки</param>
        private void SendExeption(string message)
        {
            OnException?.Invoke(this, new Tuple<string, Exception>(message, new Exception(message)));
        }

        
        public bool Validation(Volume volume, Tariff tarif)
        {
            bool valid = true;
            if (volume.HouseId != tarif.HouseId)
            {
                SendExeption("<HouseId> Индификаторы домов не совпадают \n");
                valid=false;
            }   
            if (volume.ServiceId != tarif.ServiceId)
            {
                SendExeption("<ServiceId> Индификаторы услуг не совпадают \n");
                valid= false;
            }
            if (tarif.PeriodBegin >= volume.Month || volume.Month >= tarif.PeriodEnd)
            {
                SendExeption("<Month> Месяц объема не входит в период действия тарифа! \n");
                valid= false;
            }
            if (tarif.Value <=0)
            {
                SendExeption("<Tarif.Value> Значение тарифа не должно быть меньше или равно нулю \n");
                valid = false;
            }
            if (volume.Value <0)
            {
                SendExeption("< Volume.Value > Значение объема не должно быть меньше нуля\n");
                valid = false;
            }
                return valid;
        }
        public Charge CalculateSubsidy(Volume volumes, Tariff tariff)
        {
            Charge charge = null;
            try
            { 
                OnNotify?.Invoke(this, $"Расчёт начат в {DateTime.Now:G}");
                if (!Validation(volumes, tariff)) return null;

                charge = new Charge
                {
                    HouseId = volumes.HouseId,
                    ServiceId = volumes.ServiceId,
                    Month = volumes.Month,
                    Value = volumes.Value * tariff.Value
                };
                OnNotify?.Invoke(this, $"Расчет успешно завершен в {DateTime.Now:G}");

            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, new Tuple<string, Exception>(ex.Message, ex));
            }
            
     
            return charge;
        }
    }
}
