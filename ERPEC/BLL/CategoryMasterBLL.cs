using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class CategoryMasterBLL
    {
        CategoryMasterDAL _CategoryMasterDAL = new CategoryMasterDAL();
        public CategoryMasterDOM saveSupplier(CategoryMasterDOM CategoryMaster)
        {
            return _CategoryMasterDAL.Save(null, null, CategoryMaster);
        }

        public void deleteSupplier(int LocationMasterId)
        {
            _CategoryMasterDAL.Delete(null, null, LocationMasterId);
        }

        public List<CategoryMasterDOM> geAllCategories(long? cat_id)
        {
            return _CategoryMasterDAL.GetAllCategories(null, null, cat_id);
        }

        public string getCatagoryName(int cat_id)
        {
            return _CategoryMasterDAL.getCatagory(null, null, cat_id);
        }
    }
}