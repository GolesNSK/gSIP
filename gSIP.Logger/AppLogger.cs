using log4net;
using log4net.Config;
using System;
using System.Reflection;

namespace gSIP.Logger
{
    /// <summary>
    /// Статический класс для ведения журнала событий приложения.
    /// </summary>
    public class AppLogger
    {
        /// <summary>
        /// Логгер по умолчанию (должен присутствовать в секции настроек log4net файла App.config
        /// </summary>
        private const string DEFAULT_LOGER = "LOGGER";

        /// <summary>
        /// Конфигурационный файл log4net.
        /// </summary>
        private const string CONFIG_FILE = ".\\Log4net.config";

        /// <summary>
        /// Внутренняя переменная для хранения логгера по умолчанию.
        /// </summary>
        private static ILog dl = null;

        /// <summary>
        /// Возвращает логгер по умолчанию, используется для обеспечения функции ведения журнала работы приложения.
        /// </summary>
        public static ILog DefaultLogger
        {
            get { return dl; }
        }

        /// <summary>
        /// Статический конструктор для инициализации объекта Logger.
        /// </summary>
        static AppLogger()
        {
            if (dl == null)
            {
                // Загрузка конфигурации из файла App.config для инициализации библиотеки log4net
                XmlConfigurator.Configure(new System.IO.FileInfo(CONFIG_FILE));


                // Логгер по умолчанию
                dl = LogManager.GetLogger(DEFAULT_LOGER);
                    dl.InfoFormat("Start application {0} v{1}",
                    Assembly.GetExecutingAssembly().GetName().Name,
                    Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }

        /// <summary>
        /// Функция для получения произвольного логгера по названию.
        /// </summary>
        /// <param name="name">Наименование логгера.</param>
        /// <returns></returns>
        public static ILog GetLogger(string name)
        {
            ILog logger = null;

            if (dl != null)
            {
                try
                {
                    logger = LogManager.GetLogger(name);
                }
                catch (Exception ex)
                {
                    // Если не удалось инициировать логгер
                    dl.Error("Ошибка инициализации логгера " + name, ex);
                    dl.ErrorFormat("В качестве логгера {0} будет использоваться логгер по умолчанию {1}", name, DEFAULT_LOGER);
                    logger = dl;
                }
            }
            return logger;
        }
    }
}
