using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TShirt.API.Data.Model;
using TShirt.API.Model;

namespace TShirt.API
{
    public interface ITShirtService
    {
        Task<Shirt> AddTShirt(Shirt shirt);
        Task<bool> UpdateShirt(Shirt shirt);
        Task<List<Shirt>> GetAllTShirts();
        Task<Shirt> GetTShirtById(int id);
        Task<List<Option>> GetAllSizes();
        Task<List<Option>> GetAllStyles();
        byte[] GetFile(string fileNameWithExtn);
        Shirt MapTShirtData(DataRow dr);
        Task<bool> DeleteTShirt(int tshirtId, int userId);
    }
}
