using ERPEC.Common;
using ERPEC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.DAL
{
    public class InvoiceDAL
    {
        public InvoiceDOM Save(SqlConnection SqlCon, SqlTransaction SqlTran, InvoiceDOM invoice)
        {
            Invoice_H_DAL _Invoice_H = new Invoice_H_DAL();
            Invoice_D_DAL _Invoice_D = new Invoice_D_DAL();
            StockMovementDAL _ST_M = new StockMovementDAL();
            StockMovement_Trans_DAL _SM_Trans = new StockMovement_Trans_DAL();

            InvoiceDOM _InvoiceH, _InvoiceD = new InvoiceDOM();
            StockMovementDOM _SM = new StockMovementDOM();
            StockMovementTransDOM _SMTrans = new StockMovementTransDOM();

            bool dispose = SqlCon == null;


            using (SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting))
            {
                if (dispose)
                {
                    SqlCon.Open();
                }

                using (SqlTransaction transaction = SqlCon.BeginTransaction())
                {
                    try
                    {
                        string Query = @"SELECT * FROM Invoice_H WHERE INV_H_Id = " + invoice.INV_HID;
                        DataTable DT_invoice_H = DBCon.GetData(SqlCon, transaction, Query, null);

                        if (DT_invoice_H != null && DT_invoice_H.Rows.Count > 0)
                        {
                            _InvoiceH = _Invoice_H.Save(SqlCon, transaction, invoice);
                            invoice.INV_HID = _InvoiceH.INV_HID;
                            _Invoice_D.Delete(SqlCon, transaction, _InvoiceH.INV_HID);
                            _InvoiceD = _Invoice_D.Save(SqlCon, transaction, invoice);
                        }
                        else
                        {
                            _InvoiceH = _Invoice_H.Save(SqlCon, transaction, invoice);
                            invoice.INV_HID = _InvoiceH.INV_HID;
                            invoice.INV_Code = _InvoiceH.INV_Code;
                            _InvoiceD = _Invoice_D.Save(SqlCon, transaction, invoice);

                            _InvoiceD.Items.ForEach(itm =>
                            {
                                _SM.SM_Type = Common.Properties.Database.ERP.SM_Types.INV;
                                _SM.SM_Item = itm.Item_ID;
                                _SM.SM_Location = _InvoiceH.INV_Location;
                                _SM.SM_Date = _InvoiceH.INV_Date;
                                _SM.SM_Bal = itm.Item_Qty;

                                _ST_M.Save(SqlCon, transaction, _SM,false);

                                _SMTrans.SMT_Item = itm.Item_ID;
                                _SMTrans.SMT_Qty = itm.Item_Qty;
                                _SMTrans.SMT_Location = _InvoiceH.INV_Location;
                                _SMTrans.SMT_Type = Common.Properties.Database.ERP.SM_Types.INV;
                                _SMTrans.SMT_Doc = _InvoiceH.INV_HID;
                                _SMTrans.SMT_Doc_No = _InvoiceH.INV_Code;

                                _SM_Trans.Save(SqlCon, transaction, _SMTrans);

                            });
                        }

                        transaction.Commit();
                    }
                    catch (Exception e1)
                    {
                        transaction.Rollback();
                        throw e1;
                    }
                }
            }
            return invoice;
        }

        public List<InvoiceDOM> GetAllInvoices(SqlConnection SqlCon, SqlTransaction SqlTran,DateTime? FromDate,DateTime? ToDate)
        {
            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            List<InvoiceDOM> InvoiceH = new List<InvoiceDOM>();

            string Query = @"SELECT * FROM Invoice_H ";
            if (FromDate != null && ToDate != null)
                Query += "WHERE INV_H_Date BETWEEN '" + FromDate.Value.ToString("yyyy/MM/dd") + "' AND '" + ToDate.Value.ToString("yyyy/MM/dd") + "'";

            DataTable DT_InvoiceH = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_InvoiceH != null && DT_InvoiceH.Rows.Count > 0)
            {
                DT_InvoiceH.AsEnumerable().ToList().ForEach(r =>
                {
                    InvoiceH.Add(new InvoiceDOM()
                    {
                        INV_HID = r.Field<long>("INV_H_Id"),
                        INV_Code = r.Field<string>("INV_H_Code"),
                        INV_Date = r.Field<DateTime>("INV_H_Date"),
                        INV_Location = r.Field<int>("INV_H_Location"),
                        INV_Value = r.Field<decimal>("INV_H_Amount"),
                        INV_H_User = r.Field<int>("INV_H_User")
                    });
                });
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return InvoiceH;
        }

        public InvoiceDOM SelectInvoice(SqlConnection SqlCon, SqlTransaction SqlTran, long INV_ID)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            InvoiceDOM InvoiceH = new InvoiceDOM();
            List<ItemGridDOM> ITM = new List<ItemGridDOM>();

            string Query = @"SELECT * FROM Invoice_H WHERE INV_H_Id=" + INV_ID;
            string Query_D = @"  SELECT INVD.*,IM.ITEM_code,IM.ITEM_name FROM Invoice_D INVD " +
                                 "JOIN ItemMaster IM ON " +
                                 "INVD.INV_Item_Id=IM.ITEM_id " +
                                 "WHERE INV_D_H_Id=" + INV_ID;

            DataTable DT_INVH = DBCon.GetData(SqlCon, SqlTran, Query, null);
            DataTable DT_INVD = DBCon.GetData(SqlCon, SqlTran, Query_D, null);

            InvoiceH.INV_HID = DT_INVH.Rows[0].Field<long>("INV_H_Id");
            InvoiceH.INV_Code = DT_INVH.Rows[0].Field<string>("INV_H_Code");
            InvoiceH.INV_Date = DT_INVH.Rows[0].Field<DateTime>("INV_H_Date");
            InvoiceH.INV_Location = DT_INVH.Rows[0].Field<int>("INV_H_Location");
            InvoiceH.INV_Value = DT_INVH.Rows[0].Field<decimal>("INV_H_Amount");;
            InvoiceH.INV_H_User = DT_INVH.Rows[0].Field<int>("INV_H_User");

            if (DT_INVD != null && DT_INVD.Rows.Count > 0)
            {
                DT_INVD.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("INV_Item_Id"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Qty = r.Field<decimal>("INV_Itm_Qty"),
                        Item_Price = r.Field<decimal>("INV_Itm_Rate"),
                        Item_Value = r.Field<decimal>("INV_Itm_Rate") * r.Field<decimal>("INV_Itm_Qty")
                    });
                });
                InvoiceH.Items = ITM;
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return InvoiceH;

        }

        public InvoiceDOM getFastomoving(SqlConnection SqlCon, SqlTransaction SqlTran, DateTime? FromDate, DateTime? ToDate)
        {

            bool dispose = SqlCon == null;
            SqlCon = SqlCon ?? new SqlConnection(Properties.Settings.Default.ConSetting);
            if (dispose)
            {
                SqlCon.Open();
            }

            InvoiceDOM InvoiceH = new InvoiceDOM();
            List<ItemGridDOM> ITM = new List<ItemGridDOM>();

            string Query = @"SELECT TOP 10 IM.ITEM_id,IM.ITEM_code,IM.ITEM_name,ID.INV_Itm_Qty FROM Invoice_H IH " +
                            "JOIN Invoice_D ID ON IH.INV_H_Id=ID.INV_D_H_Id " +
                            "JOIN ItemMaster IM ON ID.INV_Item_Id=IM.ITEM_id " +
                            "WHERE IH.INV_H_Date BETWEEN '" + FromDate.Value.ToString("yyyy/MM/dd") + "' AND '" +
                            ToDate.Value.ToString("yyyy/MM/dd") + "' ORDER BY ID.INV_Itm_Qty DESC";

            DataTable DT_Items = DBCon.GetData(SqlCon, SqlTran, Query, null);

            if (DT_Items != null && DT_Items.Rows.Count > 0)
            {
                DT_Items.AsEnumerable().ToList().ForEach(r =>
                {
                    ITM.Add(new ItemGridDOM()
                    {
                        Item_ID = r.Field<long>("ITEM_id"),
                        Item_Code = r.Field<string>("ITEM_code"),
                        Item_Name = r.Field<string>("ITEM_name"),
                        Item_Qty = r.Field<decimal>("INV_Itm_Qty")
                    });
                });
                InvoiceH.Items = ITM;
            }

            if (dispose)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }

            return InvoiceH;

        }

    



        



    }
}