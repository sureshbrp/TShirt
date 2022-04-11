using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TShirt.API.Model;

namespace TShirt.API
{
    public class DAL
    {
        private readonly SqlHelper _helper;
        public DAL(SqlHelper helper)
        {
            _helper = helper;
        }

        public async Task<int> AddTShirt(Shirt shirt)
        {
            SqlParameter[] sParams =
                {
                    new SqlParameter("@Gender", shirt.Gender),
                    new SqlParameter("@Made", shirt.Made),
                    new SqlParameter("@Price",shirt.Price),
                    new SqlParameter("@Color", shirt.Color),
                    new SqlParameter("@SizeId", shirt.SizeId),
                    new SqlParameter("@StyleId", shirt.StyleId),
                    new SqlParameter("@Description", shirt.Description),
                    new SqlParameter("@ActualFileName", shirt.ActualFileName),
                    new SqlParameter("@NewFileName", shirt.NewFileName),
                    new SqlParameter("@FileExtension", shirt.FileExtension),
                    new SqlParameter("@FileSizeInKB", shirt.FileSizeInKB),
                    new SqlParameter("@CreatedByUserId", shirt.CreatedByUserId),
                };
            return await _helper.ExecuteNonQueryAsync("AddTShirt",sParams);
        }

        public async Task<DataTable> GetAllTShirts()
        {
            return await _helper.ExecuteDataTableAsync("GetAllTShirt", "dtTShirts");
        }

        public async Task<DataTable> GetTShirtById(int id)
        {
           SqlParameter[] sParams = {new SqlParameter("@TShirtdId", id)};
           return await _helper.ExecuteDataTableAsync("GetTShirtById", sParams);
        }

        public async Task<DataTable> GetAllSizes()
        {
            return await _helper.ExecuteDataTableAsync("GetSizes", "dtSizes");
        }

        public async Task<DataTable> GetAllStyles()
        {
            return await _helper.ExecuteDataTableAsync("GetStyles", "dtStyles");
        }

        public async Task<bool> DeleteTShirt(int tshirtId, int userId)
        {
            SqlParameter[] sParams = { 
                            new SqlParameter("@TShirtId", tshirtId) ,
                            new SqlParameter("@UserId", userId) 
                        };
            return await _helper.ExecuteNonQueryAsync("DeleteTShirt", sParams) == 1 ? true : false;
        }

        public async Task<int> UpdateTShirt(Shirt shirt)
        {
            try
            {
                SqlParameter[] sParams =
                    {
                    new SqlParameter("@TShirtId", shirt.TShirtId),
                    new SqlParameter("@Gender", shirt.Gender),
                    new SqlParameter("@Made", shirt.Made),
                    new SqlParameter("@Price",shirt.Price),
                    new SqlParameter("@Color", shirt.Color),
                    new SqlParameter("@SizeId", shirt.SizeId),
                    new SqlParameter("@StyleId", shirt.StyleId),
                    new SqlParameter("@Description", shirt.Description),
                    new SqlParameter("@ActualFileName", shirt.ActualFileName),
                    new SqlParameter("@NewFileName", shirt.NewFileName),
                    new SqlParameter("@FileExtension", shirt.FileExtension),
                    new SqlParameter("@FileSizeInKB", shirt.FileSizeInKB),
                    new SqlParameter("@CreatedByUserId", shirt.CreatedByUserId),
                };
                return await _helper.ExecuteNonQueryAsync("UpdateTShirt", sParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
