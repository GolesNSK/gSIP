using System;
using System.Collections.Concurrent;
using System.Threading;
using gSIP.Logger;
using log4net;

namespace gSIP.Common
{
    /// <summary>
    /// Класс реализующий потокобезопасную очередь для обмена данными между потоками.
    /// </summary>
    /// <typeparam name="T">Тип передаваемых данных.</typeparam>
    public class DataQueue<T>
    {
        /// <summary>
        /// Логгер для ведения журнала событий приложения.
        /// </summary>
        protected ILog Log = AppLogger.GetLogger("LOGGER");

        /// <summary>
        /// Потокобезопасная очередь объектов.
        /// </summary>
        private ConcurrentQueue<T> cQueue = new ConcurrentQueue<T>();

        /// <summary>
        /// Семафор для управления доступом в очередь из других потоков.
        /// </summary>
        private SemaphoreSlim semaphore = new SemaphoreSlim(0);

        /// <summary>
        /// Объект для отправки сигнала отмены.
        /// </summary>
        private CancellationTokenSource cancellTokenSrc = new CancellationTokenSource();

        /// <summary>
        /// Если очередь остановлена - true; если очередь работает - false.
        /// </summary>
        public bool IsStopped
        {
            get
            {
                return cancellTokenSrc.Token.IsCancellationRequested;
            }
        }

        /// <summary>
        /// Количество объектов в очереди.
        /// </summary>
        public int Count
        {
            get
            {
                return cQueue.Count;
            }
        }

        /// <summary>
        /// Добавляет объект в конец очереди.
        /// </summary>
        /// <param name="obj">Объект, добавляемый в конец очереди.</param>
        public void Enqueue(T item)
        {
            if (!IsStopped)
            {
                cQueue.Enqueue(item);
                try
                {
                    semaphore.Release();
                }
                catch (Exception ex)
                {
                    Log.Warn("Ошибка управления очередью DataQueue<T> при добавлении объекта.", ex);
                }
            }
        }

        /// <summary>
        /// Удаляет и возвращает объект, находящийся в начале потокобезопасной очереди.
        /// </summary>
        /// <param name="result">Параметр result, возвращаемый данным методом, содержит удаленный из начала очереди объект.
        /// Если объект, доступный для удаления, не найден, то метод блокируется до его появления.</param>
        public void Dequeue(out T result)
        {
            if (!IsStopped)
            {
                try
                {
                    semaphore.Wait(cancellTokenSrc.Token);
                }
                catch (Exception ex)
                {
                    Log.Warn("Ошибка управления очередью DataQueue<T> при выборке объекта.", ex);
                }
                
                cQueue.TryDequeue(out result);
            }
            else
            {
                result = default(T);
            }
        }

        /// <summary>
        /// Остановка работы очереди, снятие блокировок.
        /// </summary>
        public void Stop()
        {
            try
            {
                cancellTokenSrc.Cancel();
            }
            catch (Exception ex)
            {
                Log.Warn("Ошибка при попытке остановки работы очереди DataQueue<T>.", ex);
            }
        }
    }
}
