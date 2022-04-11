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
    public class TShirtService : ITShirtService
    {
        string mainDirectory = Path.Combine(Directory.GetCurrentDirectory(), "images");

        private readonly DAL _dal;
        public TShirtService(DAL dal)
        {
            _dal = dal;
        }

        public async Task<Shirt> AddTShirt(Shirt shirt)
        {
            shirt.NewFileName = Guid.NewGuid().ToString();
            int shirtId = await _dal.AddTShirt(shirt);
            shirt.TShirtId = shirtId;
            if (!Directory.Exists(mainDirectory))
            {
                Directory.CreateDirectory(mainDirectory);
            }
            var path = Path.Combine(mainDirectory, $"{shirt.NewFileName}.{shirt.FileExtension}");
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await shirt.Image.CopyToAsync(stream);
                return shirt;
            } 
        }

        public async Task<bool> UpdateShirt(Shirt shirt)
        {
            bool isBoolean = false;

            var oldData = await GetTShirtById(shirt.TShirtId);

            shirt.NewFileName = shirt.Image != null ? Guid.NewGuid().ToString():oldData.NewFileName;
            int isUpdated = await _dal.UpdateTShirt(shirt);
            if(isUpdated > 0)
            {
                if (shirt.Image != null)
                {
                    var path = Path.Combine(mainDirectory, $"{shirt.NewFileName}.{shirt.FileExtension}");
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await shirt.Image.CopyToAsync(stream);

                        MoveToArchieve($"{oldData.NewFileName}.{oldData.FileExtension}");

                        isBoolean = true;
                    }
                }
            }
            return isBoolean;
        }

        public async Task<List<Option>> GetAllSizes()
        {
            DataTable dt = await _dal.GetAllSizes();
            List<Option> sizes = new List<Option>();
            foreach(DataRow dr in dt.Rows)
            {
                Option size = new Option
                {
                    Value = int.Parse(dr["SizeId"].ToString()),
                    Text = dr["SizeName"].ToString()
                };
                sizes.Add(size);
            }
            return sizes;
        }

        public async Task<List<Option>> GetAllStyles()
        {
            DataTable dt = await _dal.GetAllStyles();
            List<Option> styles = new List<Option>();
            foreach (DataRow dr in dt.Rows)
            {
                Option style = new Option
                {
                    Value = int.Parse(dr["StyleId"].ToString()),
                    Text = dr["StyleName"].ToString()
                };
                styles.Add(style);
            }
            return styles;
        }

        public async Task<List<Shirt>> GetAllTShirts()
        {
            DataTable dt = await _dal.GetAllTShirts();
            List<Shirt> shirts = new List<Shirt>();
            foreach (DataRow dr in dt.Rows)
            {
                Shirt shirt = MapTShirtData(dr);
                shirts.Add(shirt);
            }
            return shirts;
        }

        public Shirt MapTShirtData (DataRow dr)
        {
            Shirt shirt = new Shirt
            {
                TShirtId = int.Parse(dr["TShirtId"].ToString()),
                Gender = dr["Gender"].ToString(),
                Made = dr["Made"].ToString(),
                Price = decimal.Parse(dr["Price"].ToString()),
                Color = dr["Color"].ToString(),
                SizeId = int.Parse(dr["SizeId"].ToString()),
                SizeName = dr["SizeName"].ToString(),
                StyleId = int.Parse(dr["StyleId"].ToString()),
                StyleName = dr["StyleName"].ToString(),
                Description = dr["Description"].ToString(),
                ActualFileName = dr["ActualFileName"].ToString(),
                NewFileName = dr["NewFileName"].ToString(),
                FileExtension = dr["FileExtension"].ToString(),
                FileSizeInKB = int.Parse(dr["FileSizeInKB"].ToString()),
                CreatedByUserId = int.Parse(dr["CreatedByUserId"].ToString()),
                CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString()),
                ImageFileUrl = $"{dr["NewFileName"]}.{dr["FileExtension"]}"
            };
            return shirt;
        }

        public async Task<Shirt> GetTShirtById(int id)
        {
            DataTable dt = await _dal.GetTShirtById(id);
            DataRow dr = dt.Rows[0];
            Shirt shirt = MapTShirtData(dr);
            return shirt;
        }

        public byte[] GetFile(string fileNameWithExtn)
        {
            string path = Path.Combine(mainDirectory, fileNameWithExtn);
            byte[] b = System.IO.File.ReadAllBytes(path);
            return b;
        }

        public async Task<bool> DeleteTShirt(int tshirtId, int userId)
        {
            bool isDeleted = false;
            var tShirtData = await GetTShirtById(tshirtId);
            if (tShirtData != null)
            {
                if (await _dal.DeleteTShirt(tshirtId, userId))
                {
                    string fileName = $"{tShirtData.NewFileName}.{tShirtData.FileExtension}";
                    MoveToArchieve(fileName);
                    isDeleted = true;
                }
            }
            return isDeleted;
        }

        public void MoveToArchieve(string fileNameWithExtn)
        {
            string archieveFolder = Path.Combine(mainDirectory, "archieve");
            if (File.Exists(Path.Combine(mainDirectory, fileNameWithExtn)))
            {
                if(!Directory.Exists(archieveFolder))
                {
                    Directory.CreateDirectory(archieveFolder);
                }
                File.Move(Path.Combine(mainDirectory, fileNameWithExtn), Path.Combine(archieveFolder, fileNameWithExtn));
            }
        }
    }
}
