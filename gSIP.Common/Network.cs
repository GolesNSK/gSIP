using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using gSIP.Logger;
using log4net;

namespace gSIP.Common
{
    /// <summary>
    /// Статический класс содержащий различные сетевые утилиты.
    /// </summary>
    public static class Network
    {
        /// <summary>
        /// Логгер для ведения журнала событий приложения.
        /// </summary>
        private static ILog Log = AppLogger.GetLogger("LOGGER");

        /// <summary>
        /// Найденный IPv4 адрес.
        /// </summary>
        private static IPAddress IPv4Address = null;

        /// <summary>
        /// При первом запуске определяется первый попавшийся IPv4 адрес и запонимается в статической переменной.
        /// При последующих запусках поиск не ведется и возвращается значение из статической переменной.
        /// </summary>
        /// <returns>Возвращает первый подходящий IPv4 адрес от имеющихся сетевых адаптеров.</returns>
        public static IPAddress GetIPv4Address()
        {
            Log.Debug("Поиск локального IPv4 адреса.");
            if (IPv4Address != null)
            {
                Log.DebugFormat("Выбран ранее найденный IPv4 адрес: {0}", IPv4Address);
                return IPv4Address;
            }
            else
            {
                NetworkInterface[] networkInterfaces;

                // Получение списка всех сетевых интерфейсов.
                try
                {
                    networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                }
                catch (NetworkInformationException ex)
                {
                    Log.Error("Ошибка получения списка сетевых интерфейсов.", ex);
                    return null;
                }

                // Анализ каждого интерфейса.
                foreach (NetworkInterface network in networkInterfaces)
                {
                    IPInterfaceProperties properties = network.GetIPProperties();

                    // Каждый сетевой интерфейс может иметь несколь IP адресов.
                    foreach (IPAddressInformation addresses in properties.UnicastAddresses)
                    {
                        if (addresses.Address.AddressFamily == AddressFamily.InterNetwork
                            && !IPAddress.IsLoopback(addresses.Address))
                        {
                            IPv4Address = addresses.Address;
                            Log.DebugFormat("Найден локальный IPv4 адрес: {0}", IPv4Address);
                            return IPv4Address;
                        }
                    }
                }
            }

            Log.Error("Локальный IPv4 адрес не найден!");
            return null;
        }

        /// <summary>
        /// Получение номера свободного UDP порта.
        /// </summary>
        /// <param name="ipAddress">IP адрес на котором будет осуществляться поиск свободного номера порта.</param>
        /// <param name="startPort">Начальное значение диапазона номеров в котором будет осуществляться поиск.</param>
        /// <param name="endPort">Конечное значение диапазона номеров в котором будет осуществляться поиск.</param>
        /// <returns>Возвращает номер свободнгого UDP порта, если номер не найден, то возвращается 0.</returns>
        public static int GetFreeUDPPort(IPAddress ipAddress, int startPort, int endPort)
        {
            Log.DebugFormat("Поиск свободного UDP порта в диапазоне от {0} до {1}.", startPort, endPort);
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            // Получение списка активных UDP слушателей.
            IPEndPoint[] endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startPort & n.Port <= endPort & n.Address.Equals(ipAddress)
                               select n.Port);
            portArray.Sort();

            // Поиск свободного UDP порта.
            for (int i = startPort; i < UInt16.MaxValue; i++)
            {
                if (!portArray.Contains(i))
                {
                    Log.DebugFormat("Найден свободный UDP порт: {0}.", i);
                    return i;
                }
            }

            Log.Error("Свободный UDP порт не найден!");
            return 0;
        }
    }
}
