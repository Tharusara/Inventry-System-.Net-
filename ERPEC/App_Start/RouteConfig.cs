using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERPEC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //LocationMaster
            #region LocationMaster
            routes.MapRoute(
              name: "LocationMaster",
              url: "locations",
              defaults: new { controller = "LocationMaster", action = "Index" }
          );

            routes.MapRoute(
             name: "SaveLocation",
             url: "locations/save",
             defaults: new { controller = "LocationMaster", action = "SaveLocation" }
         );

            routes.MapRoute(
             name: "GetAllLocation",
             url: "locations/getall",
             defaults: new { controller = "LocationMaster", action = "GetLocations" }
         );

            routes.MapRoute(
             name: "GetAllUsers",
             url: "users/getall",
             defaults: new { controller = "LocationMaster", action = "getUsers" }
         );
            #endregion

            #region SupplierMaster

            routes.MapRoute(
              name: "SupplierMaster",
              url: "suppliers",
              defaults: new { controller = "SupplierMaster", action = "Index" }
          );

            routes.MapRoute(
              name: "SaveSupplier",
              url: "suppliers/save",
              defaults: new { controller = "SupplierMaster", action = "SaveSupplier" }
          );

            routes.MapRoute(
             name: "GetAllSupplier",
             url: "suppliers/gelall",
             defaults: new { controller = "SupplierMaster", action = "GetSuppliers" }
         );

            #endregion

            #region CategoryMaster
            routes.MapRoute(
             name: "CategoryMaster",
             url: "categories",
             defaults: new { controller = "CategoryMaster", action = "Index" }
         );
            routes.MapRoute(
              name: "SaveCatagory",
              url: "categories/save",
              defaults: new { controller = "CategoryMaster", action = "SaveCatagoty" }
          );
            routes.MapRoute(
            name: "GetAllCatagories",
            url: "categories/gelall",
            defaults: new { controller = "CategoryMaster", action = "GetCatagories" }
        );
            #endregion

            #region ItemMaster

            routes.MapRoute(
             name: "ItemMaster",
             url: "items",
             defaults: new { controller = "ItemMaster", action = "Index" }
         );
            routes.MapRoute(
             name: "GetAllItems",
             url: "items/getall",
             defaults: new { controller = "ItemMaster", action = "GetAllItems" }
         );
            routes.MapRoute(
             name: "SaveItem",
             url: "items/save",
             defaults: new { controller = "ItemMaster", action = "SaveItem" }
         );
            

            #endregion

            #region GRN
            routes.MapRoute(
             name: "GRN",
             url: "grns",
             defaults: new { controller = "GRN", action = "Index" }
         );
            routes.MapRoute(
             name: "GRNSave",
             url: "grn/save",
             defaults: new { controller = "GRN", action = "SaveGRN" }
         );
            routes.MapRoute(
            name: "GRNGetAll",
            url: "grn/getall",
            defaults: new { controller = "GRN", action = "getAllGRN" }
        );

            routes.MapRoute(
            name: "SelectGRN",
            url: "grn/select",
            defaults: new { controller = "GRN", action = "selectGRN" }
        );
           
            #endregion

            #region PO
            routes.MapRoute(
             name: "PO",
             url: "poders",
             defaults: new { controller = "PurchaseOrder", action = "Index" }
         );

            routes.MapRoute(
             name: "POSave",
             url: "poders/save",
             defaults: new { controller = "PurchaseOrder", action = "SavePO" } 
         );

            routes.MapRoute(
            name: "POGetAll",
            url: "poders/getall",
            defaults: new { controller = "PurchaseOrder", action = "getAllPO" }
        );

            routes.MapRoute(
            name: "SelectPO",
            url: "poders/select",
            defaults: new { controller = "PurchaseOrder", action = "selectPO" }
        );


            #endregion

            #region LT
            routes.MapRoute(
             name: "LT",
             url: "lts",
             defaults: new { controller = "LocationTransfer", action = "Index" }
         );

            routes.MapRoute(
            name: "LTSave",
            url: "lt/save",
            defaults: new { controller = "LocationTransfer", action = "SaveLT" }
        );

            routes.MapRoute(
            name: "LTGetAll",
            url: "lt/getall",
            defaults: new { controller = "LocationTransfer", action = "getAllLT" }
        );

            routes.MapRoute(
            name: "SelectLT",
            url: "lt/select",
            defaults: new { controller = "LocationTransfer", action = "selectLT" }
        );

            #endregion

            #region Invoice
            routes.MapRoute(
             name: "Invoice",
             url: "invoices",
             defaults: new { controller = "Invoice", action = "Index" }
         );

            routes.MapRoute(
             name: "InvoiceSave",
             url: "invoice/save",
             defaults: new { controller = "Invoice", action = "SaveInvoice" }
         );
                        
            routes.MapRoute(
            name: "InvoiceGetAll",
            url: "invoice/getall",
            defaults: new { controller = "Invoice", action = "getAllInvoices" }
        );

            routes.MapRoute(
            name: "SelectInvoice",
            url: "invoice/select",
            defaults: new { controller = "Invoice", action = "selectInvoice" }
        );

            routes.MapRoute(
             name: "InvoiceSale",
             url: "invoices/sale",
             defaults: new { controller = "Invoice", action = "Sales" }
         );

            routes.MapRoute(
             name: "InvoiceFastMoiving",
             url: "invoices/fastmoving",
             defaults: new { controller = "Invoice", action = "getFastMoivingItems" }
         );
            #endregion

            #region Reorder
            routes.MapRoute(
            name: "Reorder",
            url: "reorders",
            defaults: new { controller = "Reorder", action = "Index", active = true }
        );
            routes.MapRoute(
            name: "GetReorderItems",
            url: "reorder/getItems",
            defaults: new { controller = "Reorder", action = "loadReorderItems", active = true }
        );
            routes.MapRoute(
            name: "ReorderSave",
            url: "reorder/save",
            defaults: new { controller = "Reorder", action = "SaveRO", active = true }
        );
            routes.MapRoute(
           name: "ReorderGetAll",
           url: "reorder/getall",
           defaults: new { controller = "Reorder", action = "getAllReordes", active = true }
       );
            routes.MapRoute(
           name: "SelectReorder",
           url: "reorder/selectreorder",
           defaults: new { controller = "Reorder", action = "selectRO", active = true }
       );
            #endregion

            #region Comman
            routes.MapRoute(
            name: "GetItemActive",
            url: "comman/actItems",
            defaults: new { controller = "ItemMaster", action = "GetAllItems", active = true }
        );
            routes.MapRoute(
            name: "GetLocationActive",
            url: "comman/actLocation",
            defaults: new { controller = "LocationMaster", action = "GetLocations", IsActive = true }
        );
            routes.MapRoute(
            name: "GetSuppliersActive",
            url: "comman/actSuppliers",
            defaults: new { controller = "SupplierMaster", action = "GetSuppliers", IsActive = true }
        );
            routes.MapRoute(
            name: "GetItemByCode",
            url: "comman/getitembycode",
            defaults: new { controller = "ItemMaster", action = "GetAllItems" }
        );
           routes.MapRoute(
           name: "LoadItems",
           url: "comman/loaditems",
           defaults: new { controller = "ItemMaster", action = "loadItems" }
       );
                       
            #endregion

            #region Reports

           routes.MapRoute(
           name: "BinCard",
           url: "bincard",
           defaults: new { controller = "BinCard", action = "Index" }
       );
           routes.MapRoute(
           name: "GetBinCard",
           url: "bincard/get",
           defaults: new { controller = "BinCard", action = "GetBinData" }
       );

           routes.MapRoute(
           name: "GetStockDetals",
           url: "stockdetails",
           defaults: new { controller = "StockDetails", action = "Index" }
       );
           routes.MapRoute(
           name: "GetStockItems",
           url: "stockdetails/getitems",
           defaults: new { controller = "StockDetails", action = "GetStockItems" }
       );
            #endregion

            #region Login
           routes.MapRoute(
           name: "Logins",
           url: "login",
           defaults: new { controller = "Login", action = "Index" }
         );
           routes.MapRoute(
           name: "LoginCheck",
           url: "login/check",
           defaults: new { controller = "Login", action = "SubmitLogin" }
         );
           routes.MapRoute(
           name: "LogOut",
           url: "logout",
           defaults: new { controller = "Login", action = "Logout" }
         );
#endregion

            #region User
           routes.MapRoute(
           name: "User",
           url: "users",
           defaults: new { controller = "UserMaster", action = "Index" }
         );
           routes.MapRoute(
           name: "SaveUser",
           url: "users/save",
           defaults: new { controller = "UserMaster", action = "SaveUser" }
         );
           routes.MapRoute(
           name: "GetUsers",
           url: "users/get",
           defaults: new { controller = "UserMaster", action = "GetUsers" }
         );

           #endregion

           //routes.MapRoute(
           //    name: "Default",
           //    url: "{controller}/{action}/{id}",
           //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           //);

           routes.MapRoute(
               name: "Default",
               url: "",
               defaults: new { controller = "Login", action = "Index"}
           );

           

           

            


        }
    }
}
