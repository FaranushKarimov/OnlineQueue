

using OnlineQuee.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;



namespace OnlineQuee.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public Repository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public Repository()
        {
            _applicationDbContext = null;
        }

        public Setting LoadSettings()
        {
            try
            {
            ApplicationDbContext applicationDbContext;
            applicationDbContext = _applicationDbContext ?? new ApplicationDbContext();
            return (from settings in applicationDbContext.Settings select settings)
                    .OrderBy(x => x.Id)
                    .Last();
            }catch (Exception ex)
            {
                Utils.Logger.CoreLogger.ErrorException("DB, LoadSettings got exception", ex);
                return null;
            }
        }

        public bool SaveSettings(Setting model)
        {
            try
            {
                ApplicationDbContext applicationDbContext;
                applicationDbContext = _applicationDbContext ?? new ApplicationDbContext();
                applicationDbContext.Add(model);
                applicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Utils.Logger.CoreLogger.ErrorException("DB, SaveSettings got exception", ex);
            }
            return true;
        }

        public List<Category> GetCatigories()
        {
            try
            {
                ApplicationDbContext applicationDbContext;
                applicationDbContext = _applicationDbContext ?? new ApplicationDbContext();
                return applicationDbContext.Categories.OrderBy(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                Utils.Logger.CoreLogger.ErrorException("DB, GetCatigories got exception", ex);
            }
            return null;
        }
        public Category GetCatigoryById(int id)
        {
            try
            {
                ApplicationDbContext applicationDbContext;
                applicationDbContext = _applicationDbContext ?? new ApplicationDbContext();
                return applicationDbContext.Categories.Where(x=>x.Id == id).First();
            }
            catch (Exception ex)
            {
                Utils.Logger.CoreLogger.ErrorException("DB, GetCatigoryById got exception", ex);
            }
            return null;
        }

        public bool SaveCategories(List<Category> models)
        {
            ApplicationDbContext applicationDbContext;
            applicationDbContext = _applicationDbContext ?? new ApplicationDbContext();
            var trancastion = applicationDbContext.Database.BeginTransaction();

            try
            {
                applicationDbContext.Database.ExecuteSqlInterpolated($"DELETE FROM Categories"); //ExecuteSqlCommand("DELETE FROM Categories");
                applicationDbContext.Database.ExecuteSqlInterpolated($"DELETE FROM Users");
                applicationDbContext.AddRange(models);
                applicationDbContext.SaveChanges();
                trancastion.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trancastion.Rollback();
                Utils.Logger.CoreLogger.ErrorException("DB, SaveCategories got exception", ex);
            }
            return false;
        }
        public List<User> GetUsers(int categoryId)
        {
            try
            {
                ApplicationDbContext applicationDbContext;
                applicationDbContext = _applicationDbContext ?? new ApplicationDbContext();
                return applicationDbContext.Users
                 .Where(x => x.CategoryId == categoryId)
                 .OrderBy(x => x.Id)
                 .ToList();
            }
            catch (Exception ex)
            {
                Utils.Logger.CoreLogger.ErrorException("DB, GetUsers got exception", ex);
                return null;
            }
        }
        public User GetUserById(int id)
        {
            try
            {
                ApplicationDbContext applicationDbContext;
                applicationDbContext = _applicationDbContext ?? new ApplicationDbContext();
                return applicationDbContext.Users
                 .Where(x => x.Id == id)
                 .First();
            }
            catch (Exception ex)
            {
                Utils.Logger.CoreLogger.ErrorException("DB, GetUserById got exception", ex);
            }
            return null;
        }

    }
}
