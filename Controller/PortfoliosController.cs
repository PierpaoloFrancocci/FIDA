using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Caching;
using FidaWorkstation.Professional.ObjectModel;
using FidaWorkstation.Professional.Web.Models;
using FidaWorkstation.Professional.Web.Utils;
using FidaWorkstation.Professional.Data.Service.Interfaces;

using FidaWorkstation.Professional.Common;
using log4net;


namespace FidaWorkstation.Professional.Web.Controllers
{
    public class DynamicValidationAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            var valueProvider = filterContext.Controller.ValueProvider;
            var keysWithNoIncomingValue = modelState.Keys.Where(x => !valueProvider.ContainsPrefix(x));
            foreach (var key in keysWithNoIncomingValue)
                modelState[key].Errors.Clear();
        }
    }
    [Utils.FundDBInCache]
    [Authorize]
    public class PortfoliosController : Controller
    {
        //
        // GET: /Portfolios/

        private const string SessionKeyCustomer = "PortfoglioController_Customer";
        private const string SessionKeyCurrentPage = "PortfoglioController_CurrentPage";
        private const string SessionKeyPortfogli = "PortfoglioController_Portfogli";
        private const string SessionKeyDati = "PortfoglioController_Dati";
        private const string SessionTreeKeyDati = "PortfoglioController_TreeDati";
        
        

        private static readonly ILog logger = LogManager.GetLogger(typeof(PortfoliosController));

       

        private int CurrentPage
        {
            get
            {
                if (Session[SessionKeyCurrentPage] == null)
                {
                    Session[SessionKeyCurrentPage] = 0;
                }
                return int.Parse(Session[SessionKeyCurrentPage].ToString());
            }
            set
            {
                Session[SessionKeyCurrentPage] = value;
            }
        }

        private List<MembershipUser> customers
        {
            get
            {
                if (Session[SessionKeyCustomer] == null)
                {
                    Session[SessionKeyCustomer] = new List<MembershipUser> { };
                }
                return Session[SessionKeyCustomer] as List<MembershipUser>;
            }
            set
            {
                Session[SessionKeyCustomer] = value;
            }
        }
        private PortFoglioViewModel dati
        {
            get
            {
                if (Session[SessionKeyDati] == null)
                {
                    Session[SessionKeyDati] = new PortFoglioViewModel { };
                }
                return Session[SessionKeyDati] as PortFoglioViewModel;
            }
            set
            {
                Session[SessionKeyDati] = value;
            }
        }
        private pfoglioTreeDTO treeData
        {
            get
            {
                if (Session[SessionTreeKeyDati] == null)
                {
                    Session[SessionTreeKeyDati] = new pfoglioTreeDTO { };
                }
                return Session[SessionTreeKeyDati] as pfoglioTreeDTO;
            }
            set
            {
                Session[SessionTreeKeyDati] = value;
            }
        
        }

        private void generaViewData()
        {
            List<DDLItem> lTipi =
            new List<DDLItem>{
                        new DDLItem { ID = 1, DESC= "Tipo 1" },
                        new DDLItem { ID = 2, DESC= "Tipo 2" },
                        new DDLItem { ID = 3, DESC= "Tipo 3" }};

            SelectList _type = new SelectList(lTipi, "ID", "DESC");

            ViewData["_type"] = _type;

            List<DDLItem> lcur =
            new List<DDLItem>{
                        new DDLItem { ID = 1, DESC= "EUR" },
                        new DDLItem { ID = 2, DESC= "USD" },
                        new DDLItem { ID = 3, DESC= "GBP" },
                        new DDLItem { ID = 4, DESC= "CHF" }
                        };

            SelectList _currency = new SelectList(lcur, "ID", "DESC");
            ViewData["_currency"] = _currency;


        }



        private InterviewPortfoglioViewModel _editModel;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //if (((System.Web.Mvc.ReflectedActionDescriptor)filterContext.ActionDescriptor).ActionName == "Edit")
            //{

            //    _editModel = (Request.Form["Edit_Model"].Deserialize() ?? TempData["Edit_Model"]
            //        ?? new InterviewPortfoglioViewModel()) as InterviewPortfoglioViewModel;

            //    TryUpdateModel(_editModel);
            //}
            //else 
            //{ 


            base.OnActionExecuting(filterContext);

            GenericFunction.Log("OnActionExecuting", filterContext.RouteData);
            //}

            


        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //string ActionName=((RedirectToRouteResult)(filterContext.Result)).RouteValues["action"].ToString();
            //string EditName = "Edit";
            //if (ActionName==EditName)

            base.OnResultExecuted(filterContext);
            string sDbg = filterContext.Controller.TempData["DebugTrc"] as string;
            System.Diagnostics.Debug.WriteLine("OnResultExecuted " + sDbg);

            GenericFunction.Log("OnResultExecuted", filterContext.RouteData);

            //if (((System.Web.Mvc.ViewResultBase)(filterContext.Result)).ViewName == "Edit")
            //{
            //    //if (((System.Web.Mvc.ReflectedActionDescriptor)filterContext.ActionDescriptor).ActionName == "Edit")
            //    //{
            //    if (filterContext.Result is RedirectToRouteResult)
            //        TempData["Edit_Model"] = _editModel;
            //    //}
            //    //else
            //    //{ base.OnResultExecuted(filterContext); }
            //}
            //else
            //{
            //    base.OnResultExecuted(filterContext); 
            //}
        }


        
        private void GetDati()
        {


            PortFoglio pf = new PortFoglio();

            MembershipUserCollection customers = new MembershipUserCollection();


            //foreach (var ute in Roles.GetUsersInRole("Clienti")) members.Add(Membership.GetUser(ute));
            foreach (var ute in Roles.FindUsersInRole(Utils.GenericFunction.BuildCustomerRoleName(pf.getCustomerRoleSuffix()),
                User.Identity.Name + "#%")) customers.Add(Membership.GetUser(ute));



            CustomersListViewModel myModel = new CustomersListViewModel();
            myModel.MembershipCollection = (MembershipUserCollection)customers;
            myModel.ParentUserName = User.Identity.Name;


            foreach (MembershipUser item in myModel.MembershipCollection)
            {
                UserProfile up = UserProfile.GetUserProfile(item.ProviderUserKey.ToString());
                //string mUserName = item.UserName.Remove(0, myModel.ParentUserName.Length + 1);

            }


            //foreach (var ute in Roles.GetUsersInRole("Clienti"))
            //    customers.Add(Membership.GetUser(ute));

            System.Guid g = System.Guid.Empty;

            PortFoglioViewModel pfVM = new PortFoglioViewModel();
            foreach (MembershipUser mu in myModel.MembershipCollection)
            {
                UserProfile up = UserProfile.GetUserProfile(mu.UserName);
                //UserProfile up = UserProfile.GetUserProfile(mu.ProviderUserKey.ToString());
                //System.Web.Security.MembershipUser memberUser = System.Web.Security.Membership.GetUser(this.User.Identity.Name);
                //ViewData["User_ProviderUserKey"] = memberUser.ProviderUserKey.ToString();
                if (up.Cognome != string.Empty)
                {
                    //g = new Guid(mu.UserName.Remove(0, myModel.ParentUserName.Length + 1));


                    
                    g = new Guid(mu.ProviderUserKey.ToString());
                    //"32963047-d1b0-4d41-9390-ee2a31ce9cd6"
                    pfoglioDTO pfDTO = new pfoglioDTO();
                    pfDTO.gUserID = g;
                    pfDTO.up = up;

                    IList<FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioInfoDTO> lpf =
                        pf.getPortfoglio(g);

                    pfDTO.lp = lpf;

                    pfVM.portfoglioDTO.Add(pfDTO);
                }

            }

            dati = pfVM;

            generaViewData();


        }


        public ActionResult Index(int? PFB, string DaProfiloCliente_userId)
        {
            if (TempData["ViewData"] != null)
            {
                foreach (KeyValuePair<string, object> kvp in (IEnumerable<KeyValuePair<string, object>>)TempData["ViewData"])
                {
                    ViewData.Add(kvp);
                }
            }
            if (PFB != null) ViewData["PortfolioBenchmark"] = PFB;
            if (DaProfiloCliente_userId != null) ViewData["DaProfiloCliente_userId"] = DaProfiloCliente_userId.ToString();
            TempData["ViewData"] = ViewData;

            return RedirectToAction("Show");
        }

        public ActionResult Show(int? page)
        {
            if (TempData["ViewData"] != null)
            {
                foreach (KeyValuePair<string, object> kvp in (IEnumerable<KeyValuePair<string, object>>)TempData["ViewData"])
                {
                    ViewData.Add(kvp);
                }
            }
            CurrentPage = page.HasValue ? page.Value : CurrentPage;
            //CurrentPage = page;
            GetDati();


            //GridViewData<MembershipUser> viewData = new GridViewData<MembershipUser>
            //{
            //    PagedList = customers.ToPagedList<MembershipUser>(CurrentPage, 4)
            //};

            //return View("Index", viewData);
            ViewData["CurrentPage"] = CurrentPage;
            return View("Index", dati);

        }
        public ActionResult Test()
        {


            return View();
        }


        //
        // GET: /Portfolios/Details/5

        public ActionResult Details(int id)
        {


            return View("Details");
        }

        //
        // GET: /Portfolios/Create

        [HttpPost]
        [DynamicValidationAttribute]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Interview");
                }

                generaViewData();
                FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioInfoDTO pf = new FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioInfoDTO();


                //UpdateModel(pf, Request.Form.AllKeys);
                pf._portfolioName = Request.Form["NamePortfoglio"];
                pf._portfolioDescription = Request.Form["DescriptionPortfoglio"];
                pf._type = Convert.ToInt32(collection["TipoPortfoglio"]);
                pf._portfolioName = Request.Form["NamePortfoglio"];

                if (Request.Form.AllKeys.Contains("StartDate"))
                    pf._startDate = Convert.ToDateTime(Request.Form["StartDate"]);
                else
                    pf._startDate = DateTime.Now;

                //pf._type = Convert.ToInt32(Request.Form["_type"]);

                pf._currency =
                    (from e in (SelectList)ViewData["_currency"]
                     where e.Value == Request.Form["Currency"]
                     select e.Text).Single<string>();
                pf._riequilibrio = Convert.ToInt32(collection["PeriodoBilanciamento"]);


                System.Guid g = System.Guid.Empty;

                pf._userId = new System.Guid(Request.Form["UserID"]);

                ////pf._portfolioName = "_portfolioName " + name;
                ////pf._portfolioDescription = "_portfolioDescription " + name;
                ////pf._currency = "EUR";
                ////pf._startDate = DateTime.Now;
                ////pf._type = 1;
                ////pf._userId = userID;

                PortFoglio pof = new PortFoglio();
                Int64 idPf = pof.CreatePTF(pf);

                //return RedirectToAction("Show");
                //return View("Struttura");
                if (collection.AllKeys.Contains("PortfolioBenchmark"))
                {
                    ViewData["PortfolioBenchmark"] = collection["PortfolioBenchmark"];
                    TempData["ViewData"] = ViewData;
                }
                return RedirectToAction("Struttura", new { id = idPf, gUserID = Request.Form["UserID"], TipoP = collection["hiddenTipoPortfoglio"] });
            }
            catch (Exception ex)
            {
                ViewData.ModelState.AddModelError("_FORM", ex.Message);
                var viewModel = new InterviewPortfoglioViewModel();

                viewModel.UserID = collection["UserId"];

                this.ViewData.Model = viewModel;
                return View("Interview");
            }
        }



        //
        // GET: /Portfolios/Edit/5

        public ActionResult Edit(long id, string gUserID, int? PFB)
        {
            if (PFB != null) ViewData["PortfolioBenchmark"] = PFB; 
            

            PortFoglio pf = new PortFoglio();
            System.Guid g = new Guid(gUserID);

            FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioDTO pfDTO = pf.GetInfoPTF(g, id);

            InterviewPortfoglioViewModel pfvm = new InterviewPortfoglioViewModel();
            pfvm.Currency = pfDTO._portfolioInfo._currency;
            pfvm.NamePortfoglio = pfDTO._portfolioInfo._portfolioName;
            pfvm.DescriptionPortfoglio = pfDTO._portfolioInfo._portfolioDescription;

            pfvm.StartDate = pfDTO._portfolioInfo._startDate;
            pfvm.TipoPortfoglio = pfDTO._portfolioInfo._type;
            pfvm.UserID = g.ToString();
            pfvm.PortfoglioID = pfDTO._portfolioInfo._portfolioId;
            pfvm.PeriodoBilanciamento = pfDTO._portfolioInfo._riequilibrio.ToString();

            return View("Edit", pfvm);
        }

        //
        // POST: /Portfolios/Edit/5

        [HttpPost]
        public ActionResult Edit(string submitButton, int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                PortFoglio pf = new PortFoglio();


                System.Guid g = System.Guid.Empty;

                g = new System.Guid(Request.Form["UserID"]);
                pf.updatePTF(g, id, collection);

                string TipoPort = collection["hiddenTipoPortfoglio"].ToString();
                if (TipoPort.IndexOf("Tipo ") >= 0)
                    TipoPort = TipoPort.Substring(5, TipoPort.Length - 5);

                if (collection.AllKeys.Contains("PortfolioBenchmark"))
                {
                    ViewData["PortfolioBenchmark"] = collection["PortfolioBenchmark"];
                    TempData["ViewData"] = ViewData;
                }

                if (submitButton == "Successivo")
                {
                    
                    return RedirectToAction("Struttura", new { id = id, gUserID = Request.Form["UserID"], TipoP = TipoPort });
                }
                else
                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        private void GetFundPicking(long id, string gUserID, string TipoP)
        {
            System.Web.Security.MembershipUser memberUser = System.Web.Security.Membership.GetUser(this.User.Identity.Name);

            ViewData["PromotoreID"] = memberUser.ProviderUserKey.ToString();

            System.Guid g = new Guid(ViewData["PromotoreID"].ToString());
            ViewData["UserID"] = gUserID;
            ViewData["PortfoglioID"] = id.ToString();
            ViewData["TipoPortfoglio"] = TipoP;

            FundPicking fp = new FundPicking();
            
            try
            {
                FundSelectorViewModel fsvm = (FundSelectorViewModel)HttpContext.Cache["funds.db"];
                IEnumerable<GroupResult<FundSelectorDTO>> resultG = fp.getFundPickerGroupBy(g, fsvm);
                ViewData["FundPicking"] = resultG;
            }
            catch (Exception ex)
            {
                logger.Error("Impossibile caricare la base dati fondi dalla cache", ex);
            }

            
            
        }
        //The type 'string' must be a non-nullable value type in order to use it as parameter 'T' in the generic type or method 'System.Nullable<T>'
        

        public ActionResult Struttura(long id, string gUserID, string TipoP,string submitButton)
        {
            if (TempData["ViewData"] != null)
            {
                foreach (KeyValuePair<string, object> kvp in (IEnumerable<KeyValuePair<string, object>>)TempData["ViewData"])
                {
                    ViewData.Add(kvp);
                }
            }
            if (TipoP.IndexOf("Tipo ") >= 0)
                TipoP = TipoP.Substring(5, TipoP.Length - 5);
            GetFundPicking(id, gUserID, TipoP);



            //PortFoglio pf = new PortFoglio();

            //FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioDTO pfDTO =
            //        pf.GetInfoPTF(new Guid(gUserID), id);

            string DeveloppingApplication = GenericFunction.ReadConfig("DeveloppingApplication");
            if (string.IsNullOrEmpty( submitButton))
                return View();
            else
                if (DeveloppingApplication == "1")
                    return View();
                else
                {
                    PortFoglio pf = new PortFoglio();

                    FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioDTO pfDTO =
                            pf.GetInfoPTF(new Guid(gUserID), id);
                    if (pfDTO._List.Count > 0)
                    {
                        if (pfDTO._List.Count == 1)
                        {
                            if (pfDTO._List[0]._fidaCode == "XXLIQ")
                                return View();
                            else
                            {
                                if (logger.IsDebugEnabled)
                                    logger.Debug("RICHIAMO NICOLA");
                                return RedirectToAction("Operations", new { id = id, gUserID = gUserID });
                            }
                        }
                        else
                        {
                            if (logger.IsDebugEnabled)
                                logger.Debug("RICHIAMO NICOLA");
                            return RedirectToAction("Operations", new { id = id, gUserID = gUserID });
                        }
                    }
                    else
                        return View();
                }   

        }


        [Authorize]
        public ActionResult Picking()
        {
            if (this.Request.IsAuthenticated)
            {

                System.Guid g = System.Guid.Empty;
                System.Web.Security.MembershipUser memberUser = System.Web.Security.Membership.GetUser(this.User.Identity.Name);
                ViewData["User_ProviderUserKey"] = memberUser.ProviderUserKey.ToString();
                g = new Guid(ViewData["User_ProviderUserKey"].ToString());


                GetFundPicking(1, g.ToString(), string.Empty);

                return View("FromFP");

            }
            return View("FromFP");

        }

        
        public JsonResult GetOperationPortfoglio(long id, string gUserID)
        {
            string[] arUser = null;
            arUser = gUserID.Split('_');
            gUserID = arUser[0];
            PortFoglio pf = new PortFoglio();

            FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioDTO pfDTO =
                    pf.GetInfoPTF(new Guid(gUserID), id);
            Products p = new Products();

            string[] lstID = new string[1];
            string[] lstFields = new string[1];

            string FundName = string.Empty;

            List<PtfOperation> lop = new List<PtfOperation>();
            foreach (PortfolioOperationDTO op in pfDTO._List)
            {
                lstID[0] = op._fidaCode;
                lstFields[0] = "NAME";
                if (op._fidaCode == "XXLIQ")
                    FundName = "Liquidità";
                else
                    FundName = p.getGenericFields(lstID, lstFields).Rows[0]["NAME"].ToString();
                PtfOperation ptfOp = new PtfOperation
                {
                    _portfolioListId = op._portfolioListId,
                    _fidaCode = op._fidaCode,
                    _fundName = FundName,
                    //_fundName = op._fidaCode,
                    _operationDate =
                          String.Format("{0:d}", op._operationDate),
                    _quantity = op._quantity,
                    _unitPrice = op._unitPrice,
                    _price = op._price,
                    _weight = op._weight

                };
                lop.Add(ptfOp);
            }

            JsonResult jsr = Json(lop, JsonRequestBehavior.AllowGet);

            return jsr;
        }

        
        public JsonResult GetFundPrize(long id, string FundID, string noCache, string gUserID) 
        {
            string prize = string.Empty;
            string[] arDate = null;
            DateTime dtStart;
            DateTime dtEnd;

            arDate = noCache.Split('_');
            dtStart = Convert.ToDateTime(arDate[0]);
            dtEnd = Convert.ToDateTime(arDate[0]);

            PortFoglio pf = new PortFoglio();
            System.Guid g = new Guid(gUserID);

            PortfolioDTO pfDTO = pf.GetInfoPTF(g, id);        
            PtfInfoPrize ptfPrize = new PtfInfoPrize();
            ptfPrize=pf.getInfoPrizeFund(FundID, dtStart, dtEnd);

            JsonResult jsr = Json(ptfPrize, JsonRequestBehavior.AllowGet);

            return jsr;
        }
        
        [Utils.AssetAllocationDBInCache]
        public ActionResult TestAA()
        {
            AssetAllocationViewModel aaList = (AssetAllocationViewModel)HttpContext.Cache["assetallocation.db"];
            return View(aaList);
        }

        [Utils.AssetAllocationDBInCache]
        public ActionResult TestCC()
        {
            AssetAllocationViewModel aaList = (AssetAllocationViewModel)HttpContext.Cache["assetallocation.db"];
            return View(aaList);
        }

        public ActionResult TestBB()
        {
           return View();
        }


        [Utils.AssetAllocationDBInCache]
        public ActionResult FromAA()
        {
            AssetAllocationViewModel aaList = (AssetAllocationViewModel)HttpContext.Cache["assetallocation.db"];            
            return PartialView("~/Views/Portfolios/FromAA.ascx", aaList);
        }

        public ActionResult TestTree()
        {

            if (GenericFunction.ReadConfig("DeveloppingApplication") == "1")
            {
                GetDati();

                treeData.Customer.Clear();
                Products p = new Products();
                PortFoglio pfC = new PortFoglio();
                CustomerTree ct = null;
                pFtreeDTO pF = null;

                for (int i = 0; i < dati.portfoglioDTO.Count; i++)
                {
                    ct = new CustomerTree();
                    ct.gUserID = dati.portfoglioDTO[i].gUserID;
                    ct.up = dati.portfoglioDTO[i].up;


                    foreach (PortfolioInfoDTO PFinfo in dati.portfoglioDTO[i].lp)
                    {
                        pF = new pFtreeDTO();
                        pF.pfInfo = PFinfo;

                        FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioDTO pfDTO =
                        pfC.GetInfoPTF(ct.gUserID, PFinfo._portfolioId);

                        pfOperationDTO pfOpDTO = null;
                        string[] lstID = new string[1];
                        string[] lstFields = new string[1];
                        foreach (PortfolioOperationDTO pfO in pfDTO._List)
                        {
                            pfOpDTO = new pfOperationDTO();
                            lstID[0] = pfO._fidaCode;
                            lstFields[0] = "NAME";
                            if (pfO._fidaCode == "XXLIQ")
                                pfOpDTO.FundName = "Liquidità";
                            else
                                pfOpDTO.FundName = p.getGenericFields(lstID, lstFields).Rows[0]["NAME"].ToString();

                            pfOpDTO.pfOpDTO = pfO;
                            pF.lop.Add(pfOpDTO);
                        }
                        ct.lp.Add(pF);
                    }
                    treeData.Customer.Add(ct);
                }

                ViewData["treeData"] = treeData;
            }
            return View();
        }



        public ActionResult FromCopia()
        {

            //if (GenericFunction.ReadConfig("DeveloppingApplication") == "1")
            //{
                GetDati();
                treeData.Customer.Clear();

                Products p = new Products();
                PortFoglio pfC = new PortFoglio();
                CustomerTree ct = null;
                pFtreeDTO pF = null;

                for (int i = 0; i < dati.portfoglioDTO.Count; i++)
                {
                    ct = new CustomerTree();
                    ct.gUserID = dati.portfoglioDTO[i].gUserID;
                    ct.up = dati.portfoglioDTO[i].up;


                    foreach (PortfolioInfoDTO PFinfo in dati.portfoglioDTO[i].lp)
                    {
                        pF = new pFtreeDTO();
                        pF.pfInfo = PFinfo;

                        FidaWorkstation.Professional.Data.Service.Interfaces.PortfolioDTO pfDTO =
                        pfC.GetInfoPTF(ct.gUserID, PFinfo._portfolioId);

                        pfOperationDTO pfOpDTO=null;
                        string[] lstID = new string[1];
                        string[] lstFields = new string[1];
                        foreach (PortfolioOperationDTO pfO in pfDTO._List)
                        {
                            pfOpDTO=new pfOperationDTO() ;                            
                            lstID[0] = pfO._fidaCode;
                            lstFields[0] = "NAME";
                            if (pfO._fidaCode == "XXLIQ")
                                pfOpDTO.FundName = "Liquidità";
                            else
                                pfOpDTO.FundName = p.getGenericFields(lstID, lstFields).Rows[0]["NAME"].ToString();

                            pfOpDTO.pfOpDTO = pfO;
                            pF.lop.Add(pfOpDTO);
                        }
                        ct.lp.Add(pF);
                    }
                    treeData.Customer.Add(ct);
                }

                ViewData["treeData"] = treeData;
            //}
            return PartialView("~/Views/Portfolios/FromCopia.ascx" );
        }


        public ActionResult BenckMark()
        {
            AssetAllocationViewModel aaList = (AssetAllocationViewModel)HttpContext.Cache["assetallocation.db"];
            return View(aaList);
        }


        public ActionResult BenckMarkDetails(long? id)
        {
            
            AssetAllocationDTO myAA = null;
            if (id != null)
            {
                AssetAllocationViewModel aaList = (AssetAllocationViewModel)HttpContext.Cache["assetallocation.db"];
                myAA = (from c in aaList.AssetAllocations
                        select c)
                        .Where(c => c.ID == id).FirstOrDefault();
            }

            return View("FromAA", "", myAA);
        }



        [HttpPost]
        public ActionResult Struttura(string submitButton, int id, FormCollection collection)
        {
            try
            {
                switch (submitButton)
                {
                    case "Successivo":
                        return RedirectToAction("Struttura", new { id = id, gUserID = collection["UserID"], TipoP = collection["TipoPortafoglio"] });
                    case "Salva":

                        System.Guid g = new Guid(collection["UserID"]);
                        PortFoglio pf = new PortFoglio();
                        Int64 ret = pf.SavePTFOperations(g, id, collection);
                        return RedirectToAction("Struttura", new { id = id, gUserID = collection["UserID"], TipoP = collection["TipoPortafoglio"], submitButton=submitButton });
                    case "Precedente":
                        return RedirectToAction("Edit", new { id = id, gUserID = collection["UserID"] });
                    default:
                        return View();
                }
            }
            catch
            {
                GetFundPicking(id, collection["UserID"], collection["TipoPortafoglio"]);
                return View();
            }
        }

        //
        // GET: /Portfolios/Delete/5

        public ActionResult Delete(long id, string gUserID)
        {
            PortFoglio pf = new PortFoglio();


            System.Guid g = System.Guid.Empty;

            g = new System.Guid(gUserID);
            pf.DeletePTF(g, id);
            ViewData["DeletePortfolio"] = gUserID;

            TempData["ViewData"] = ViewData;
            return RedirectToAction("Index");
        }

        //
        // POST: /Portfolios/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }




        [HttpPost]
        public ActionResult Interview(InterviewPortfoglioViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            return this.View("Index");
        }

        public ActionResult Add(string id, int? PFB)
        {

            if (PFB != null) ViewData["PortfolioBenchmark"] = PFB;
            var viewModel = new InterviewPortfoglioViewModel();
            viewModel.UserID = id;

            this.ViewData.Model = viewModel;

            return View("Interview");




        }

        //controller per dettaglio portafoglio
        public ActionResult Operations(long id, string gUserID, bool? isReport)
        {

            try
            {
                if (logger.IsDebugEnabled)
                    logger.Debug(string.Format("Entro in operation con id:{0} gUserID:{1}", id.ToString(), gUserID));

                PortFoglio pf = new PortFoglio();
                System.Guid g = new Guid(gUserID);

                PortfolioDTO pfDTO = pf.GetInfoPTF(g, id);
                if (logger.IsDebugEnabled)
                    logger.Debug("Eseguito pf.GetInfoPTF(g, id)");


                //preparazione del model
                PortFoglioDetailViewModel pfDVM = new PortFoglioDetailViewModel();

                pfDVM.pfinfo = pfDTO._portfolioInfo;
                pfDVM.lpo = pfDTO._List;

                UserProfile up = null;
                MembershipUser us = Membership.GetUser(g);
                if (us != null) up = UserProfile.GetUserProfile(us.UserName);

                if (logger.IsDebugEnabled)
                    logger.Debug("Eseguito  Membership.GetUser(g) e UserProfile.GetUserProfile(us.UserName)");

                //string[] ute = Roles.FindUsersInRole(Utils.GenericFunction.BuildCustomerRoleName(getCustomerRoleSuffix()),
                //                                    User.Identity.Name + "#%");
                //foreach (var u in ute)
                //{
                //    if (u.Contains(gUserID))
                //    {
                //        up = UserProfile.GetUserProfile(Membership.GetUser(u).UserName);
                //        break;
                //    }

                //} 
                pfDVM.up = up;


                //if (TempData["ViewData"] != null)
                //{
                //    foreach (KeyValuePair<string, object> kvp in (IEnumerable<KeyValuePair<string, object>>)TempData["ViewData"])
                //    {
                //        ViewData.Add(kvp);
                //    }
                //}

                //dati e storico da WS
                FWSDataTable fwsDT = null;
                FWSHistDataTable fwsHist = null;

                //recupero i dati del dettaglio per tutti i prodotti del portafoglio
                Products p = new Products();
                List<string> strum = new List<string>();
                //key per la cache
                string cKey = "";
                foreach (PortfolioOperationDTO item in pfDTO._List)
                {
                    //tra gli strumenti non metto la Liquidità
                    if (!"XXLIQ".Equals(item._fidaCode))
                        strum.Add(item._fidaCode);
                    cKey += item._fidaCode + "_";
                }
                if (logger.IsDebugEnabled)
                    logger.Debug("Eseguito  ciclo");

                if (strum.Count > 0)
                {
                    //fwsDT = p.getAllFields(strum.ToArray());
                    fwsDT = p.getFieldsForDetail(strum.ToArray());
                    if (logger.IsDebugEnabled)
                        logger.Debug("Eseguito  getFieldsForDetail");

                    //aggiungo in cache
                    string strObjKey = GenericFunction.getFidaCodeForCache(null, cKey);
                    fwsDT = (FWSDataTable)GenericFunction.RetrieveSetCache(strObjKey, fwsDT);
                    if (logger.IsDebugEnabled)
                        logger.Debug("Eseguito  RetrieveSetCache");

                    //valore CLOSE da FidaXHistoricalDataRepository
                    uint daysToAsk = 100; //10 in produzione
                    fwsHist = pf.getHistData(strum.ToArray(), daysToAsk, pfDTO._portfolioInfo._currency);
                    if (logger.IsDebugEnabled)
                        logger.Debug("Eseguito  getHistData");
                }

                Dictionary<string, string> fc2name = new Dictionary<string, string>();
                //aggiungo la liquidità
                fc2name.Add("XXLIQ", "Liquidità");

                double totPTFValue = 0D;
                foreach (PortfolioOperationDTO ope in pfDVM.lpo)
                {
                    for (int i = 0; i < fwsDT.Rows.Count; i++)
                    {
                        if (((string)fwsDT.Rows[i]["FIDA_CODE"]).Equals(ope._fidaCode))
                        {
                            //carico mappa delle corrispondenze Id - Nome degli strumenti
                            fc2name.Add(ope._fidaCode, (string)fwsDT.Rows[i]["NAME"]);

                            //aggiungo il valore dentro a FWSDataTable 
                            for (int j = 0; j < fwsHist.Rows.Count; j++)
                            {
                                if (fwsHist.Rows[j].Count > 0)
                                {
                                    if (fwsHist.Rows[j].First<FWSHistDataTableRowValue>().ID == ope._fidaCode)
                                    {
                                        if (fwsDT.Rows[i].ContainsKey("_CLOSE_")) fwsDT.Rows[i].Remove("_CLOSE_");
                                        fwsDT.Rows[i].Add("_CLOSE_", fwsHist.Rows[j].Last<FWSHistDataTableRowValue>().Close);

                                        //aggiorno il valore totale del portafoglio (close * quantità)
                                        totPTFValue += (fwsHist.Rows[j].Last<FWSHistDataTableRowValue>().Close * ope._quantity);
                                        break;
                                    }
                                }
                            }
                            //se non ho trovato il codice fida nello storico inserisco la close a null
                            if (!fwsDT.Rows[i].ContainsKey("_CLOSE_"))
                                fwsDT.Rows[i].Add("_CLOSE_", double.NaN);
                            break;
                        }
                    }
                    //aggiorno il totale aggiungendo la liquidità
                    if ("XXLIQ".Equals(ope._fidaCode))
                    {
                        totPTFValue += ope._price;

                        if (logger.IsDebugEnabled)
                            logger.Debug("Eseguito  aggiungo la liquidità");
                    }
                }

                if (logger.IsDebugEnabled)
                    logger.Debug("Eseguito  ciclo foreach (PortfolioOperationDTO ope in pfDVM.lpo)");


                //serie di NAV (passare il portafoglio come parametro??)
                NAVCalculatorDTO nav = pf.getNAV(pfDTO);

                if (logger.IsDebugEnabled)
                    logger.Debug("Eseguito  pf.getNAV(pfDTO)");

                //preparo il modello per la view
                PortFoglioDetailFullModel pfFULL = new PortFoglioDetailFullModel();
                pfFULL.pfDetail = pfDVM;
                pfFULL.fwsData = fwsDT;
                pfFULL.fwsHist = fwsHist;
                pfFULL.fc2Name = fc2name;
                pfFULL.pfValue = totPTFValue;
                pfFULL.NavDTO = nav;


                if (logger.IsDebugEnabled)
                    logger.Debug("Eseguito  preparo il modello per la view");
            

            // valore di default del parametro se assente 
            if (isReport == null) isReport = false;
            ViewData["isReport"] = isReport;
            if (isReport == true) return View("DetailsPortfolio", "Report", pfFULL);

            return View("DetailsPortfolio", pfFULL);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }

            return View("DetailsPortfolio");
        }

        //controller per il report di portafoglio
        public ActionResult ReportPF(long id, string gUserID)
        {
            //qui ricalcolo tutti i dati del report 
            //e li salvo in un oggetto ReportPortFoglio
            //che alla fine viene serializzato in un file xml

            PortFoglio pf = new PortFoglio();
            System.Guid g = new Guid(gUserID);

            //recupero i dati di portafoglio
            PortfolioDTO pfDTO = pf.GetInfoPTF(g, id);

            //recupero i dati utente
            UserProfile up = null;
            MembershipUser mbus = Membership.GetUser(g);
            if (mbus != null) up = UserProfile.GetUserProfile(mbus.UserName);

            //recupero dati e storico da WS
            FWSDataTable fwsDT = null;
            FWSHistDataTable fwsHist = null;
            
            //preparo la lista per tutti gli strumenti del portafoglio
            List<string> strum = new List<string>();
            foreach (PortfolioOperationDTO item in pfDTO._List)
            {
                //tra gli strumenti non metto la Liquidità
                if (!"XXLIQ".Equals(item._fidaCode)) strum.Add(item._fidaCode);
            }
            if (strum.Count > 0)
            {
                //recupero i dati del dettaglio per tutti i prodotti del portafoglio
                Products p = new Products();
                fwsDT = p.getFieldsForDetail(strum.ToArray());

                //valore CLOSE preso da FidaXHistoricalDataRepository
                uint daysToAsk = 100; //10 in produzione
                fwsHist = pf.getHistData(strum.ToArray(), daysToAsk, pfDTO._portfolioInfo._currency);
            }

            //costruisco la mappa delle corrispondenze Id - Nome strumento
            Dictionary<string, string> fc2name = new Dictionary<string, string>();
            //aggiungo la liquidità
            fc2name.Add("XXLIQ", "Liquidità");
            //valore totale del portafoglio
            double totPTFValue = 0D;
            foreach (PortfolioOperationDTO ope in pfDTO._List)
            {
                for (int i = 0; i < fwsDT.Rows.Count; i++)
                {
                    if (((string)fwsDT.Rows[i]["FIDA_CODE"]).Equals(ope._fidaCode))
                    {
                        fc2name.Add(ope._fidaCode, (string)fwsDT.Rows[i]["NAME"]);

                        //aggiungo CLOSE dentro a FWSDataTable prendendolo dallo storico
                        for (int j = 0; j < fwsHist.Rows.Count; j++)
                        {
                            if (fwsHist.Rows[j].Count > 0)
                            {
                                if (fwsHist.Rows[j].First<FWSHistDataTableRowValue>().ID == ope._fidaCode)
                                {
                                    if (fwsDT.Rows[i].ContainsKey("_CLOSE_")) fwsDT.Rows[i].Remove("_CLOSE_");
                                    fwsDT.Rows[i].Add("_CLOSE_", fwsHist.Rows[j].Last<FWSHistDataTableRowValue>().Close);

                                    //aggiorno il valore totale del portafoglio (close * quantità)
                                    totPTFValue += (fwsHist.Rows[j].Last<FWSHistDataTableRowValue>().Close * ope._quantity);
                                    break;
                                }
                            }
                        }
                        //se non ho trovato il codice fida nello storico inserisco la close a null
                        if (!fwsDT.Rows[i].ContainsKey("_CLOSE_")) fwsDT.Rows[i].Add("_CLOSE_", double.NaN);
                        break;
                    }
                }
                //aggiorno il totale aggiungendo la liquidità
                if ("XXLIQ".Equals(ope._fidaCode)) totPTFValue += ope._price;
            }

            //recupero i valori NAV
            NAVCalculatorDTO navCDTO = pf.getNAV(pfDTO);


            //istanzio e popolo nuovo oggetto ReportPortFoglio
            ReportPortFoglio repPf = new ReportPortFoglio();
            repPf.user = up.Nome + " " + up.Cognome;
            repPf.pfName = pfDTO._portfolioInfo._portfolioName;
            repPf.pfCurr = pfDTO._portfolioInfo._currency;
            //tipo di portafoglio: 1,2=AssetAllocation / 3=contabile
            Int64 iType = pfDTO._portfolioInfo._type;
            string pfType = "";
            switch (iType)
            {
                case 1:
                    pfType = "Asset Allocation";
                    break;
                case 2:
                    pfType = "Asset Allocation ribilanciato";
                    break;
                case 3:
                    pfType = "Contabile";
                    break;
            }
            repPf.pfType = pfType;
            repPf.pfBenc = pfDTO._portfolioInfo._benchmarkCode;
            if (navCDTO != null)
            {
                repPf.pfdtFrom = navCDTO.StartDate.ToShortDateString();
                repPf.pfdtTo = navCDTO.EndDate.ToShortDateString();

                //legenda
                repPf.objNavData.NavStart = navCDTO.NavStart.ToString("F2");
                repPf.objNavData.NavLast = navCDTO.NavLast.ToString("F2");
                repPf.objNavData.TWRR100 = Convert.ToSingle(navCDTO.TWRR * 100).ToString("F2") + "%"; 
                YTDPeriodIndicator nav =
                  (from c in navCDTO.StatisticalData.PeriodIndicator
                   select c)
                  .Where(c => c.yearPeriod == 1000).FirstOrDefault();
                if (nav != null)
                {
                    repPf.objNavData.Volatilita = Convert.ToSingle(nav.Volatilita * 100).ToString("F2") + "%";
                    repPf.objNavData.DSR = Convert.ToSingle(nav.DSR * 100).ToString("F2") + "%";
                    repPf.objNavData.Sharpe = nav.Sharpe.ToString("F2");
                    repPf.objNavData.Sortino = navCDTO.StatisticalData.PeriodIndicator[0].Sortino.ToString("F2");
                }

                //---Analisi periodale
                List<YTDPeriodIndicator> navList =
                    (from c in navCDTO.StatisticalData.PeriodIndicator
                     select c)
                    .Where(c => c.yearPeriod != 1000).ToList();
                List<YTDPeriodValue> ytdPV = navCDTO.StatisticalData.RAnni;
                foreach (YTDPeriodIndicator pi in navList)
                {
                    ReportPortFoglio.PeriodRow entryPeriodRow = new ReportPortFoglio.PeriodRow();
                    string sYearPeriod = string.Empty;
                    Single rend = Single.NaN;
                    if (pi.yearPeriod == 0)
                    {
                        sYearPeriod = "YTD";
                        if (navCDTO.StatisticalData.RYTD.Count > 0) rend = navCDTO.StatisticalData.RYTD[0].value * 100;
                    }
                    else 
                    {
                        sYearPeriod = pi.yearPeriod.ToString() + " anni";
                        foreach (YTDPeriodValue pv in ytdPV)
                        {
                            if (pv.yearPeriod == pi.yearPeriod) { rend = pv.value * 100; break; }
                        }

                    }
                    entryPeriodRow.yearPeriod = sYearPeriod;
                    entryPeriodRow.Volatilita = Convert.ToSingle(pi.Volatilita * 100).ToString("F2") + "%";
                    entryPeriodRow.Rend = rend.ToString("F2") + "%"; ;
                    entryPeriodRow.DSR = Convert.ToSingle(pi.DSR * 100).ToString("F2") + "%"; ;
                    entryPeriodRow.Sharpe = pi.Sharpe.ToString("F2");
                    entryPeriodRow.Sortino = pi.Sortino.ToString("F2");

                    repPf.lPeriodList.Add(entryPeriodRow);
                }

                double totPercnt = 0D;
                double totPercntNav = 0D;
                foreach (PortfolioOperationDTO ope in pfDTO._List)
                {
                    string idOpe = ope._fidaCode;
                    string prdName = "";
                    if (!fc2name.TryGetValue(idOpe, out prdName)) prdName = idOpe;
                    double percIni = ope._weight;

                    //individuo la riga corrispondente allo strumento
                    int ind = -1;
                    for (int i = 0; i < fwsDT.Rows.Count; i++)
                    {
                        if ((string)fwsDT.Rows[i]["FIDA_CODE"] == idOpe) { ind = i; break; }
                    }

                    //--valorizzazione e --composizione
                    double price = double.NaN;
                    if (ind >= 0 && fwsDT.Rows[ind].ContainsKey("_CLOSE_")) price = Convert.ToDouble(fwsDT.Rows[ind]["_CLOSE_"]);
                    if (iType > 2)
                    {
                        //CONTABILE
                        double value = double.NaN;
                        if (!price.Equals(double.NaN)) value = ope._quantity * price;
                        //liquidità
                        if (prdName.ToUpper().Contains("LIQUIDIT")) value = ope._price;
                        //valorizzo la percentuale
                        percIni = 0D;
                        if (!value.Equals(double.NaN) && totPTFValue > 0) percIni = (value / totPTFValue) * 100;

                        //nuova entry in lista valorizzazione 
                        ReportPortFoglio.ValorRow_CN entryValorRow = new ReportPortFoglio.ValorRow_CN();
                        entryValorRow.numQuot = prdName.ToUpper().Contains("LIQUIDIT") ? String.Empty : ope._quantity.ToString("F2");
                        entryValorRow.valQuot = prdName.ToUpper().Contains("LIQUIDIT") ? String.Empty : price.ToString("F2");
                        entryValorRow.valore = value.ToString("F2");
                        entryValorRow.perc = percIni.ToString("F2") + "%";
                        entryValorRow.prdName = prdName;
                        repPf.lValorList_CN.Add(entryValorRow);

                        //nuova entry in lista composizione
                        ReportPortFoglio.CompoRow entryCompoRow = new ReportPortFoglio.CompoRow();
                        entryCompoRow.percIni = percIni.ToString("F2") + "%";
                        entryCompoRow.perc = String.Empty;
                        entryCompoRow.prdName = prdName;
                        repPf.lCompoList.Add(entryCompoRow);

                        //aggiorno il totale della percentuale
                        totPercnt += percIni;
                    } 
                    else
                    {
                        //ASSET ALLOCATION
                        double percNav = double.NaN;
                        NAVCalculator_PortfolioOperationDTO navcPO =
                         (from c in navCDTO.OperationList
                          select c)
                         .Where(c => c._fidaCode == idOpe).FirstOrDefault();

                        if (navcPO != null)
                            percNav = navcPO._weight * 100;
                        else
                            percNav = 0;

                        price = ((navCDTO.NavLast * percNav) / 100);

                        //nuova entry in lista valorizzazione
                        ReportPortFoglio.ValorRow_AA entryValorRow = new ReportPortFoglio.ValorRow_AA();
                        entryValorRow.percIni = percIni.ToString("F2") + "%";
                        entryValorRow.perc = percNav.ToString("F2") + "%";
                        entryValorRow.valore = price.ToString("F2");
                        entryValorRow.prdName = prdName;
                        repPf.lValorList_AA.Add(entryValorRow);

                        //nuova entry in lista composizione
                        ReportPortFoglio.CompoRow entryCompoRow = new ReportPortFoglio.CompoRow();
                        entryCompoRow.percIni = percIni.ToString("F2") + "%";
                        entryCompoRow.perc = percNav.ToString("F2") + "%";
                        entryCompoRow.prdName = prdName;
                        repPf.lCompoList.Add(entryCompoRow);

                        //aggiorno i totale della percentuale
                        totPercnt += percIni;
                        totPercntNav += percNav;
                    }

                    //--Analisi1a e --Bluerating
                    if (ind >= 0)
                    {
                        //nuova entry in lista Analisi1a
                        ReportPortFoglio.Ana1aRow entryAna1a = new ReportPortFoglio.Ana1aRow();
                        entryAna1a.prdName = prdName;
                        entryAna1a.prdDate = Convert.ToDateTime(fwsDT.Rows[ind]["PX_DATE"]).ToShortDateString();
                        entryAna1a.prdPrice = price.ToString("F2");
                        entryAna1a.prdPerf = Convert.ToDouble(fwsDT.Rows[ind]["PERF_1Y"]).ToString("F2") + "%";
                        entryAna1a.prdVolat = Convert.ToDouble(fwsDT.Rows[ind]["EFF_DSR_1Y"]).ToString("F2") + "%";
                        entryAna1a.prdNVolat = Convert.ToDouble(fwsDT.Rows[ind]["EFF_STD_DEV_1Y"]).ToString("F2") + "%";
                        entryAna1a.prdSharpe = Convert.ToDouble(fwsDT.Rows[ind]["EFF_SHARPE_1Y"]).ToString("F2");
                        entryAna1a.prdSortino = Convert.ToDouble(fwsDT.Rows[ind]["EFF_SORTINO_1Y"]).ToString("F2");
                        repPf.lAna1aList.Add(entryAna1a);

                        //nuova entry in lista Bluerating
                        ReportPortFoglio.BlueRow entryBlue = new ReportPortFoglio.BlueRow();
                        entryBlue.prdName = prdName;
                        entryBlue.prdPerc = percIni.ToString("F2") + "%";
                        entryBlue.prdBRcat = Convert.ToString(fwsDT.Rows[ind]["BFC_CAT_NAME"]);
                        entryBlue.rating = Convert.ToSingle(fwsDT.Rows[ind]["BFC_RATING_12M"]).ToString();
                        repPf.lBlueList.Add(entryBlue);
                    }

                }//foreach
                //salvo i totali di portafoglio
                if (iType > 2)
                {
                    repPf.pfTotale = totPTFValue.ToString("F2"); //totale portafoglio
                    repPf.pfTotPerc = totPercnt.ToString("F2") + "%";
                    repPf.pfTotPercIni = String.Empty;
                }
                else 
                {
                    repPf.pfTotale = navCDTO.NavLast.ToString("F2");
                    repPf.pfTotPerc = totPercntNav.ToString("F2") + "%";
                    repPf.pfTotPercIni = totPercnt.ToString("F2") + "%";
                }
            }

            //imposto l'attributo id del portafoglio 
            repPf.id = "0";

            //aggiungo il report alla classe contenitore
            ReportPortFoglioContainer repPfCont = new ReportPortFoglioContainer(repPf);
            
            //produco il file xml
            string sXmlFile = "C:/temp/provaFW.xml";
            repPfCont.SerializeToXml(sXmlFile);

            return Operations(id, gUserID, true);
        }

    }
}
