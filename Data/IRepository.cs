using OnlineQuee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuee.Data
{
    public interface IRepository
    {
        Setting LoadSettings();
        bool SaveSettings(Setting model);
        List<Category> GetCatigories();
        Category GetCatigoryById(int id);
        List<User> GetUsers(int categoryId);
        User GetUserById(int id);
        bool SaveCategories(List<Category> models);
    }
}
