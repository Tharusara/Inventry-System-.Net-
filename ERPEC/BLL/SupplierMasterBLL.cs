using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class SupplierMasterBLL
    {
        SupplierMasterDAL _SupplierMasterDAL = new SupplierMasterDAL();
        public SupplierMasterDOM saveSupplier(SupplierMasterDOM SupplierMaster)
        {
            return _SupplierMasterDAL.Save(null, null, SupplierMaster);
        }

        public void deleteSupplier(int LocationMasterId)
        {
            _SupplierMasterDAL.Delete(null, null, LocationMasterId);
        }

        public List<SupplierMasterDOM> getAllSuppliers(bool? IsActive)
        {
            return _SupplierMasterDAL.GetAllSuppliers(null, null, IsActive);
        }

        public List<SupplierMasterDOM> getSupplierCode()
        {
            return _SupplierMasterDAL.GetSupplierCode(null, null);
        }
    }
}