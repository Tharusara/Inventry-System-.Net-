using ERPEC.DAL;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPEC.BLL
{
    public class ItemMasterBLL
    {
        ItemMasterDAL _ItemMasterDAL = new ItemMasterDAL();
        public ItemMasterDOM saveItem(ItemMasterDOM ItemMaster)
        {
            return _ItemMasterDAL.Save(null, null, ItemMaster);
        }

        public void deleteItem(int ItemID)
        {
            _ItemMasterDAL.Delete(null, null, ItemID);
        }

        public List<ItemMasterDOM> getAll(bool?IsActive,long? ItemID,string ItemCode)
        {
            return _ItemMasterDAL.GetAll(null, null, IsActive, ItemID, ItemCode);
        }

        public List<ItemMasterDOM> getActive()
        {
            return _ItemMasterDAL.GetActive(null, null);
        }

        public List<ItemGridDOM> loadAvalibleItems(int Loc, long? item_id, string item_code)
        {
            return _ItemMasterDAL.getItems(null, null, Loc, item_id, item_code);
        }

        public List<ItemGridDOM> loadReOrderItems(int? Loc)
        {
            return _ItemMasterDAL.getReorderItems(null, null, Loc);
        }

       
        
    }
}