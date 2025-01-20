using LiteDB;
using NotificatorMobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Services
{
    public class NotificationService : INotificationService
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<Notification> _notificationCollection;
        public NotificationService(string databasePath, string collectionName)
        {
            _database = new LiteDatabase(databasePath);
            _notificationCollection = _database.GetCollection<Notification>(collectionName);
        }
        /// <summary>
        /// Add notification to the db
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public async Task Create(Notification notification)
        {
            try
            {
                await Task.Run(() => _notificationCollection.Insert(notification));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
            }
        }
        /// <summary>
        /// Delete notification from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            try
            {
                await Task.Run(() => _notificationCollection.Delete(id));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
            }
        }
        /// <summary>
        /// Get all notifications from db
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Notification>?> GetAll()
        {
            try
            {
                return await Task.Run(() => _notificationCollection.Query().ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
                return null;
            }
        }
        /// <summary>
        /// Get notification by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Notification?> GetById(int id)
        {
            try
            {
                return await Task.Run(() => _notificationCollection.FindById(id));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
                return null;
            }
        }
        /// <summary>
        /// Update notification in the db
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public async Task Update(Notification notification)
        {
            try
            {
                await Task.Run(() => _notificationCollection.Update(notification));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
            }
        }

        public void Register(Notification notification)
        {
            throw new NotImplementedException();
        }

        public void Cancel(int id)
        {
            throw new NotImplementedException();
        }

        private double GetSecondsDifference(DateTime dateTime)
        {
            return (DateTime.Now - dateTime).TotalSeconds;
        }
    }
}
